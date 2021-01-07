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
        private ItemTooltip itemTooltip = null;

        public void SetupActionSlot(string slotButton, InputAction inputAction)
        {
            textAsset.text = slotButton;

            HasSpellInSlot = false;

            itemTooltip = GetComponent<ItemTooltip>();

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
                itemTooltip.SetItem(mySpellIcon.MyScriptableSpell);
            }
        }
        /// <summary>
        /// Remove the current spell from slot
        /// </summary>
        public void RemoveSpellIcon()
        {
            HasSpellInSlot = false;

            itemTooltip.SetItem(null);

            Destroy(mySpellIcon.gameObject);
        }

        #region Properties
        /// <summary>
        /// Whether or not a spell is active in this ActionButton
        /// </summary>
        public bool HasSpellInSlot { get; private set; } = false;
        #endregion
    }
}