using UnityEngine;
using UnityEngine.UI;
using Spells;

namespace PlayerUI
{
    public class SpellIcon : MonoBehaviour
    {
        public ScriptableSpell scriptableSpell = null;
        private int spellIndex = 0;
        private Image spellIcon = null;
        private Button spellButton = null;

        /// <summary>
        /// Set default values and disable button in parent object
        /// </summary>
        /// <param name="spellToCast"></param>
        /// <param name="index"></param>
        public void SetupIcon(ScriptableSpell spellToCast, int index)
        {
            scriptableSpell = spellToCast;
            spellIndex = index;

            GetComponentInParent<Button>().enabled = false;

            spellIcon.sprite = spellToCast.Artwork;

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