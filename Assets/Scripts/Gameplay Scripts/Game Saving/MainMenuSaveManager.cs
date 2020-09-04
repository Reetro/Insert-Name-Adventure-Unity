using UnityEngine;
using UnityEngine.UI;

namespace PlayerCharacter.GameSaving
{
    public class MainMenuSaveManager : MonoBehaviour
    {
        [SerializeField] private Button continueButton = null;

        private SaveSlot currentSelectedSlot = null;
        /// <summary>
        /// Check to see if there any active save files if not disable continue button
        /// </summary>
        private void Start()
        {
            CheckContinueButton();

        }
        /// <summary>
        /// Checks to see if the continue button can be enabled
        /// </summary>
        private void CheckContinueButton()
        {
            if (continueButton)
            {
                if (GeneralFunctions.IsAnySaveSlotActive())
                {
                    continueButton.interactable = true;
                }
                else
                {
                    continueButton.interactable = false;
                }
            }
        }
        /// <summary>
        /// Loads the current active save file
        /// </summary>
        public void LoadActiveSave()
        {
            GeneralFunctions.LoadActiveSave();
        }
        /// <summary>
        /// Closes the game
        /// </summary>
        public void QuitGame()
        {
            Application.Quit();
        }
        /// <summary>
        /// When every a slot is click update the current selected slot
        /// </summary>
        /// <param name="slotToActivate"></param>
        public void OnSlotSelected(SaveSlot slotToActivate)
        {
            currentSelectedSlot = slotToActivate;
        }
        /// <summary>
        /// Will delete the current selected save slot
        /// </summary>
        public void DeleteSaveSlot()
        {
            if (currentSelectedSlot)
            {
                GeneralFunctions.DeleteGameInSlot(currentSelectedSlot.SlotNumber);

                currentSelectedSlot.UpdateSlotText();

                currentSelectedSlot = null;

                UpdateActiveSlot();

                CheckContinueButton();
            }
        }
        /// <summary>
        /// Gets a new active slot
        /// </summary>
        private void UpdateActiveSlot()
        {
            var allSlots = FindObjectsOfType<SaveSlot>();

            foreach (SaveSlot saveSlot in allSlots)
            {
                if (saveSlot)
                {
                    if (!GeneralFunctions.IsSlotActive(saveSlot.SlotNumber))
                    {
                        GeneralFunctions.SetActiveSlot(saveSlot.SlotNumber);

                        saveSlot.UpdateSlotText();
                    }
                }
            }
        }
    }
}