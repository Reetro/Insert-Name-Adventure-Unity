﻿using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using Spells;
using UnityEngine.Events;

namespace GameplayManagement
{
    [Serializable]
    public class GameplayManager : MonoBehaviour
    {
        [Header("Combat Text Settings")]
        [Tooltip("How fast spawned combat text moves")]
        [Range(0.01f, 1f)] public float combatTextSpeed = 0.01f;
        [Tooltip("How long combat text exists in the world")]
        public float combatTextUpTime = 0.5f;
        [Tooltip("Minimum amount for spawned text X coordinate")]
        public float combatRandomVectorMinX = -0.5f;
        [Tooltip("Maximum amount for spawned text X coordinate")]
        public float combatRandomVectorMaxX = 1f;
        [Tooltip("Minimum amount for spawned text Y coordinate")]
        public float combatRandomVectorMinY = -0.5f;
        [Tooltip("Maximum amount for spawned text Y coordinate")]
        public float combatRandomVectorMaxY = 1f;
        [Tooltip("How long it takes for combat text to disappear")]
        public float disappearTime = 3f;

        [Header("Tooltip Settings")]
        [Tooltip("Font size for ToolTip item names")]
        public float nameFontSize = 36f;
        [Tooltip("Font size for ToolTip item descriptions")]
        public float descriptionFontSize = 28f;

        [Space]

        [Header("Damage Settings")]
        [Tooltip("Layers that can receive damage")]
        public LayerMask whatCanBeDamaged;

        [Header("Player Spell Settings")]
        [Tooltip("Spells the player starts with")]
        public ScriptableSpell[] playerSpells;

        [Space]

        [Tooltip("Keybinds for Player Actionbar for every Input Action in this array a new action slot is created")]
        public InputActionMap actionBarInputActions;

        [Space]

        [Header("Game Debug")]
        [Tooltip("Whether or not to print save game debug messages")]
        public bool debugSave = false;

        [System.Serializable]
        public class OnControllerUpdate : UnityEvent<bool> { }

        [HideInInspector]
        public OnControllerUpdate controllerUpdated;

        [HideInInspector]
        public UnityEvent onLevelExitOverlap;

        [HideInInspector]
        public UnityEvent onSceneLoadingDone;

        /// <summary>
        /// Checks to see if a Gamepad is connected
        /// </summary>
        public bool _IsGamepadActive { get; private set; } = false;
        /// <summary>
        /// Check to see if a Gamepad is active when game starts
        /// </summary>
        private void Awake()
        {
            UpdateGamepadState();
            UpdateMouseCursor();
        }

        #region ID Management
        /// <summary>
        /// Generates a random number between 1 and 1000000 then adds to the gameIDS array
        /// </summary>
        /// <returns>A random int</returns>
        public int GenID()
        {
            var newID = UnityEngine.Random.Range(1, 1000000);

            foreach (int currentID in gameIDS)
            {
                if (gameIDS.Contains(newID))
                {
                    newID = UnityEngine.Random.Range(1, 1000000);
                    break;
                }
            }

            gameIDS.Add(newID);
            return newID;
        }
        /// <summary>
        /// Get all current gameIDS
        /// </summary>
        public List<int> gameIDS { get; } = new List<int>();
        #endregion

        #region Control Functions
        /// <summary>
        /// Bind onInputDeviceChange to input user onChange event
        /// </summary>
        private void OnEnable()  { InputUser.onChange += IsGamepadActive; }
        /// <summary>
        /// Unbind onInputDeviceChange to input user onChange event
        /// </summary>
        private void OnDisable() { InputUser.onChange -= IsGamepadActive; }
        /// <summary>
        /// Checks to see if a Gamepad is connected and updates cursor state
        /// </summary>
        private void IsGamepadActive(InputUser user, InputUserChange change, InputDevice device)
        {
            UpdateGamepadState();
            UpdateMouseCursor();
        }
        /// <summary>
        /// Check to see if a Gamepad is currently connected
        /// </summary>
        private void UpdateGamepadState()
        {
            _IsGamepadActive = (Gamepad.all.Count >= 1) ? true : false;

            controllerUpdated.Invoke(_IsGamepadActive);
        }
        /// <summary>
        /// Show / Hide mouse cursor depending on if a Gamepad is active
        /// </summary>
        private void UpdateMouseCursor()
        {
            if (_IsGamepadActive)
            {
                Cursor.lockState = CursorLockMode.Locked;

                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;

                Cursor.visible = true;
            }
        }
        #endregion
    }
}