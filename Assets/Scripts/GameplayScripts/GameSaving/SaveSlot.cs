using GeneralScripts;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameplayScripts.GameSaving
{
    public class SaveSlot : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private int slotNumber = 1;
        [SerializeField] private TextMeshProUGUI activeText = null;
        [SerializeField] private MainMenuSaveManager saveManager = null;
        [SerializeField] private TextMeshProUGUI slotText = null;
        [SerializeField] private Button slotButton = null;

        private int clickCount = 0;

        /// <summary>
        /// Gets the slot number
        /// </summary>
        public int SlotNumber => slotNumber;
        /// <summary>
        /// Get the actual slot button
        /// </summary>
        public Button SlotButton => slotButton;

        private void OnEnable()
        {
            UpdateSlotText();
        }
        /// <summary>
        /// When slot is selected update color
        /// </summary>
        public void OnSlotSelected()
        {
            slotButton.Select();

            saveManager.CurrentSaveSlot = this;
        }
        /// <summary>
        /// Checks to see if there is a save slot in the given slot if not set text to empty
        /// </summary>
        public void UpdateSlotText()
        {
            if (slotText)
            {
                if (!GeneralFunctions.DoesSaveExistInSlot(slotNumber))
                {
                    slotText.text = "Empty";

                    activeText.enabled = false;
                }
                else
                {
                    activeText.enabled = GeneralFunctions.IsSlotActive(slotNumber);
                }
            }
        }
        /// <summary>
        /// Count mouse clicks if double click either load the selected game or start a new game depending on if the slot is empty or not
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerClick(PointerEventData eventData)
        {
            clickCount = eventData.clickCount;

            if (clickCount != 2) return;
            
            clickCount = 0;

            if (GeneralFunctions.DoesSaveExistInSlot(slotNumber))
            {
                GeneralFunctions.LoadGameFromSlot(slotNumber);
            }
            else
            {
                GeneralFunctions.CreateNewSave(slotNumber);
            }
        }
    }
}