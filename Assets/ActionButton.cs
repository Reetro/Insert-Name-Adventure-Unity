using UnityEngine;
using TMPro;
using Spells;

namespace PlayerUI
{
    public class ActionButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textAsset = null;

        public void SetupActionSlot(string slotButton, KeyCode keyCode)
        {
            textAsset.text = slotButton;
            MyKeyCode = keyCode;
        }

        #region Properties
        /// <summary>
        /// Reference to the Keycode assigned to this object
        /// </summary>
        public KeyCode MyKeyCode { get; private set; }
        #endregion
    }
}