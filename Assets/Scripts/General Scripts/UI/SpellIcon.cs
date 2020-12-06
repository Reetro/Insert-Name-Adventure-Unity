using UnityEngine;
using UnityEngine.UI;
using Spells;

namespace PlayerUI
{
    public class SpellIcon : MonoBehaviour
    {
        private ScriptableSpell scriptableSpell = null;
        private Image spellIcon = null;
        private Button spellButton = null;

        /// <summary>
        /// Where this spell is located on the Actionbar
        /// </summary>
        public int SpellIndex { get; private set; }

        /// <summary>
        /// Set default values and disable button in parent object
        /// </summary>
        /// <param name="spellToCast"></param>
        /// <param name="index"></param>
        public void SetupIcon(ScriptableSpell spellToCast, int index)
        {
            scriptableSpell = spellToCast;
            SpellIndex = index;

            spellIcon = GetComponent<Image>();

            spellIcon.sprite = spellToCast.Artwork;

            spellButton = GetComponent<Button>();

            spellButton.onClick.AddListener(CastSpell);
        }
        /// <summary>
        /// Cast the spell at the given index on the action bar
        /// </summary>
        /// <param name="spellIndex"></param>
        public void CastSpell()
        {
            var spellToCast = Instantiate(scriptableSpell.SpellToSpawn);

            GeneralFunctions.StartSpellCast(spellToCast, scriptableSpell);
        }
    }
}