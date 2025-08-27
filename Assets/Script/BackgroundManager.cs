using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    // Menggunakan [Tooltip] untuk memberikan deskripsi di Inspector
    [Tooltip("Seret objek pemain ke sini. Pastikan objek pemain memiliki tag 'Player'.")]
    public Transform playerTransform;

    [Tooltip("Seret kamera utama ke sini.")]
    public Camera mainCamera;

    private float tileSizeX;
    private float tileSizeY;

    void Start()
    {
        // Mencari objek pemain jika belum diset di Inspector
        if (playerTransform == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                playerTransform = playerObject.transform;
            }
        }

        // Mencari kamera utama jika belum diset di Inspector
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        // Mendapatkan ukuran sprite dari background pertama (child pertama)
        if (transform.childCount > 0)
        {
            SpriteRenderer renderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                tileSizeX = renderer.bounds.size.x;
                tileSizeY = renderer.bounds.size.y;
            }
            else
            {
                Debug.LogError("SpriteRenderer tidak ditemukan pada child pertama. Pastikan semua background Anda adalah Sprite.");
            }
        }
    }

    // Menggunakan LateUpdate agar pergerakan background terjadi setelah pergerakan kamera
    void LateUpdate()
    {
        if (mainCamera == null)
        {
            Debug.LogError("Kamera utama tidak ditemukan. Pastikan kamera sudah diset.");
            return;
        }

        // Menghitung posisi target untuk manager berdasarkan posisi kamera.
        // Mathf.RoundToInt membulatkan posisi kamera ke kelipatan terdekat dari ukuran tile.
        int targetX = Mathf.RoundToInt(mainCamera.transform.position.x / tileSizeX);
        int targetY = Mathf.RoundToInt(mainCamera.transform.position.y / tileSizeY);

        // Menetapkan posisi baru untuk BackgroundManager
        Vector3 targetPosition = new Vector3(targetX * tileSizeX, targetY * tileSizeY, transform.position.z);
        transform.position = targetPosition;
    }
}