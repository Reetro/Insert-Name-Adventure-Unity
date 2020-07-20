using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayerCharacter.GameSaving;

namespace PlayerUI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Gradient gradient = null;
        [SerializeField] Image fill = null;
        [SerializeField] TextMeshProUGUI maxHealthText = null;
        [SerializeField] TextMeshProUGUI currentHealthText = null;

        public Slider slider = null;
        private static float lastMax = 0;

        public void SetMaxHealth(float maxHealth)
        {
            slider.maxValue = maxHealth;
            slider.value = maxHealth;
            lastMax = maxHealth;

            maxHealthText.text = maxHealth.ToString();
            currentHealthText.text = maxHealth.ToString();

            fill.color = gradient.Evaluate(1f);
        }

        public void SetHealth(float health)
        {
            slider.value = health;
            slider.maxValue = lastMax;

            maxHealthText.text = lastMax.ToString();

            if (slider.value < slider.maxValue)
            {
                currentHealthText.text = health.ToString("F1");
            }
            else
            {
                currentHealthText.text = health.ToString();
            }

            fill.color = gradient.Evaluate(slider.normalizedValue);
        }

        public void SetPlayerHealth(PlayerState playerState)
        {
            slider.maxValue = playerState.GetCurrentMaxHealth();
            slider.value = playerState.GetCurrentHealth();

            maxHealthText.text = playerState.GetCurrentMaxHealth().ToString();

            if (slider.value < slider.maxValue)
            {
                currentHealthText.text = playerState.GetCurrentHealth().ToString("F1");
            }
            else
            {
                currentHealthText.text = playerState.GetCurrentHealth().ToString();
            }

            fill.color = gradient.Evaluate(slider.normalizedValue);
        }
    }
}