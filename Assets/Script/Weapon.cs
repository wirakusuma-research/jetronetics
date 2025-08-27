using UnityEngine;
using System.Collections.Generic;
using System;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Properties")]
    // Prefab peluru yang akan ditembakkan
    public Sprite bulletIcon;
    public GameObject bulletPrefab;

    // Biaya kesehatan untuk menembakkan senjata ini (tetap satu nilai)
    public float fireCost = 10f;

    // Gunakan List untuk menampung banyak fire point dan konfigurasinya
    public List<FirePointConfig> firePointConfigs;

    // Fungsi untuk menembak, yang akan dipanggil dari luar
    public void Shoot(string shooterTag)
    {
        if (bulletPrefab == null)
        {
            Debug.LogError("Weapon: Properti Bullet Prefab belum diatur!");
            return;
        }

        // Cek apakah list konfigurasi kosong
        if (firePointConfigs == null || firePointConfigs.Count == 0)
        {
            Debug.LogError("Weapon: List Fire Point Configs kosong!");
            return;
        }

        // Loop melalui setiap konfigurasi fire point
        foreach (FirePointConfig config in firePointConfigs)
        {
            // Pastikan fire point tidak null sebelum menembak
            if (config.firePoint == null)
            {
                Debug.LogWarning("Weapon: Ada Fire Point yang belum diatur, lewati.");
                continue; // Lanjut ke fire point berikutnya
            }

            // Buat instance peluru di posisi fire point saat ini
            GameObject newBullet = Instantiate(bulletPrefab, config.firePoint.position, config.firePoint.rotation);

            // Atur properti peluru dari data konfigurasi
            Bullet bulletScript = newBullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.chargeType = config.chargeType;
                bulletScript.chargeValue = config.chargeValue;
                bulletScript.shooterTag = shooterTag;
            }
        }
    }
}

[Serializable]
public class FirePointConfig
{
    // Titik asal peluru akan keluar
    public Transform firePoint;

    // Tipe muatan dan nilai peluru untuk fire point ini
    public Bullet.ChargeType chargeType;
    public float chargeValue;
}