using System;
using System.Collections.Generic;
using System.Linq;
using Spells;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace GameplayScripts
{
    [Serializable]
    public class GameplayManager : MonoBehaviour
    {
        [Header("Combat Text Settings")]
        [Tooltip("How fast spawned combat text moves")]
        [Range(0.01f, 1f)] public float combatTextSpeed = 0.01f;
        [Tooltip("How long combat text exists in the world")]
        public float combatTextUpTime = 0.5f;
        [Tooltip("How long it takes for combat text to disappear")]
        public float disappearTime = 3f;
        [Tooltip("How far damage text travels")]
        public float textDistance = 1f;

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
        public ScriptableSpell[] playerStartingSpells;

        [Space]

        [Header("Game Debug")]
        [Tooltip("Whether or not to print save game debug messages")]
        public bool debugSave;

        [Serializable]
        public class OnControllerUpdate : UnityEvent<bool> { }

        [Serializable]
        public class OnCameraBoxOverlap : UnityEvent<string> { }


        [HideInInspector]
        public OnControllerUpdate controllerUpdated;

        [HideInInspector]
        public OnCameraBoxOverlap cameraBoxOverlap;

        [HideInInspector]
        public UnityEvent onCameraTopOverlap;

        [HideInInspector]
        public UnityEvent onCameraLeftOverlap;

        [HideInInspector]
        public UnityEvent onCameraRightOverlap;

        [HideInInspector]
        public UnityEvent onCameraBottomOverlap;

        [HideInInspector]
        public UnityEvent onLevelExitOverlap;

        [HideInInspector]
        public UnityEvent onSceneLoadingDone;

        [HideInInspector]
        public UnityEvent onGamePause;

        [HideInInspector]
        public UnityEvent onGameResume;

        /// <summary>
        /// Checks to see if a Gamepad is connected
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public bool _IsGamepadActive { get; private set; }
        /// <summary>
        /// Check to see if the Game is paused
        /// </summary>
        public bool IsGamePaused { get; set; }
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

            if (GameIds.Any(currentID => GameIds.Contains(newID)))
            {
                newID = UnityEngine.Random.Range(1, 1000000);
            }

            GameIds.Add(newID);
            return newID;
        }
        /// <summary>
        /// Get all current gameIDS
        /// </summary>
        public List<int> GameIds { get; } = new List<int>();
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
            _IsGamepadActive = (Gamepad.all.Count >= 1);

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