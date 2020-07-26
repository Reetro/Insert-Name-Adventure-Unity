using UnityEngine;
using System.Collections.Generic;
using System;
using UnityStandardAssets.CrossPlatformInput;
using PlayerCharacter.Controller;

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

        [Header("Gamepad Settings")]
        [Tooltip("How often to check for controller input")]
        public float defaultControllerCheckTimer = 2;

        [HideInInspector]
        // Current controllers that are connected to the computer
        public string[] currentControllers;

        private bool playstationController, xboxController, keyboard;
        private float controllerCheckTimer = 2;
        private PlayerGun playerGun = null;

        [Space]

        [Header("Damage Settings")]
        [Tooltip("Layers that can receive damage")]
        public LayerMask whatCanBeDamaged;

        private void Start()
        {
            playerGun = GeneralFunctions.GetPlayerGameObject().GetComponentInChildren<PlayerGun>();

            ControllerCheck();
        }

        private void Update()
        {
            bool currentlyMoving = CrossPlatformInputManager.GetAxis("Horizontal") < 0;

            if (!CrossPlatformInputManager.GetButtonDown("Fire1") && !CrossPlatformInputManager.GetButton("Jump") && !currentlyMoving)
            {
                controllerCheckTimer -= Time.deltaTime;
                if (controllerCheckTimer <= 0)
                {
                    ControllerCheck();
                    controllerCheckTimer = defaultControllerCheckTimer;
                }
            }
            else
            {
                controllerCheckTimer = defaultControllerCheckTimer;
            }
        }

        private void ControllerCheck()
        {
            System.Array.Clear(currentControllers, 0, currentControllers.Length);   
            System.Array.Resize<string>(ref currentControllers, Input.GetJoystickNames().Length);
            int numberOfControllers = 0;
            for (int i = 0; i < Input.GetJoystickNames().Length; i++)
            {
                currentControllers[i] = Input.GetJoystickNames()[i].ToLower();
                if ((currentControllers[i] == "controller (xbox 360 for windows)" || currentControllers[i] == "controller (xbox 360 wireless receiver for windows)" || currentControllers[i] == "controller (xbox one for windows)"))
                {
                    xboxController = true;
                    keyboard = false;
                    playstationController = false;
                }
                else if (currentControllers[i] == "wireless controller")
                {
                    playstationController = true; //not sure if wireless controller is just super generic but that's what DS4 comes up as.
                    keyboard = false;
                    xboxController = false;
                }
                else if (currentControllers[i] == "")
                {
                    numberOfControllers++;
                }
            }
            if (numberOfControllers == Input.GetJoystickNames().Length)
            {
                keyboard = true;
                xboxController = false;
                playstationController = false;
            }

            UpdateGameInput();
        }

        private void UpdateGameInput()
        {
            if (xboxController && !playstationController && !keyboard)
            {
                playerGun.UpdateInput(true);
            }
            else if (playstationController && !playstationController && !keyboard)
            {
                playerGun.UpdateInput(true);
            }
            else if (keyboard && !xboxController && !playstationController)
            {
                playerGun.UpdateInput(false);
            }
        }

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

        public List<int> gameIDS { get; } = new List<int>();
    }
}