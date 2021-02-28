using GeneralScripts;
using UnityEngine;
using UnityEngine.UI;

namespace GameplayScripts.GameSaving
{
    public class MainMenuSaveManager : MonoBehaviour
    {
        [SerializeField] private Button continueButton = null;
        /// <summary>
        /// The current selected save slot
        /// </summary>
        public SaveSlot CurrentSaveSlot { get; set; }
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
            if (!continueButton) return;

            continueButton.interactable = GeneralFunctions.IsAnySaveSlotActive();
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
        /// Will delete the current selected save slot
        /// </summary>
        public void DeleteSaveSlot()
        {
            if (CurrentSaveSlot)
            {
                GeneralFunctions.DeleteGameInSlot(CurrentSaveSlot.SlotNumber);

                CurrentSaveSlot.UpdateSlotText();

                CurrentSaveSlot = null;

                UpdateActiveSlot();

                CheckContinueButton();
            }
        }
        /// <summary>
        /// Gets a new active slot
        /// </summary>
        private static void UpdateActiveSlot()
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