using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LevelObjects.SceneLoading;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Spells;
using UnityEngine.EventSystems;

namespace PlayerUI
{
    public class PlayerUIManager : MonoBehaviour
    {
        [Header("GameOver UI Elements")]
        [SerializeField] private Button loadCheckpointBTN = null;
        [SerializeField] private TextMeshProUGUI gameOverText = null;

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

            // Clear selected Object
            EventSystem.current.SetSelectedGameObject(null);
        }
        /// <summary>
        /// Show the player death UI
        /// </summary>
        public void ShowDeathUI()
        {
            loadCheckpointBTN.gameObject.SetActive(true);
            gameOverText.gameObject.SetActive(true);

            if (GeneralFunctions.GetGameplayManager()._IsGamepadActive)
            {
                // Clear selected Object
                EventSystem.current.SetSelectedGameObject(null);

                // Set new selected object
                EventSystem.current.SetSelectedGameObject(loadCheckpointBTN.gameObject);
            }
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

                    var finalKeyName = fullName[1].Split();

                    keyName = finalKeyName[2];
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
                    if (!FindSpellOnActionbar(scriptableSpell))
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
                    if (!FindSpellOnActionbar(scriptableSpell))
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
                if (!FindSpellOnActionbar(scriptableSpell))
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
                if (!FindSpellOnActionbar(scriptableSpell))
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
        /// Finds the given ScriptableSpell on the Actionbar then removes it
        /// </summary>
        /// <param name="scriptableSpell"></param>
        public void RemoveSpellFromSlot(ScriptableSpell scriptableSpell)
        {
            if (scriptableSpell)
            {
                var foundSpell = FindSpellIconOnActionbar(scriptableSpell);
                
                if (foundSpell)
                {
                    GeneralFunctions.GetPlayerState().RemoveSpellFromList(scriptableSpell);

                    foundSpell.ParentButton.RemoveSpellIcon();
                }
                else
                {
                    Debug.LogError("Failed to RemoveSpellFromSlot failed to find spell " + scriptableSpell.name + " on Actionbar");
                }
            }
            else
            {
                Debug.LogError("Failed to RemoveSpellFromSlot scriptableSpell was not valid");
            }
        }
        /// <summary>
        /// Finds the given ScriptableSpell on the Actionbar then removes it and check to see if the spell was removed
        /// </summary>
        /// <param name="scriptableSpell"></param>
        /// <param name="wasSpellRemoved"></param>
        public void RemoveSpellFromSlot(ScriptableSpell scriptableSpell, out bool wasSpellRemoved)
        {
            if (scriptableSpell)
            {
                var foundSpellIcon = FindSpellIconOnActionbar(scriptableSpell);

                if (foundSpellIcon)
                {
                    GeneralFunctions.GetPlayerState().RemoveSpellFromList(scriptableSpell);

                    foundSpellIcon.ParentButton.RemoveSpellIcon();

                    wasSpellRemoved = true;
                }
                else
                {
                    Debug.LogError("Failed to RemoveSpellFromSlot scriptableSpell failed to find spell on Actionbar");

                    wasSpellRemoved = false;
                }
            }
            else
            {
                Debug.LogError("Failed to RemoveSpellFromSlot scriptableSpell was not valid");

                wasSpellRemoved = false;
            }
        }
        /// <summary>
        /// Finds the given ScriptableSpells on the Actionbar then remove them
        /// </summary>
        /// <param name="scriptableSpells"></param>
        public void RemoveSpellsFromSlots(ScriptableSpell[] scriptableSpells)
        {
            foreach (ScriptableSpell scriptableSpell in scriptableSpells)
            {
                if (scriptableSpell)
                {
                    var foundSpellIcon = FindSpellIconOnActionbar(scriptableSpell);

                    if (foundSpellIcon)
                    {
                        GeneralFunctions.GetPlayerState().RemoveSpellFromList(scriptableSpell);

                        foundSpellIcon.ParentButton.RemoveSpellIcon();
                    }
                    else
                    {
                        Debug.LogError("Failed to RemoveSpellFromSlot failed to find spell " + scriptableSpell.name + " on Actionbar");
                    }
                }
            }
        }
        /// <summary>
        /// Finds the given ScriptableSpells on the Actionbar then remove them and check to see if they where all removed
        /// </summary>
        /// <param name="scriptableSpells"></param>
        /// <param name="whereAllSpellsRemoved"></param>
        public void RemoveSpellsFromSlots(ScriptableSpell[] scriptableSpells, out bool whereAllSpellsRemoved)
        {
            var successCount = 0;

            foreach (ScriptableSpell scriptableSpell in scriptableSpells)
            {
                if (scriptableSpell)
                {
                    var foundSpellIcon = FindSpellIconOnActionbar(scriptableSpell);

                    if (foundSpellIcon)
                    {
                        GeneralFunctions.GetPlayerState().RemoveSpellFromList(scriptableSpell);

                        foundSpellIcon.ParentButton.RemoveSpellIcon();

                        successCount++;
                    }
                    else
                    {
                        Debug.LogError("Failed to RemoveSpellFromSlot failed to find spell " + scriptableSpell.name + " on Actionbar");
                    }
                }
            }

            if (successCount >= scriptableSpells.Length)
            {
                whereAllSpellsRemoved = true;
            }
            else
            {
                whereAllSpellsRemoved = false;
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
        /// Find the given spell on the Actionbar
        /// </summary>
        /// <param name="spellToFind"></param>  
        /// <returns>The found ScriptableSpell</returns>
        public ScriptableSpell FindSpellOnActionbar(ScriptableSpell spellToFind)
        {
            foreach(ActionButton actionButton in ActionBarButtons)
            {
                if (actionButton)
                {
                    if (actionButton.transform.childCount > 1)
                    {
                        var spell = actionButton.transform.GetChild(1).GetComponent<SpellIcon>();

                        if (spell)
                        {
                            if (spell.MyScriptableSpell.GetType() == spellToFind.GetType())
                            {
                                return spell.MyScriptableSpell;
                            }
                        }
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// Finds the Spell Icon that has the given spell
        /// </summary>
        /// <param name="scriptableSpell"></param>
        public SpellIcon FindSpellIconOnActionbar(ScriptableSpell scriptableSpell)
        {
            foreach(ActionButton actionButton in ActionBarButtons)
            {
                if (actionButton)
                {
                    if (actionButton.transform.childCount > 1)
                    {
                        var spell = actionButton.transform.GetChild(1).GetComponent<SpellIcon>();

                        if (spell)
                        {
                            if (spell.MyScriptableSpell.GetType() == scriptableSpell.GetType())
                            {
                                return spell;
                            }
                        }
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// Called whenever a gamepad is connected or disconnected
        /// </summary>
        /// <param name="connected"></param>
        private void OnGamepadUpdated(bool connected)
        {
            var tempSpellList = GeneralFunctions.GetPlayerState().PlayerSpells.ToArray();

            foreach (ActionButton actionButton in ActionBarButtons)
            {
                Destroy(actionButton.gameObject);
            }

            ActionBarButtons.Clear();

            GeneralFunctions.GetPlayerState().ClearSpellList();

            CreateActionbar();

            AssignSpells(tempSpellList);
        }
        #endregion
    }
}