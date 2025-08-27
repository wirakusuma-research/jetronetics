using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WeaponListPreview : MonoBehaviour
{
    // Referensi ke PlayerShootingController untuk mendapatkan data senjata
    [SerializeField] private PlayerShootingController playerShootingController;
    [SerializeField] private GameObject switchWeaponButton;
    [SerializeField] private Image currentImageActiveWeapon;
    [SerializeField] private Text currentDamageActiveWeapon;

    // List untuk menampung UI Image (preview senjata)
    [SerializeField] private List<WeaponPreview> weaponPreviewImages;

    void Start()
    {
        if (playerShootingController == null)
        {
            Debug.LogError("WeaponListPreview: PlayerShootingController belum diatur!");
            return;
        }

        // Dapatkan referensi semua senjata dari PlayerShootingController
        List<Weapon> playerWeapons = playerShootingController.weapons;

        // Pastikan jumlah UI Image sama dengan jumlah senjata
        if (playerWeapons.Count != weaponPreviewImages.Count)
        {
            Debug.LogError("Jumlah Weapon di PlayerShootingController tidak sesuai dengan jumlah Image Preview di WeaponListPreview!");
            return;
        }

        // Pasang event listener
        playerShootingController.OnWeaponSwitched.AddListener(UpdateWeaponPreview);

        // Inisialisasi tampilan awal
        UpdateWeaponPreview(playerShootingController.GetCurrentWeaponIndex());


        for (int i = 0; i < weaponPreviewImages.Count; i++)
        {
            weaponPreviewImages[i].playerShootingController = playerShootingController;
            weaponPreviewImages[i].currentIndex = i;
        }
    }

    /// <summary>
    /// Fungsi ini akan dipanggil setiap kali senjata beralih.
    /// </summary>
    /// <param name="activeWeaponIndex">Indeks senjata yang sedang aktif.</param>
    public void UpdateWeaponPreview(int activeWeaponIndex)
    {
        // Iterasi melalui semua gambar preview
        for (int i = 0; i < weaponPreviewImages.Count; i++)
        {
            // Atur Sprite untuk setiap gambar
            weaponPreviewImages[i].imagePreview.sprite = playerShootingController.weapons[i].bulletIcon;

            // Atur warna berdasarkan apakah senjata aktif atau tidak
            if (i == activeWeaponIndex)
            {
                // Senjata aktif: warna penuh dan pekat
                weaponPreviewImages[i].imagePreview.color = Color.white;
                weaponPreviewImages[i].GetComponent<Image>().color = Color.white;
            }
            else
            {
                // Senjata tidak aktif: warna buram (grayscale)
                Color grayColor = new Color(0.5f, 0.5f, 0.5f, 0.5f); // Contoh warna buram
                weaponPreviewImages[i].imagePreview.color = grayColor;
                weaponPreviewImages[i].GetComponent<Image>().color = grayColor;
            }

        }
        currentImageActiveWeapon.sprite = playerShootingController.weapons[activeWeaponIndex].bulletIcon;

        float totalChargeValue = 0;
        foreach(FirePointConfig config in playerShootingController.weapons[activeWeaponIndex].firePointConfigs)
        {
            totalChargeValue += config.chargeValue;
        }
        currentDamageActiveWeapon.text = totalChargeValue.ToString();
        switchWeaponButton.GetComponent<Animator>().SetTrigger("Rotate");
    }

    private void OnDestroy()
    {
        // Lepas listener saat objek dihancurkan untuk menghindari error
        if (playerShootingController != null)
        {
            playerShootingController.OnWeaponSwitched.RemoveListener(UpdateWeaponPreview);
        }
    }
}