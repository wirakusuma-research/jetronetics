using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovementController : MonoBehaviour
{
    [Header("Movement Properties")]
    [Tooltip("Kecepatan pergerakan musuh.")]
    [SerializeField] private float enemySpeed = 3f;
    [Tooltip("Jarak minimum ke pemain sebelum musuh berhenti.")]
    [SerializeField] private float stoppingDistance = 1.5f;
    [Tooltip("Kecepatan rotasi musuh.")]
    [SerializeField] private float rotationSpeed = 5f;

    [Header("Crowd Avoidance")]
    [Tooltip("Jarak musuh akan mendeteksi musuh lain untuk dihindari.")]
    [SerializeField] private float avoidanceRadius = 2f;
    [Tooltip("Kekuatan dorongan untuk menghindari musuh lain.")]
    [SerializeField] private float avoidanceForce = 5f;

    private Transform playerTarget;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D tidak ditemukan. Skrip EnemyMovementController membutuhkan Rigidbody2D.");
        }

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

    void FixedUpdate()
    {
        if (playerTarget == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 chaseDirection = (playerTarget.position - transform.position).normalized;

        // Hitung arah penghindaran
        Vector2 avoidanceVector = GetAvoidanceVector();

        // Gabungkan vektor pengejaran dan penghindaran
        Vector2 finalDirection = (chaseDirection + avoidanceVector).normalized;

        if (finalDirection.sqrMagnitude > 0)
        {
            float angle = Mathf.Atan2(finalDirection.y, finalDirection.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        float distanceToPlayer = Vector2.Distance(transform.position, playerTarget.position);

        if (distanceToPlayer > stoppingDistance)
        {
            rb.linearVelocity = finalDirection * enemySpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private Vector2 GetAvoidanceVector()
    {
        Vector2 avoidanceVector = Vector2.zero;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, avoidanceRadius);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy") && hitCollider.gameObject != this.gameObject)
            {
                Vector2 directionToOtherEnemy = (hitCollider.transform.position - transform.position);
                float distance = directionToOtherEnemy.magnitude;

                // Hitung kekuatan dorongan berdasarkan jarak
                // Semakin dekat, semakin kuat dorongannya
                if (distance > 0)
                {
                    avoidanceVector -= directionToOtherEnemy.normalized * (1f / distance);
                }
            }
        }

        // Beri bobot pada vektor penghindaran
        return avoidanceVector * avoidanceForce;
    }
}