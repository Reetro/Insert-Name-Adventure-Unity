using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LevelObjects.SceneLoading;
using GameplayManagement.Assets;
using System.Collections.Generic;
using Spells;

namespace PlayerUI
{
    public class PlayerUIManager : MonoBehaviour
    {
        [Header("GameOver UI Elements")]
        [SerializeField] Button loadCheckpointBTN = null;
        [SerializeField] TextMeshProUGUI gameOverText = null;

        [Header("Player UI")]
        [SerializeField] HealthBar healthBar = null;

        [SerializeField] private GridLayoutGroup actionBarLayout = null;
        [SerializeField] private ActionButton actionSlot = null;
        [SerializeField] private SpellIcon spellIcon = null;

        private LevelLoader levelLoader = null;

        /// <summary>
        /// List of all action buttons
        /// </summary>
        [HideInInspector]
        public List<ActionButton> actionBarButtons = new List<ActionButton>();

        private void Awake()
        {
            GameAssets.GlobalManager.controllerUpdated.AddListener(OnControllerStateUpdated);

            HideDeathUI();

            levelLoader = FindObjectOfType<LevelLoader>();

            loadCheckpointBTN.onClick.AddListener(loadCheckpoint_onclick);

            CreateActionbar();

            AssignSpells();
        }
        /// <summary>
        /// Load the current checkpoint index
        /// </summary>
        private void loadCheckpoint_onclick()
        {
            levelLoader.LoadCheckpoint();
        }
        /// <summary>
        /// Set the internal health bar value to the player HP bar
        /// </summary>
        public HealthBar HPBar => healthBar;
        /// <summary>
        /// Hide the player death screen UI
        /// </summary>
        public void HideDeathUI()
        {
            loadCheckpointBTN.gameObject.SetActive(false);
            gameOverText.gameObject.SetActive(false);
        }
        /// <summary>
        /// Show the player death UI
        /// </summary>
        public void ShowDeathUI()
        {
            loadCheckpointBTN.gameObject.SetActive(true);
            gameOverText.gameObject.SetActive(true);
        }

        #region Actionbar Functions
        /// <summary>
        /// For every key in the gameplay manager create a action slot
        /// </summary>
        private void CreateActionbar()
        {
            if (!GameAssets.GlobalManager._IsGamepadActive)
            {
                for (int index = 0; index < GameAssets.GlobalManager.spellKeybindsKeyboard.Length; index++)
                {
                    var keyName = GeneralFunctions.GetKeyName(GameAssets.GlobalManager.spellKeybindsKeyboard[index]);

                    var spawnedSlot = Instantiate(actionSlot, actionBarLayout.transform);

                    spawnedSlot.SetupActionSlot(keyName, GameAssets.GlobalManager.spellKeybindsKeyboard[index]);

                    actionBarButtons.Add(spawnedSlot);
                }
            }
            else
            {
                for (int index = 0; index < GameAssets.GlobalManager.spellKeybindsGamepad.Length; index++)
                {
                    var spawnedSlot = Instantiate(actionSlot, actionBarLayout.transform);

                    spawnedSlot.SetupActionSlot("1", GameAssets.GlobalManager.spellKeybindsGamepad[index]);

                    actionBarButtons.Add(spawnedSlot);
                }
            }
        }
        /// <summary>
        /// Assign player spells to Actionbar
        /// </summary>
        private void AssignSpells()
        {
            for (int index = 0; index < GameAssets.GlobalManager.playerSpells.Length; index++)
            {
                if (index >= 0 && index < actionBarButtons.Count)
                {
                    var spawnedSpellIcon = Instantiate(spellIcon, actionBarButtons[index].transform);

                    spawnedSpellIcon.SetupIcon(GameAssets.GlobalManager.playerSpells[index], index);

                    actionBarButtons[index].SetSpellIcon(spawnedSpellIcon);
                }
            }
        }
        /// <summary>
        /// Called when a controller is connected or disconnected
        /// </summary>
        /// <param name="isActive"></param>
        private void OnControllerStateUpdated(bool isActive)
        {
            foreach (ActionButton actionButton in actionBarButtons)
            {
                Destroy(actionButton.gameObject);
            }

            actionBarButtons.Clear();

            CreateActionbar();

            AssignSpells();
        }
        #endregion
    }
}