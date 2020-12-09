using UnityEngine;
using UnityEngine.SceneManagement;
using PlayerCharacter.Controller;
using PlayerUI;
using GameplayManagement.Assets;

namespace PlayerCharacter.GameSaving
{
    public class PlayerState : MonoBehaviour
    {
        public PlayerController player = null;
        public HealthBar playerHPBar = null;

        #region Local Variables
        private static float currentHealth = 0f;
        private static float maxHealth = 0f;
        private static int checkpointLevelIndex = 0;
        private static bool isSceneLoading = false;
        private static int currentSlot = 0;

        /// <summary>
        /// Struct that contains base info for every save slot
        /// </summary>
        public struct SavedGameSlot
        {
            /// <summary>
            /// Slot the game is saved in
            /// </summary>
            public int slot;
            /// <summary>
            /// Whether or not this is the current active save slot
            /// </summary>
            public bool isActive;

            /// <summary>
            /// Creates a new save slot
            /// </summary>
            /// <param name="slotToSet"></param>
            /// <param name="isActiveToSet"></param>
            public SavedGameSlot(int slotToSet, bool isActiveToSet)
            {
                slot = slotToSet;
                isActive = isActiveToSet;
            }
        }
        #endregion

        #region Properites
        /// <summary>
        /// Get the players current health
        /// </summary>
        public float CurrentHealth { get { return currentHealth; } }
        /// <summary>
        /// Get the players max health
        /// </summary>
        public float MaxHealth { get { return maxHealth; } }
        /// <summary>
        /// Get the players current checkpoint level
        /// </summary>
        public int CheckpointLevelIndex { get { return checkpointLevelIndex; } }
        /// <summary>
        /// Get the index of the current level the player is in
        /// </summary>
        public int CurrentLevelIndex { get; private set; }
        /// <summary>
        /// Checks to see if a scene is currently loading
        /// </summary>
        public bool IsLoadingScene { get { return isSceneLoading; } }
        /// <summary>
        /// All current saved game slots
        /// </summary>
        public SavedGameSlot[] SavedGameSlots { get; private set; }
        #endregion

        #region Setup Functions
        private void Awake()
        {
            int playerStateCount = FindObjectsOfType<PlayerState>().Length;

            if (playerStateCount > 1)
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
            }

            SavedGameSlots = new SavedGameSlot[3]
            {
                new SavedGameSlot(1, false),
                new SavedGameSlot(2, false),
                new SavedGameSlot(3, false),
            };
        }
        public void SetCheckpointIndex(int index)
        {
            checkpointLevelIndex = index;
        }
        public void SetSceneLoading(bool value)
        {
            isSceneLoading = value;
        }
        #endregion
        
        #region Health Functions
        public void SetPlayerHealth()
        {
            player = FindObjectOfType<PlayerController>();

            player.GetComponent<HealthComponent>().SetHealth(currentHealth, maxHealth);

            playerHPBar.SetHealth(currentHealth);
        }

        public void ResetHealthToMax()
        {
            currentHealth = maxHealth;
        }

        public void UpdatePlayerStateHP(float currentHP, float maxHP)
        {
            currentHealth = currentHP;
            maxHealth = maxHP;
        }
        #endregion

