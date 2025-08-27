using UnityEngine;
using UnityEngine.UI;

public class Ampheres : MonoBehaviour
{
    [Header("Electric Current")]
    public float current;
    public Text currentText;

    [Header("Magnetic Field Strength (optional)")]
    public float magneticField;

    private HealthManager touchedPlayer;


    private void Start()
    {
        if (currentText)
            currentText.text = "I: " + (current >= 0 ? "+" : "-") + Mathf.Abs(current).ToString();
    }

    private void Update()
    {
        if (touchedPlayer)
        {
            // Hitung arah arus dari tanda current
            if (current > 0)
            {
                touchedPlayer.TakeDamage(Bullet.ChargeType.Positive, current * Time.deltaTime);
            }
            else if (current < 0)
            {
                touchedPlayer.TakeDamage(Bullet.ChargeType.Negative, Mathf.Abs(current) * Time.deltaTime);
            }

            // Medan magnet (opsional, kalau mau dipakai)
            magneticField = 0.001f * current; // konstanta skala biar pas di game

            //Debug.Log($"{touchedPlayer.gameObject.name} terkena arus {current} A dengan B = {magneticField}");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Enemy"))
        {
            touchedPlayer = collision.GetComponent<HealthManager>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            touchedPlayer = null;
        }
    }
}
