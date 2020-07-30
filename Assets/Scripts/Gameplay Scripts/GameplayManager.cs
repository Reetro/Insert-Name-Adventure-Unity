using UnityEngine;
using System.Collections.Generic;
using System;
using PlayerControls;
using UnityEngine.InputSystem;

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

        private Controls controls = null;

        /// <summary>
        /// Create new player controls object
        /// </summary>
        private void Awake()
        {
            foreach (InputDevice inputDevice in InputSystem.devices)
            {
                print(inputDevice.name);
            }
        }
        /// <summary>
        /// Generates a random number between 1 and 1000000 then adds to the gameIDS array
        /// </summary>
        /// <returns>A random int</returns>
        public int GenID()
        {
            var newID = UnityEngine.Random.Range(1, 1000000);

            for (int index = 0; index < gameIDS.Count; index++)
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
    }
}