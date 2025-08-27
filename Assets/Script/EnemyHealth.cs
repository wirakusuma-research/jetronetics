using UnityEngine;

public class EnemyHealth : HealthManager
{
    [Header("Score")]
    [Tooltip("Skor yang diberikan saat musuh dihancurkan.")]
    [SerializeField] private int scoreValue = 100;

    protected override void Die()
    {
        // Panggil fungsi Die() dari base class
        base.Die();

        // Tambahkan skor ke GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(scoreValue);
        }

        Debug.Log("Musuh telah dikalahkan! Mendapat " + scoreValue + " poin.");
        Destroy(transform.parent.gameObject);
    }
}