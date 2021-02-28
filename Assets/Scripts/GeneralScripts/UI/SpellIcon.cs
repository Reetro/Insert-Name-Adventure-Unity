using Spells;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GeneralScripts.UI
{
    public class SpellIcon : MonoBehaviour
    {
        private Image spellIcon;
        private Button spellButton;

        /// <summary>
        /// The actual spell assigned to this slot
        /// </summary>
        public ScriptableSpell MyScriptableSpell { get; private set; }
        /// <summary>
        /// The actual button the spell is on
        /// </summary>
        public ActionButton ParentButton { get; private set; }

        [Tooltip("A reference to the cooldown image on the spell icon")]
        [SerializeField] private Image cooldownImage;

        [Tooltip("A reference to the cooldown image on the spell icon")]
        [SerializeField] private TextMeshProUGUI coolDownText;

        /// <summary>
        /// Set default values and disable button in parent object
        /// </summary>
        /// <param name="spellToCast"></param>
        public void SetupIcon(ScriptableSpell spellToCast)
        {
            ParentButton = transform.GetComponentInParent<ActionButton>();

            MyScriptableSpell = spellToCast;

            spellIcon = GetComponent<Image>();

            spellIcon.sprite = spellToCast.Artwork;

            spellButton = GetComponent<Button>();

            DisplayCooldown(false);

            spellButton.onClick.AddListener(CastSpell);
        }
        /// <summary>
        /// Hide / Show Cooldown info
        /// </summary>
        /// <param name="show"></param>
        public void DisplayCooldown(bool show)
        {
            cooldownImage.enabled = show;

            coolDownText.enabled = show;
        }
        /// <summary>
        /// Update the cooldown countdown timer text
        /// </summary>
        /// <param name="cooldown"></param>
        public void UpdateCooldownText(float cooldown)
        {
            coolDownText.text = cooldown.ToString("f1");
        }

        /// <summary>
        /// Update the fill amount of the cooldown overlay image
        /// </summary>
        /// <param name="cooldown"></param>
        /// <param name="updateFill"></param>
        public void UpdateCooldownFillAmount(float cooldown, bool updateFill)
        {
            if (updateFill)
            {
                cooldownImage.fillAmount -= 1 / cooldown * Time.deltaTime;
            }
        }
        /// <summary>
        /// Reset fill amount of the cooldown overlay image
        /// </summary>
        public void ResetCooldownFilAmount()
        {
            cooldownImage.fillAmount = 1f;
        }

        /// <summary>
        /// Cast the spell at the given index on the action bar
        /// </summary>
        public void CastSpell()
        {
            GeneralFunctions.CastSpell(MyScriptableSpell, this);
        }
    }
}