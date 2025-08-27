using UnityEngine;

public class PlayerDragController : MonoBehaviour
{
    // Kecepatan pergerakan pemain
    public float playerSpeed = 5f;
    // Kecepatan rotasi pemain (seberapa cepat pemain berputar)
    public float rotationSpeed = 10f;

    // Radius 'zona mati' di mana pergerakan tidak terdeteksi
    public float deadZoneRadius = 10f;

    private Vector2 touchStartPosition;
    private bool isDragging = false;

    // Referensi ke Rigidbody2D pemain untuk pergerakan fisik
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D tidak ditemukan. Skrip VirtualJoystickController membutuhkan Rigidbody2D untuk menggerakkan pemain.");
        }
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    isDragging = true;
                    touchStartPosition = touch.position;
                    break;

                case TouchPhase.Moved:
                    if (isDragging)
                    {
                        Vector2 currentTouchPosition = touch.position;
                        Vector2 dragVector = currentTouchPosition - touchStartPosition;

                        if (dragVector.magnitude > deadZoneRadius)
                        {
                            Vector2 movementDirection = dragVector.normalized;

                            // *** KODE YANG DITAMBAHKAN/DIPERBAIKI ***
                            // Hanya putar jika pemain bergerak (untuk menghindari putaran tidak jelas saat kecepatan 0)
                            if (movementDirection.sqrMagnitude > 0)
                            {
                                float angle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;
                                Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
                                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                            }

                            rb.linearVelocity = movementDirection * playerSpeed;
                        }
                        else
                        {
                            rb.linearVelocity = Vector2.zero;
                        }
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    isDragging = false;
                    rb.linearVelocity = Vector2.zero;
                    break;
            }
        }
    }
}