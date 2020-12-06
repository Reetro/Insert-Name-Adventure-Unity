﻿using UnityEngine;
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
        /// <summary>
        /// For every key in the gameplay manager create a action slot
        /// </summary>
        private void CreateActionbar()
        {
            for (int index = 0; index < GameAssets.GlobalManager.spellKeybinds.Length; index++)
            {
                var keyName = GeneralFunctions.GetKeyName(GameAssets.GlobalManager.spellKeybinds[index]);

                var spawnedSlot = Instantiate(actionSlot, actionBarLayout.transform);

                spawnedSlot.SetupActionSlot(keyName, GameAssets.GlobalManager.spellKeybinds[index]);

                actionBarButtons.Add(spawnedSlot);
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
    }
}