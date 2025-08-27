using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [Tooltip("Objek yang akan diikuti.")]
    public Transform target;

    [Tooltip("Kecepatan objek mengikuti target.")]
    public float followSpeed = 5f;

    [Tooltip("Jarak atau posisi offset relatif terhadap target.")]
    public Vector3 offset;

    void Update()
    {
        // Pastikan ada target yang diatur.
        if (target == null)
        {
            Debug.LogWarning("Target belum diatur di skrip FollowTarget!");
            return;
        }

        // Hitung posisi akhir dengan menambahkan offset ke posisi target.
        Vector3 targetPosition = target.position + offset;

        // Pindahkan posisi objek ini ke posisi target.
        // Vector3.Lerp() membuat pergerakan terasa halus.
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }
}