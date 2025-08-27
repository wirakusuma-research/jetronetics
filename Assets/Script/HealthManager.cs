using UnityEngine;
using UnityEngine.Events;

public abstract class HealthManager : MonoBehaviour
{
    [Header("Positif Health")]
    [SerializeField] public float maxHealthPositif = 100f;
    [SerializeField] public float currentHealthPositif;

    [Header("Negatif Health")]
    [SerializeField] public float maxHealthNegatif = 100f;
    [SerializeField] public float currentHealthNegatif;

    [HideInInspector] public bool isAlive = true;

    // Ini adalah UnityEvent yang akan dipanggil saat kesehatan berubah.
    // Kita tidak perlu parameter karena HealthBar akan mengambil data langsung dari HealthManager.
    public UnityEvent OnHealthChanged;

    protected virtual void Start()
    {
        currentHealthPositif = maxHealthPositif;
        currentHealthNegatif = maxHealthNegatif;

        // Panggil event di awal untuk memastikan HealthBar diupdate
        OnHealthChanged?.Invoke();
    }

    public virtual void TakeDamage(Bullet.ChargeType bulletChargeType, float damageAmount)
    {
        if (!isAlive) return;

        if (bulletChargeType == Bullet.ChargeType.Positive)
        {
            currentHealthNegatif -= damageAmount;
            currentHealthPositif = Mathf.Min(currentHealthPositif + damageAmount, maxHealthPositif);
        }
        else if (bulletChargeType == Bullet.ChargeType.Negative)
        {
            currentHealthPositif -= damageAmount;
            currentHealthNegatif = Mathf.Min(currentHealthNegatif + damageAmount, maxHealthNegatif);
        }
        else if (bulletChargeType == Bullet.ChargeType.Neutral)
        {
            currentHealthPositif -= damageAmount / 2;
            currentHealthNegatif -= damageAmount / 2;
        }

        // Pastikan tidak ada yang negatif
        currentHealthPositif = Mathf.Max(currentHealthPositif, 0);
        currentHealthNegatif = Mathf.Max(currentHealthNegatif, 0);

        // Trigger event perubahan health
        OnHealthChanged?.Invoke();

        if (currentHealthPositif <= 0 || currentHealthNegatif <= 0)
        {
            Die();
        }
    }


    public void ReduceHealthOnFire(Bullet.ChargeType bulletChargeType, float cost)
    {
        if (!isAlive) return;

        if (bulletChargeType == Bullet.ChargeType.Positive)
        {
            currentHealthPositif -= cost;
        }
        else if (bulletChargeType == Bullet.ChargeType.Negative)
        {
            currentHealthNegatif -= cost;
        }

        currentHealthPositif = Mathf.Max(currentHealthPositif, 0);
        currentHealthNegatif = Mathf.Max(currentHealthNegatif, 0);

        // Panggil event agar semua "Subscriber" tahu bahwa kesehatan berubah
        OnHealthChanged?.Invoke();

        if (currentHealthPositif <= 0 || currentHealthNegatif <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        isAlive = false;
        Debug.Log(gameObject.name + " telah dihancurkan!");
    }
}