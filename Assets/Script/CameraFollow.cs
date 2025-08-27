using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target & Kecepatan")]
    [Tooltip("Seret objek pemain ke sini.")]
    public Transform target;
    [Tooltip("Seberapa cepat kamera mengikuti pemain. Nilai yang lebih tinggi membuat pergerakan lebih cepat dan kurang mulus.")]
    public float smoothSpeed = 0.125f;

    [Header("Offset Posisi")]
    [Tooltip("Jarak kamera dari pemain. Sesuaikan di Inspector.")]
    public Vector3 offset;

    private void LateUpdate()
    {
        // Pastikan target pemain sudah diatur
        if (target == null)
        {
            Debug.LogWarning("Target pemain tidak diatur pada skrip CameraFollow!");
            return;
        }

        // Menghitung posisi yang diinginkan (posisi pemain + offset)
        Vector3 desiredPosition = target.position + offset;

        // Menggunakan Lerp untuk membuat pergerakan kamera lebih mulus
        // Lerp bergerak dari posisi A ke B dengan kecepatan yang diberikan
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}