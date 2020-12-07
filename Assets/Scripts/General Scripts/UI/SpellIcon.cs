using UnityEngine;
using UnityEngine.UI;
using Spells;
using TMPro;

namespace PlayerUI
{
    public class SpellIcon : MonoBehaviour
    {
        private Image spellIcon = null;
        private Button spellButton = null;

        /// <summary>
        /// Where this spell is located on the Actionbar
        /// </summary>
        public int SpellIndex { get; private set; }
        /// <summary>
        /// The actual spell assigned to this slot
        /// </summary>
        public ScriptableSpell MyScriptableSpell { get; private set; }

        [Tooltip("A reference to the cooldown image on the spell icon")]
        [SerializeField] private Image cooldownImage = null;

        [Tooltip("A reference to the cooldown image on the spell icon")]
        [SerializeField] private TextMeshProUGUI coolDownText = null;

        /// <summary>
        /// Set default values and disable button in parent object
        /// </summary>
        /// <param name="spellToCast"></param>
        /// <param name="index"></param>
        public void SetupIcon(ScriptableSpell spellToCast, int index)
        {
            MyScriptableSpell = spellToCast;
            SpellIndex = index;

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
        public void UpdateCooldownFillAmount(float cooldown)
        {
            cooldownImage.fillAmount -= 1 / cooldown * Time.deltaTime;
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
        /// <param name="spellIndex"></param>
        public void CastSpell()
        {
            var spellToCast = Instantiate(MyScriptableSpell.SpellToSpawn);

            GeneralFunctions.StartSpellCast(spellToCast, MyScriptableSpell, this);
        }
    }
}