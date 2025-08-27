using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public float playerSpeed = 5f;
    public float rotationSpeed = 10f;
    public float deadZoneRadius = 10f;

    private Vector2 touchStartPosition;
    private bool isDragging = false;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D tidak ditemukan. Skrip PlayerMovementController membutuhkan Rigidbody2D.");
        }
    }

    void Update()
    {
        // Mendeteksi hanya sentuhan pertama untuk pergerakan
        if (Input.touchCount > 0)
        {
            Touch moveTouch = Input.GetTouch(0);

            switch (moveTouch.phase)
            {
                case TouchPhase.Began:
                    isDragging = true;
                    touchStartPosition = moveTouch.position;
                    break;

                case TouchPhase.Moved:
                    if (isDragging)
                    {
                        Vector2 currentTouchPosition = moveTouch.position;
                        Vector2 dragVector = currentTouchPosition - touchStartPosition;

                        if (dragVector.magnitude > deadZoneRadius)
                        {
                            Vector2 movementDirection = dragVector.normalized;

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
                    // Hentikan pergerakan saat jari diangkat
                    rb.linearVelocity = Vector2.zero;
                    break;
            }
        }
    }
}