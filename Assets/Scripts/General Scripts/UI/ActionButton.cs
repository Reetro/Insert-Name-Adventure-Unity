using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using PlayerUI.ToolTipUI;

namespace PlayerUI
{
    public class ActionButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textAsset = null;

        private SpellIcon mySpellIcon = null;

        public void SetupActionSlot(string slotButton, InputAction inputAction)
        {
            textAsset.text = slotButton;

            HasSpellInSlot = false;

            inputAction.started += ctx => CastSpell();
        }
        /// <summary>
        /// Casts the spell assigned to this Action button
        /// </summary>
        public void CastSpell()
        {
            if (mySpellIcon)
            {
                mySpellIcon.CastSpell();
            }
        }
        /// <summary>
        /// Sets the action buttons spell icon
        /// </summary>
        /// <param name="spellIcon"></param>
        public void SetSpellIcon(SpellIcon spellIcon)
        {
            mySpellIcon = spellIcon;

            HasSpellInSlot = true;

            if (mySpellIcon)
            {
                GetComponent<ItemTooltip>().SetItem(mySpellIcon.MyScriptableSpell);
            }
        }

        #region Properties
        /// <summary>
        /// Whether or not a spell is active in this ActionButton
        /// </summary>
        public bool HasSpellInSlot { get; private set; } = false;
        #endregion
    }
}