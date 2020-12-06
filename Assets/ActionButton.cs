﻿using UnityEngine;
using TMPro;

namespace PlayerUI
{
    public class ActionButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textAsset = null;

        private SpellIcon mySpellIcon = null;

        public void SetupActionSlot(string slotButton, KeyCode keyCode)
        {
            textAsset.text = slotButton;
            MyKeyCode = keyCode;
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
        }

        #region Properties
        /// <summary>
        /// Reference to the Keycode assigned to this object
        /// </summary>
        public KeyCode MyKeyCode { get; private set; }
        #endregion
    }
}