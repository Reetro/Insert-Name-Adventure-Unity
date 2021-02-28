using System.Collections.Generic;
using GameplayScripts.GameSaving;
using GeneralScripts;
using GeneralScripts.GeneralComponents;
using GeneralScripts.UI;
using Spells;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace PlayerScripts.PlayerControls
{
    public class PlayerState : MonoBehaviour
    {
        public PlayerController player;
        [FormerlySerializedAs("playerHPBar")] public HealthBar playerHpBar;

        #region Local Variables

        private static int currentSlot;

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
        public static float CurrentHealth { get; private set; }
        /// <summary>
        /// Get the players max health
        /// </summary>
        public static float MaxHealth { get; private set; }
        /// <summary>
        /// Get the players current checkpoint level
        /// </summary>
        public static int CheckpointLevelIndex { get; private set; }
        /// <summary>
        /// Get the index of the current level the player is in
        /// </summary>
        public int CurrentLevelIndex { get; private set; }
        /// <summary>
        /// Checks to see if a scene is currently loading
        /// </summary>
        public static bool IsLoadingScene { get; private set; }
        /// <summary>
        /// All current saved game slots
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        public SavedGameSlot[] SavedGameSlots { get; private set; }
        /// <summary>
        /// List of all Spells the player has on the Actionbar
        /// </summary>
        public static List<ScriptableSpell> PlayerSpells { get; private set; } = new List<ScriptableSpell>();
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

            SavedGameSlots = new[]
            {
                new SavedGameSlot(1, false),
                new SavedGameSlot(2, false),
                new SavedGameSlot(3, false),
            };

            if (PlayerSpells.Count <= 0)
            {
                PlayerSpells = new List<ScriptableSpell>();
            }
        }
        public void SetCheckpointIndex(int index)
        {
            CheckpointLevelIndex = index;
        }
        public void SetSceneLoading(bool value)
        {
            IsLoadingScene = value;
        }
        #endregion
        
        #region Health Functions

        private void SetPlayerHealth()
        {
            player = FindObjectOfType<PlayerController>();

            player.GetComponent<HealthComponent>().SetHealth(CurrentHealth, MaxHealth);

            playerHpBar.SetHealth(CurrentHealth);
        }

        public void ResetHealthToMax()
        {
            CurrentHealth = MaxHealth;
        }

        public static void UpdatePlayerStateHp(float currentHp, float maxHp)
        {
            CurrentHealth = currentHp;
            MaxHealth = maxHp;
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
            IsLoadingScene = false;

            CurrentLevelIndex = SceneManager.GetActiveScene().buildIndex;

            playerHpBar = FindObjectOfType<PlayerUIManager>().transform.GetChild(1).GetComponent<HealthBar>();

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

            if (GeneralFunctions.GetGameplayManager().debugSave)
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

                CheckpointLevelIndex = loadedData.CheckpointLevelIndex;
                CurrentLevelIndex = loadedData.CurrentLevelIndex;

                CurrentHealth = loadedData.CurrentHealth;
                MaxHealth = loadedData.MaxHealth;

                SceneManager.sceneLoaded += LoadPlayerPosition;

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
        private void LoadPlayerPosition(Scene scene, LoadSceneMode mode)
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

            if (GeneralFunctions.GetGameplayManager().debugSave)
            {
                Debug.Log("Loaded game in slot " + currentSlot);
            }

            SceneManager.sceneLoaded -= LoadPlayerPosition;
        }
        /// <summary>
        /// Delete the current saved game
        /// </summary>
        public void DeleteSaveGame(int slot)
        {
            DeactivateSlot(slot);

            SaveSystem.DeleteSaveGame(slot);

            if (GeneralFunctions.GetGameplayManager().debugSave)
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

        #region Spell Functions
        /// <summary>
        /// Adds the given spell to the PlayerSpells List
        /// </summary>
        /// <param name="scriptableSpell"></param>
        public void AddSpellToList(ScriptableSpell scriptableSpell)
        {
            PlayerSpells.Add(scriptableSpell);
        }
        /// <summary>
        /// Remove the given spell from the PlayerSpells List
        /// </summary>
        /// <param name="scriptableSpell"></param>
        public void RemoveSpellFromList(ScriptableSpell scriptableSpell)
        {
            PlayerSpells.Remove(scriptableSpell);
        }
        /// <summary>
        /// Remove all spells in PlayerSpells List
        /// </summary>
        public void ClearSpellList()
        {
            PlayerSpells.Clear();
        }
        #endregion
    }
}