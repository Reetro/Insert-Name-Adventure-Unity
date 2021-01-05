using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LevelObjects.SceneLoading;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Spells;

namespace PlayerUI
{
    public class PlayerUIManager : MonoBehaviour
    {
        [Header("GameOver UI Elements")]
        [SerializeField] Button loadCheckpointBTN = null;
        [SerializeField] TextMeshProUGUI gameOverText = null;

        [SerializeField] private GridLayoutGroup actionBarLayout = null;
        [SerializeField] private ActionButton actionSlot = null;
        [SerializeField] private SpellIcon spellIcon = null;

        private LevelLoader levelLoader = null;

        #region Properties
        /// <summary>
        /// List of all action buttons
        /// </summary>
        public List<ActionButton> ActionBarButtons { get; private set; } = new List<ActionButton>();
        #endregion

        #region Level Loading Functions
        private void Awake()
        {
            GeneralFunctions.GetGameplayManager().controllerUpdated.AddListener(OnGamepadUpdated);

            HideDeathUI();

            levelLoader = FindObjectOfType<LevelLoader>();

            loadCheckpointBTN.onClick.AddListener(loadCheckpoint_onclick);

            CreateActionbar();
        }
        /// <summary>
        /// When scene is created assign spells to Actionbar
        /// </summary>
        public void OnSceneCreated()
        {
            AssignSpells(GeneralFunctions.GetGameplayManager().playerStartingSpells);
        }
        /// <summary>
        /// Load the current checkpoint index
        /// </summary>
        private void loadCheckpoint_onclick()
        {
            levelLoader = FindObjectOfType<LevelLoader>();

            levelLoader.LoadCheckpoint();

            HideDeathUI();
        }
        #endregion

        #region Death UI Functions
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
        #endregion

