using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class PlayerShootingController : MonoBehaviour
{
    // Menggunakan List untuk menampung semua senjata yang dimiliki
    [Header("Weapons")]
    public List<Weapon> weapons;

    private int currentWeaponIndex = 0;

    // Properti publik untuk mendapatkan senjata yang sedang aktif
    public Weapon activeWeapon { get; private set; }

    // Event baru yang akan dipanggil saat senjata beralih
    public UnityEvent<int> OnWeaponSwitched;

    private PlayerHealth playerHealth;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth tidak ditemukan pada pemain.");
        }

        if (weapons.Count > 0)
        {
            activeWeapon = weapons[currentWeaponIndex];
            SetAllWeaponsInactive();
            activeWeapon.gameObject.SetActive(true);

            // Panggil event di awal untuk memastikan UI terbarui
            OnWeaponSwitched?.Invoke(currentWeaponIndex);
        }
        else
        {
            Debug.LogError("Tidak ada senjata yang ditambahkan ke PlayerShootingController!");
        }
    }

    void Update()
    {
        if (Input.touchCount >= 2)
        {
            Touch shootTouch = Input.GetTouch(1);
            if (shootTouch.phase == TouchPhase.Began)
            {
                TryShoot();
            }
        }
    }

    void TryShoot()
    {
        if (activeWeapon == null || activeWeapon.firePointConfigs.Count == 0)
        {
            Debug.LogError("Tidak ada senjata atau konfigurasi fire point yang aktif!");
            return;
        }

        float fireCost = activeWeapon.fireCost;
        float totalPositiveCost = 0;
        float totalNegativeCost = 0;

        // Hitung total biaya dari semua fire point
        foreach (FirePointConfig config in activeWeapon.firePointConfigs)
        {
            if (config.chargeType == Bullet.ChargeType.Positive)
            {
                totalPositiveCost += fireCost;
            }
            else if (config.chargeType == Bullet.ChargeType.Negative)
            {
                totalNegativeCost += fireCost;
            }
            else if (config.chargeType == Bullet.ChargeType.Neutral)
            {
                totalPositiveCost += fireCost / 2;
                totalNegativeCost += fireCost / 2;
            }
        }

        // Pengecekan akhir sebelum menembak
        if (playerHealth.currentHealthPositif >= totalPositiveCost && playerHealth.currentHealthNegatif >= totalNegativeCost)
        {
            // Kurangi kesehatan pemain
            playerHealth.currentHealthPositif -= totalPositiveCost;
            playerHealth.currentHealthNegatif -= totalNegativeCost;

            // Perintah senjata untuk menembak
            activeWeapon.Shoot(gameObject.tag);

            // Panggil event untuk memperbarui UI
            playerHealth.OnHealthChanged?.Invoke();

            //Debug.Log("Pemain menembak menggunakan senjata " + activeWeapon.gameObject.name +
            //          ". Biaya: Positif " + totalPositiveCost + ", Negatif " + totalNegativeCost);
        }
        else
        {
            Debug.Log("Tidak cukup health untuk menembak!");
        }
    }

    /// <summary>
    /// Beralih ke senjata berikutnya dalam list secara berurutan.
    /// </summary>
    public void SwitchWeapon()
    {
        // Nonaktifkan senjata yang saat ini aktif
        if (activeWeapon != null)
        {
            activeWeapon.gameObject.SetActive(false);
        }

        currentWeaponIndex++;

        // Jika sudah mencapai akhir list, kembali ke awal
        if (currentWeaponIndex >= weapons.Count)
        {
            currentWeaponIndex = 0;
        }

        activeWeapon = weapons[currentWeaponIndex];
        activeWeapon.gameObject.SetActive(true);

        OnWeaponSwitched?.Invoke(currentWeaponIndex);

        Debug.Log("Beralih ke senjata: " + activeWeapon.gameObject.name);
    }

    /// <summary>
    /// Beralih ke senjata pada indeks tertentu.
    /// </summary>
    /// <param name="index">Indeks senjata yang ingin diaktifkan.</param>
    public void SwitchToWeapon(int index)
    {
        // Validasi indeks
        if (index < 0 || index >= weapons.Count)
        {
            Debug.LogError("Indeks senjata tidak valid: " + index);
            return;
        }

        // Hentikan proses jika senjata yang dipilih sudah aktif
        if (activeWeapon == weapons[index])
        {
            return;
        }

        // Nonaktifkan senjata yang saat ini aktif
        if (activeWeapon != null)
        {
            activeWeapon.gameObject.SetActive(false);
        }

        currentWeaponIndex = index;

        activeWeapon = weapons[currentWeaponIndex];
        activeWeapon.gameObject.SetActive(true);

        OnWeaponSwitched?.Invoke(currentWeaponIndex);

        Debug.Log("Beralih langsung ke senjata: " + activeWeapon.gameObject.name);
    }

    private void SetAllWeaponsInactive()
    {
        foreach (Weapon weapon in weapons)
        {
            if (weapon != null)
            {
                weapon.gameObject.SetActive(false);
            }
        }
    }

    public int GetCurrentWeaponIndex()
    {
        return currentWeaponIndex;
    }
}