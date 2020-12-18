using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using PlayerUI.ToolTipUI;
using UnityEngine.EventSystems;

namespace PlayerUI
{
    public class ActionButton : MonoBehaviour, IDropHandler
    {
        [SerializeField] private TextMeshProUGUI textAsset = null;

        private SpellIcon mySpellIcon = null;
        private ItemTooltip myItemTooltip = null;

        public void SetupActionSlot(string slotButton, InputAction inputAction)
        {
            textAsset.text = slotButton;

            inputAction.started += ctx => CastSpell();

            myItemTooltip = GetComponent<ItemTooltip>();
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

            if (mySpellIcon)
            {
                myItemTooltip.enabled = true;

                myItemTooltip.SetItem(mySpellIcon.MyScriptableSpell);
            }
        }
        /// <summary>
        /// Remove the current spell from slot
        /// </summary>
        public void RemoveIcon()
        {
            mySpellIcon = null;

            myItemTooltip.enabled = false;
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag)
            {
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;

                eventData.pointerDrag.transform.SetParent(transform);

                var spell = eventData.pointerDrag.transform.GetComponent<SpellIcon>();

                if (spell)
                {
                    SetSpellIcon(spell);
                }
                else
                {
                    Debug.LogError("Spell Drop Failed to get spell icon");
                }
            }
        }

        #region Properties
        /// <summary>
        /// Reference to the Keycode assigned to this object
        /// </summary>
        public KeyCode MyKeyCode { get; private set; }
        #endregion
    }
}