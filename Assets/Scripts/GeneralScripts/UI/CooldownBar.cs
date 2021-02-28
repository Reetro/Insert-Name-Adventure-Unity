using UnityEngine;
using UnityEngine.UI;

namespace GeneralScripts.UI
{
    public class CooldownBar : MonoBehaviour
    {
        public Slider slider = null;

        private bool isActive = false;

        private void SetCooldownValue(float value)
        {
            slider.value = value;
            slider.maxValue = value;
        }

        public void StartCooldown(float cooldownTime)
        {
            SetCooldownValue(cooldownTime);

            isActive = true;
        }

        public bool GetIsActive()
        {
            return isActive;
        }

        private void Update()
        {
            if (isActive)
            {
                slider.value -= Time.deltaTime;

                if (slider.value <= 0)
                {
                    isActive = false;
                }
            }
        }
    }
}