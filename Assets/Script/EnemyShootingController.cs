using UnityEngine;

[RequireComponent(typeof(EnemyHealth))]
public class EnemyShootingController : MonoBehaviour
{
    [Header("Weapon")]
    [SerializeField] private Weapon enemyWeapon;

    [Header("Shooting Behavior")]
    [Tooltip("Waktu tunggu antar tembakan.")]
    [SerializeField] private float fireRate = 2f;
    [Tooltip("Jarak minimum agar musuh bisa menembak.")]
    [SerializeField] private float shootingDistance = 10f;

    private float nextFireTime;
    private EnemyHealth enemyHealth;
    private Transform playerTarget;

    void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        nextFireTime = Time.time;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTarget = player.transform;
        }
        else
        {
            Debug.LogError("Player tidak ditemukan di scene! Pastikan GameObject pemain memiliki tag 'Player'.");
        }
    }

    void Update()
    {
        if (playerTarget == null || enemyWeapon == null || !enemyHealth.isAlive)
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, playerTarget.position);

        // Musuh akan menembak jika berada dalam jarak tembak dan sudah waktunya menembak.
        if (distanceToPlayer <= shootingDistance && Time.time >= nextFireTime)
        {
            TryShoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void TryShoot()
    {
        if (enemyWeapon == null || enemyWeapon.firePointConfigs.Count == 0)
        {
            return;
        }

        float fireCost = enemyWeapon.fireCost;
        float positiveCost = 0;
        float negativeCost = 0;

        foreach (FirePointConfig config in enemyWeapon.firePointConfigs)
        {
            if (config.chargeType == Bullet.ChargeType.Positive)
            {
                positiveCost += fireCost;
            }
            else if (config.chargeType == Bullet.ChargeType.Negative)
            {
                negativeCost += fireCost;
            }
            else if (config.chargeType == Bullet.ChargeType.Neutral)
            {
                positiveCost += fireCost / 2;
                negativeCost += fireCost / 2;
            }
        }

        if (enemyHealth.currentHealthPositif >= positiveCost && enemyHealth.currentHealthNegatif >= negativeCost)
        {
            enemyHealth.currentHealthPositif -= positiveCost;
            enemyHealth.currentHealthNegatif -= negativeCost;

            enemyHealth.OnHealthChanged?.Invoke();

            enemyWeapon.Shoot(gameObject.tag);
            //Debug.Log(gameObject.name + " menembak!");
        }
        else
        {
            //Debug.Log(gameObject.name + " tidak punya cukup health untuk menembak!");
        }
    }
}