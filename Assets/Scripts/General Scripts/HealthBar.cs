using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Gradient gradient = null;
    [SerializeField] Image fill = null;
    [SerializeField] TextMeshProUGUI maxHealthText = null;
    [SerializeField] TextMeshProUGUI currentHealthText = null;

    public Slider slider = null;

    public void SetMaxHealth(float maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;

        maxHealthText.text = maxHealth.ToString();
        currentHealthText.text = maxHealth.ToString();

        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(float health)
    {
        slider.value = health;

        currentHealthText.text = health.ToString();

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
