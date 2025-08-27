using UnityEngine;

public class BackgroundLooping2D : MonoBehaviour
{
    private float backgroundWidth;
    private float backgroundHeight;

    private Transform playerTransform;

    void Start()
    {
        // Temukan transform pemain. Ganti "Player" dengan nama objek pemain Anda.
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // Ambil ukuran background. Gunakan BoxCollider2D atau SpriteRenderer.bounds.size
        // Menggunakan BoxCollider2D lebih fleksibel.
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        backgroundWidth = collider.size.x;
        backgroundHeight = collider.size.y;
    }

    void Update()
    {
        // Pindahkan background jika pemain terlalu jauh
        if (playerTransform == null) return;

        // Looping secara vertikal
        if (playerTransform.position.y > transform.position.y + backgroundHeight / 2)
        {
            float verticalOffset = backgroundHeight * 2f;
            transform.position = (Vector2)transform.position + new Vector2(0, verticalOffset);
        }
        else if (playerTransform.position.y < transform.position.y - backgroundHeight / 2)
        {
            float verticalOffset = backgroundHeight * 2f;
            transform.position = (Vector2)transform.position - new Vector2(0, verticalOffset);
        }

        // Looping secara horizontal
        if (playerTransform.position.x > transform.position.x + backgroundWidth / 2)
        {
            float horizontalOffset = backgroundWidth * 2f;
            transform.position = (Vector2)transform.position + new Vector2(horizontalOffset, 0);
        }
        else if (playerTransform.position.x < transform.position.x - backgroundWidth / 2)
        {
            float horizontalOffset = backgroundWidth * 2f;
            transform.position = (Vector2)transform.position - new Vector2(horizontalOffset, 0);
        }
    }
}