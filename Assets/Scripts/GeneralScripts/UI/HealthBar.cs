using System.Globalization;
using PlayerScripts.PlayerControls;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GeneralScripts.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Gradient gradient;
        [SerializeField] private Image fill;
        [SerializeField] private TextMeshProUGUI maxHealthText;
        [SerializeField] private TextMeshProUGUI currentHealthText;

        public Slider slider;
        private static float lastMax;

        public void SetMaxHealth(float maxHealth)
        {
            slider.maxValue = maxHealth;
            slider.value = maxHealth;
            lastMax = maxHealth;

            maxHealthText.text = maxHealth.ToString(CultureInfo.InvariantCulture);
            currentHealthText.text = maxHealth.ToString(CultureInfo.InvariantCulture);

            fill.color = gradient.Evaluate(1f);
        }

        public void SetHealth(float health)
        {
            slider.value = health;
            slider.maxValue = lastMax;

            maxHealthText.text = lastMax.ToString(CultureInfo.InvariantCulture);

            currentHealthText.text = slider.value < slider.maxValue ? health.ToString("F1") : health.ToString(CultureInfo.InvariantCulture);

            fill.color = gradient.Evaluate(slider.normalizedValue);
        }

        public void SetPlayerHealth(PlayerState playerState)
        {
            slider.maxValue = PlayerState.MaxHealth;
            slider.value = PlayerState.CurrentHealth;

            maxHealthText.text = PlayerState.CurrentHealth.ToString(CultureInfo.InvariantCulture);

            currentHealthText.text = slider.value < slider.maxValue ? PlayerState.CurrentHealth.ToString("F1") : PlayerState.CurrentHealth.ToString(CultureInfo.InvariantCulture);

            fill.color = gradient.Evaluate(slider.normalizedValue);
        }
    }
}