        #region Actionbar Functions
        /// <summary>
        /// For every key in the gameplay manager create a action slot
        /// </summary>
        private void CreateActionbar()
        {
            var actionbarInputActions = GeneralFunctions.GetPlayerController().actionBarInputs;

            for (int index = 0; index < actionbarInputActions.Count; index++)
            {
                var keyName = "";

                if (!GeneralFunctions.GetGameplayManager()._IsGamepadActive)
                {
                    var fullName = actionbarInputActions[index].GetBindingDisplayString().Split('|');

                    var finalKeyName = fullName[0].Split();

                    keyName = finalKeyName[1];
                }
                else
                {
                    var fullName = actionbarInputActions[index].GetBindingDisplayString().Split('|');

                    var finalKeyName = fullName[0].Split();

                    keyName = finalKeyName[1];
                }

                var spawnedSlot = Instantiate(actionSlot, actionBarLayout.transform);

                spawnedSlot.SetupActionSlot(keyName, actionbarInputActions[index]);

                ActionBarButtons.Add(spawnedSlot);
            }
        }
        /// <summary>
        /// Assign multiple spells to Actionbar
        /// </summary>
        public void AssignSpells(ScriptableSpell[] spellArray)
        {
            foreach (ScriptableSpell scriptableSpell in spellArray)
            {
                if (scriptableSpell)
                {
                    if (!IsSpellOnActionBar(scriptableSpell))
                    {
                        var actionButton = FindEmptySlotOnBar();

                        if (actionButton)
                        {
                            var spawnedSpellIcon = Instantiate(spellIcon, actionButton.transform);

                            spawnedSpellIcon.SetupIcon(scriptableSpell);

                            actionButton.SetSpellIcon(spawnedSpellIcon);

                            GeneralFunctions.GetPlayerState().AddSpellToList(scriptableSpell);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Assign multiple spells to Actionbar and check to see if all spells where assigned
        /// </summary>
        /// <param name="spellArray"></param>
        /// <param name="allSpellsAssigned"></param>
        public void AssignSpells(ScriptableSpell[] spellArray, out bool allSpellsAssigned)
        {
            var successCount = 0;

            foreach (ScriptableSpell scriptableSpell in spellArray)
            {
                if (scriptableSpell)
                {
                    if (!IsSpellOnActionBar(scriptableSpell))
                    {
                        var actionButton = FindEmptySlotOnBar();

                        if (actionButton)
                        {
                            var spawnedSpellIcon = Instantiate(spellIcon, actionButton.transform);

                            spawnedSpellIcon.SetupIcon(scriptableSpell);

                            actionButton.SetSpellIcon(spawnedSpellIcon);

                            GeneralFunctions.GetPlayerState().AddSpellToList(scriptableSpell);

                            successCount++;
                        }
                    }
                }
            }

            if (successCount >= spellArray.Length)
            {
                allSpellsAssigned = true;
            }
            else
            {
                allSpellsAssigned = false;
            }
        }
        /// <summary>
        /// Assign a single spell to Actionbar
        /// </summary>
        /// <param name="scriptableSpell"></param>
        public void AssignSpell(ScriptableSpell scriptableSpell)
        {
            if (scriptableSpell)
            {
                if (!IsSpellOnActionBar(scriptableSpell))
                {
                    var actionButton = FindEmptySlotOnBar();

                    if (actionButton)
                    {
                        var spawnedSpellIcon = Instantiate(spellIcon, actionButton.transform);

                        spawnedSpellIcon.SetupIcon(scriptableSpell);

                        actionButton.SetSpellIcon(spawnedSpellIcon);

                        GeneralFunctions.GetPlayerState().AddSpellToList(scriptableSpell);
                    }
                }
            }
            else
            {
                Debug.LogError("Failed to assign spell spell was invalid");
            }
        }
        /// <summary>
        /// Assign a single spell to Actionbar and check to see if it did get assigned
        /// </summary>
        /// <param name="scriptableSpell"></param>
        /// <param name="spellsAssigned"></param>
        public void AssignSpell(ScriptableSpell scriptableSpell, out bool spellsAssigned)
        {
            if (scriptableSpell)
            {
                if (!IsSpellOnActionBar(scriptableSpell))
                {
                    var actionButton = FindEmptySlotOnBar();

                    if (actionButton)
                    {
                        var spawnedSpellIcon = Instantiate(spellIcon, actionButton.transform);

                        spawnedSpellIcon.SetupIcon(scriptableSpell);

                        actionButton.SetSpellIcon(spawnedSpellIcon);

                        GeneralFunctions.GetPlayerState().AddSpellToList(scriptableSpell);

                        spellsAssigned = true;
                    }
                    else
                    {
                        spellsAssigned = false;
                    }
                }
                else
                {
                    spellsAssigned = false;
                }
            }
            else
            {
                spellsAssigned = false;

                Debug.LogError("Failed to assign spell spell was invalid");
            }
        }
        /// <summary>
        /// Tries to find a empty slot on the Actionbar
        /// </summary>
        /// <returns>The found Actionbar slot if bar is full will return null</returns>
        private ActionButton FindEmptySlotOnBar()
        {
            foreach (ActionButton actionButton in ActionBarButtons)
            {
                if (actionButton)
                {
                    if (!actionButton.HasSpellInSlot)
                    {
                        return actionButton;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// Checks to see if the spell is already on the Actionbar
        /// </summary>
        /// <param name="spell"></param>
        private bool IsSpellOnActionBar(ScriptableSpell spell)
        {
            foreach (ScriptableSpell scriptableSpell in GeneralFunctions.GetPlayerState().PlayerSpells)
            {
                if (scriptableSpell)
                {
                    if (scriptableSpell.GetType() == spell.GetType())
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        /// <summary>
        /// Called whenever a gamepad is connected or disconnected
        /// </summary>
        /// <param name="connected"></param>
        private void OnGamepadUpdated(bool connected)
        {
            foreach (ActionButton actionButton in ActionBarButtons)
            {
                Destroy(actionButton.gameObject);
            }

            ActionBarButtons.Clear();

            CreateActionbar();

            AssignSpells(GeneralFunctions.GetPlayerState().PlayerSpells.ToArray());
        }
        #endregion
    }
}