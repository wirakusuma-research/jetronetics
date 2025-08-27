using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // Seret HealthManager dari Inspector
    [SerializeField] private HealthManager linkedHealthManager;

    [SerializeField] private Image positiveEnergyBar;
    [SerializeField] private Image negativeEnergyBar;
    [SerializeField] private Text positiveEnergyText;
    [SerializeField] private Text negativeEnergyText;

    private float targetPositiveFill;
    private float targetNegativeFill;
    public float lerpSpeed = 5f;

    void Start()
    {
        // Periksa apakah referensi sudah diatur
        if (linkedHealthManager == null)
        {
            Debug.LogError("linkedHealthManager belum diatur di HealthBar!");
            return;
        }

        // Langganan ke event kesehatan
        linkedHealthManager.OnHealthChanged.AddListener(UpdateHealthUI);
        // Perbarui UI pertama kali
        UpdateHealthUI();
    }

    // Fungsi yang akan dipanggil setiap kali event OnHealthChanged di-invoke
    private void UpdateHealthUI()
    {
        targetPositiveFill = linkedHealthManager.currentHealthPositif / linkedHealthManager.maxHealthPositif;
        targetNegativeFill = linkedHealthManager.currentHealthNegatif / linkedHealthManager.maxHealthNegatif;

        positiveEnergyText.text = string.Format("{0} / {1}", Mathf.RoundToInt(linkedHealthManager.currentHealthPositif), Mathf.RoundToInt(linkedHealthManager.maxHealthPositif));
        negativeEnergyText.text = string.Format("{0} / {1}", Mathf.RoundToInt(linkedHealthManager.currentHealthNegatif), Mathf.RoundToInt(linkedHealthManager.maxHealthNegatif));
    }

    void Update()
    {
        // Lerp untuk visual yang lebih mulus
        positiveEnergyBar.fillAmount = Mathf.Lerp(positiveEnergyBar.fillAmount, targetPositiveFill, Time.deltaTime * lerpSpeed);
        negativeEnergyBar.fillAmount = Mathf.Lerp(negativeEnergyBar.fillAmount, targetNegativeFill, Time.deltaTime * lerpSpeed);
    }

    private void OnDestroy()
    {
        // Penting: Berhenti mendengarkan saat objek dihancurkan
        if (linkedHealthManager != null)
        {
            linkedHealthManager.OnHealthChanged.RemoveListener(UpdateHealthUI);
        }
    }
}