using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
    public Text muatanPreview;
    public enum ChargeType
    {
        Neutral,
        Positive,
        Negative
    }

    [Header("Bullet Properties")]
    public ChargeType chargeType = ChargeType.Neutral;
    public float chargeValue = 1f;
    public float speed = 10f;
    public float destroySelf = 2f;
    public GameObject impactPrefab;
    public bool dontDestroyOnHit = false;
    public bool printff;

    public string shooterTag;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        UpdateMuatan();



        if (rb == null)
        {
            Debug.LogError("Rigidbody2D tidak ditemukan pada objek peluru.");
        }

        rb.linearVelocity = transform.up * speed;

        // Hancurkan peluru setelah 2 detik
        Destroy(gameObject, destroySelf);
    }

    public void UpdateMuatan()
    {
        if (muatanPreview != null)
        {
            muatanPreview.text = chargeValue.ToString();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Bullet>())
        {
            Bullet targetBullet = other.GetComponent<Bullet>();
            if (targetBullet.chargeType == chargeType)
            {
                //if (targetBullet.chargeValue > chargeValue)
                other.GetComponent<Rigidbody2D>().AddForce(transform.forward * 20);
                UpdateMuatan();

                if (printff)
                {
                    Debug.Log("magnet field mengenai: " + targetBullet.gameObject.name);
                }
            }
        }


        if (other.CompareTag(shooterTag))
        {
            return;
        }

        HealthManager targetHealth = other.GetComponent<HealthManager>();

        if (targetHealth != null)
        {
            // Panggil fungsi TakeDamage dengan tipe muatan dan nilainya
            targetHealth.TakeDamage(chargeType, chargeValue);

            if (printff)
            {
                Debug.Log("magnet field menyerang: " + targetHealth.gameObject.name);
            }
            GameObject impact = Instantiate(impactPrefab);

            if(impact)
                Destroy(impact, 2);

            if(!dontDestroyOnHit)
                Destroy(gameObject);
        }
        else
        {
            //Destroy(gameObject);
        }
    }
}