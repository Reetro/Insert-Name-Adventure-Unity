﻿using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LevelObjects.SceneLoading;
using GameplayManagement.Assets;
using System.Collections.Generic;
using UnityEngine.InputSystem;

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

        /// <summary>
        /// List of all action buttons
        /// </summary>
        [HideInInspector]
        public List<ActionButton> actionBarButtons = new List<ActionButton>();

        private void Awake()
        {
            GameAssets.GlobalManager.controllerUpdated.AddListener(OnGamepadUpdated);

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
            levelLoader = FindObjectOfType<LevelLoader>();

            levelLoader.LoadCheckpoint();

            HideDeathUI();
        }
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
            var actionbarInputActions = GeneralFunctions.GetPlayerController().actionBarInputs;

            for (int index = 0; index < actionbarInputActions.Count; index++)
            {
                var keyName = "";

                if (!GameAssets.GlobalManager._IsGamepadActive)
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

                actionBarButtons.Add(spawnedSlot);
            }
        }
        /// <summary>
        /// Assign player spells to Actionbar
        /// </summary>
        private void AssignSpells()
        {
            for (int index = 0; index < GameAssets.GlobalManager.playerStartingSpells.Length; index++)
            {
                if (index >= 0 && index < actionBarButtons.Count)
                {
                    var spawnedSpellIcon = Instantiate(spellIcon, actionBarButtons[index].transform);

                    spawnedSpellIcon.SetupIcon(GameAssets.GlobalManager.playerStartingSpells[index], index);

                    actionBarButtons[index].SetSpellIcon(spawnedSpellIcon);
                }
            }
        }
        /// <summary>
        /// Called whenever a gamepad is connected or disconnected
        /// </summary>
        /// <param name="connected"></param>
        private void OnGamepadUpdated(bool connected)
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