        #region Level Loading Events
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        }

        void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            isSceneLoading = false;

            CurrentLevelIndex = SceneManager.GetActiveScene().buildIndex;

            SetPlayerHealth();
        }
        #endregion

        #region Game Saving / Loading Functions
        /// <summary>
        /// Creates a new save and loads 1st level
        /// </summary>
        public void StartNewGame(int slot)
        {
            currentSlot = slot;

            GeneralFunctions.GetLevelLoader().LoadLevelAtIndex(0);

            SceneManager.sceneLoaded += LoadStartingLevel;
        }
        /// <summary>
        /// After the starting scene is loaded create a new save file
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="mode"></param>
        private void LoadStartingLevel(Scene scene, LoadSceneMode mode)
        {
            if (IsAnySlotActive())
            {
                DeactivateAllSlots();
            }

            SaveGameToSlot(currentSlot);

            SceneManager.sceneLoaded -= LoadStartingLevel;
        }
        /// <summary>
        /// Saves all player game data
        /// </summary>
        public void SaveGameToSlot(int slot)
        {
            if (!SaveSystem.DoesSaveSlotExist(slot))
            {
                SetActiveSlot(slot);

                SaveSystem.SaveGame(this, player.gameObject, GetSavedGameSlotInfo(slot));
            }
            else
            {
                DeleteSaveGame(slot);

                SetActiveSlot(slot);

                SaveSystem.SaveGame(this, player.gameObject, GetSavedGameSlotInfo(slot));
            }

            if (GameAssets.GlobalManager.debugSave)
            {
                Debug.Log("Saved game in slot " + slot);
            }
        }
        /// <summary>
        /// Finds the current active save file and loads it
        /// </summary>
        public void LoadActiveSave()
        {
            var slot = GetActiveSlot();

            LoadGame(slot);
        }
        /// <summary>
        /// Gets saved data then sets all local Variables and load the saved level
        /// </summary>
        public void LoadGame(int slot)
        {
            if (slot >= 0)
            {
                currentSlot = slot;

                var loadedData = SaveSystem.LoadPlayerFromSlot(slot);

                checkpointLevelIndex = loadedData.CheckpointLevelIndex;
                CurrentLevelIndex = loadedData.CurrentLevelIndex;

                currentHealth = loadedData.CurrentHealth;
                maxHealth = loadedData.MaxHealth;

                SceneManager.sceneLoaded += LoadPlayerPostion;

                GeneralFunctions.GetLevelLoader().LoadLevelAtIndex(CurrentLevelIndex);
            }
            else
            {
                Debug.LogError("Failed to load saved game slot was invalid");
            }
        }
        /// <summary>
        /// Set the player position to the saved player position when scene is done loading
        /// </summary>
        private void LoadPlayerPostion(Scene scene, LoadSceneMode mode)
        {
            var loadedData = SaveSystem.LoadPlayerFromSlot(currentSlot);

            Vector3 position;
            position.x = loadedData.PlayerPosition[0];
            position.y = loadedData.PlayerPosition[1];
            position.z = loadedData.PlayerPosition[2];

            if (IsAnySlotActive())
            {
                DeactivateAllSlots();
            }

            SetActiveSlot(currentSlot);

            player.transform.position = position;

            if (GameAssets.GlobalManager.debugSave)
            {
                Debug.Log("Loaded game in slot " + currentSlot);
            }

            SceneManager.sceneLoaded -= LoadPlayerPostion;
        }
        /// <summary>
        /// Delete the current saved game
        /// </summary>
        public void DeleteSaveGame(int slot)
        {
            DeactivateSlot(slot);

            SaveSystem.DeleteSaveGame(slot);

            if (GameAssets.GlobalManager.debugSave)
            {
                Debug.Log("Deleted saved game in slot " + slot);
            }
        }
        /// <summary>
        /// Checks to see if the given is active
        /// </summary>
        /// <param name="slot"></param>
        public bool IsSlotActive(int slot)
        {
            for (int index = 0; index < SavedGameSlots.Length; index++)
            {
                if (SavedGameSlots[index].slot.Equals(slot))
                {
                    if (SaveSystem.DoesPlayerSaveGameExistInSlot(SavedGameSlots[index].slot))
                    {
                        var slotData = SaveSystem.LoadSaveSlot(SavedGameSlots[index].slot);

                        SavedGameSlots[index].slot = slotData.Slot;
                        SavedGameSlots[index].isActive = slotData.IsSlotActive;

                        return SavedGameSlots[index].isActive;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Checks see if any save slot is active
        /// </summary>
        public bool IsAnySlotActive()
        {
            for (int index = 0; index < SavedGameSlots.Length; index++)
            {
                if (SaveSystem.DoesPlayerSaveGameExistInSlot(SavedGameSlots[index].slot))
                {
                    var slotData = SaveSystem.LoadSaveSlot(SavedGameSlots[index].slot);

                    SavedGameSlots[index].slot = slotData.Slot;
                    SavedGameSlots[index].isActive = slotData.IsSlotActive;

                    if (SavedGameSlots[index].isActive)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Gets a given slots data
        /// </summary>
        /// <param name="slot"></param>
        private SavedGameSlot GetSavedGameSlotInfo(int slot)
        {
            SavedGameSlot savedGameSlot = new SavedGameSlot();

            foreach (SavedGameSlot currentSavedGameSlot in SavedGameSlots)
            {
                if (currentSavedGameSlot.slot.Equals(slot))
                {
                    savedGameSlot = currentSavedGameSlot;
                    break;
                }
            }

            return savedGameSlot;
        }
        /// <summary>
        /// Gets the of the current active slot if no slots are active defaults to 1st save slot
        /// </summary>
        /// <returns>The name of the slot as a string</returns>
        private int GetActiveSlot()
        {
            int slot = -1;

            foreach (SavedGameSlot savedGameSlot in SavedGameSlots)
            {
                if (savedGameSlot.isActive)
                {
                    slot = savedGameSlot.slot;
                    break;
                }
                else
                {
                    slot = -1;
                }
            }

            return slot;
        }
        /// <summary>
        /// Sets the given slot to be the only active slot
        /// </summary>
        /// <param name="slot"></param>
        public void SetActiveSlot(int slot)
        {
            for (int index = 0; index < SavedGameSlots.Length; index++)
            {
                if (SavedGameSlots[index].slot.Equals(slot))
                {
                    SavedGameSlots[index].isActive = true;

                    SaveSystem.SaveGameSlot(ref SavedGameSlots[index]);
                }
                else
                {
                    SavedGameSlots[index].isActive = false;

                    SaveSystem.SaveGameSlot(ref SavedGameSlots[index]);
                }
            }
        }
        /// <summary>
        /// Deactivate the given slot
        /// </summary>
        /// <param name="slot"></param>
        private void DeactivateSlot(int slot)
        {
            for (int index = 0; index < SavedGameSlots.Length; index++)
            {
                if (SavedGameSlots[index].slot.Equals(slot))
                {
                    SavedGameSlots[index].isActive = false;
                }
            }
        }
        /// <summary>
        /// Deactivate all slots
        /// </summary>
        private void DeactivateAllSlots()
        {
            for (int index = 0; index < SavedGameSlots.Length; index++)
            {
                if (DoesSaveExistInSlot(SavedGameSlots[index].slot))
                {
                    var slotData = SaveSystem.LoadSaveSlot(SavedGameSlots[index].slot);

                    SavedGameSlots[index].isActive = slotData.IsSlotActive;
                    SavedGameSlots[index].slot = slotData.Slot;

                    if (SavedGameSlots[index].isActive)
                    {
                        SavedGameSlots[index].isActive = false;

                        SaveSystem.SaveGameSlot(ref SavedGameSlots[index]);
                    }
                }
            }
        }
        /// <summary>
        /// Checks to see if there is a save in the given slot
        /// </summary>
        /// <param name="slot"></param>
        public bool DoesSaveExistInSlot(int slot)
        {
            return SaveSystem.DoesSaveGameExistInSlot(slot);
        }
        #endregion
    }
}