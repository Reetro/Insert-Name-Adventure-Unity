using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayerUI.ToolTipUI;

namespace PlayerUI.Icons
{
    public class DebuffIcon : MonoBehaviour
    {
        [SerializeField] private Image durationImage = null;
        [SerializeField] private Image icon = null;
        [SerializeField] TextMeshProUGUI stackText = null;
        [SerializeField] TextMeshProUGUI timer = null;
        [SerializeField] private ItemButton itemButton = null;

        bool hasFillAmount = true;
        private float duration = 0f;
        private float defaultDuration = 0f;

        private void Start()
        {
            itemButton = GetComponent<ItemButton>();

            UpdateStackCount(1);
        }

        void Update()
        {
            if (hasFillAmount && duration > 0)
            {
                durationImage.fillAmount -= 1 / Debuff.GetTotalTime() * Time.deltaTime;

                if (timer.enabled)
                {
                    duration -= Time.deltaTime;

                    UpdateTimerText();
                }
            }
        }
        /// <summary>
        /// Sets all needed values then checks if the given debuff has a tick count if it does will hide both the durationImage and the timer
        /// </summary>
        /// <param name="debuff"></param>
        /// <param name="hasFillAmount"></param>
        /// <param name="useTick"></param>
        public void StartCooldown(ScriptableDebuff debuff, bool hasFillAmount, bool useTick)
        {
            Debuff = debuff;
            icon.sprite = Debuff.Artwork;
            this.hasFillAmount = hasFillAmount;

            itemButton.SetItem(debuff);

            if (useTick)
            {
                durationImage.enabled = true;
                durationImage.fillAmount = 1;

                if (debuff.GetTotalTime() > 0)
                {
                    timer.enabled = true;

                    duration = debuff.GetTotalTime();
                    defaultDuration = duration;

                    UpdateTimerText();
                }
            }
            else
            {
                durationImage.enabled = false;
                timer.enabled = false;
            }
        }
        /// <summary>
        /// Add the given amount to the current stack count if current stack count is less than 1 stack count will be hidden on the icon
        /// </summary>
        /// <param name="stackCount"></param>
        public void UpdateStackCount(int stackCount)
        {
            if (stackText)
            {
                if (stackCount > 1)
                {
                    stackText.enabled = true;

                    stackText.text = stackCount.ToString();
                }
                else
                {
                    stackText.enabled = false;
                }
            }
        }
        /// <summary>
        /// Will reset both the timer and fill image
        /// </summary>
        public void ResetFill()
        {
            durationImage.fillAmount = 1;
            duration = defaultDuration;

            UpdateTimerText();
        }
        /// <summary>
        /// Update timer text to match current duration count
        /// </summary>
        private void UpdateTimerText()
        {
            timer.text = duration.ToString("F1");
        }
        /// <summary>
        /// Get the debuff attached to this icon
        /// </summary>
        public ScriptableDebuff Debuff { get; private set; } = null;
    }
}