using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using PlayerCharacter.Controller;

namespace CustomEditors
{
    public class GameplayEditor : CustomEditorBase
    {
        #region Player Movement Varaibles
        private SerializedProperty _PlayerJumpForce;
        private SerializedProperty _PlayerRunSpeed;
        private SerializedProperty _MovementSmothing;
        private SerializedProperty _HasAirControl;
        private SerializedProperty _PlayerAccleration;
        #endregion

        #region Player Gun Varaibles
        private SerializedProperty _PlayerLaserUpTime;
        private SerializedProperty _PlayerGunDumage;
        private SerializedProperty _PlayerGunCooldown;
        private SerializedProperty _GunProjectile;
        private SerializedProperty _GunFireLocation;
        private SerializedProperty _GunController;
        private SerializedProperty _CooldownBar;
        private SerializedProperty _PlayerHealthComp;
        #endregion

        #region Player Health Varaibles
        private SerializedProperty _PlayerMaxHealth;
        private SerializedProperty _PlayerHealthBar;
        private SerializedProperty _PlayerOnDeath;
        private SerializedProperty _PlayerTakeAnyDamage;
        #endregion

        #region Player Objects
        private SerializedObject playerMovementObject;
        private SerializedObject playerHealthObject;
        private SerializedObject playerGunOject;
        #endregion

        #region Player Editors
        private Editor playerMovementEditor = null;
        private Editor playerHealthEditor = null;
        private Editor playerGunEditor = null;
        #endregion


        private Vector2 scrollPosition = Vector2.zero;

        public override void OnEnable()
        {
            base.OnEnable();

            SetupPlayerEditor();

            SetPlayerMovement();

            SetPlayerHealth();

            SetPlayerGun();
        }

        public override void UpdateWindow()
        {
            SetupPlayerEditor();

            SetPlayerMovement();

            SetPlayerHealth();

            SetPlayerGun();
        }

        [MenuItem("Window/Gameplay Editor")]
        public static void ShowWindow()
        {
            GetWindow<GameplayEditor>("Gameplay Editor");
        }

        private void SetPlayerMovement()
        {
            _PlayerJumpForce = playerMovementObject.FindProperty("jumpForce");
            _PlayerRunSpeed = playerMovementObject.FindProperty("runSpeed");
            _MovementSmothing = playerMovementObject.FindProperty("movementSmoothing");
            _HasAirControl = playerMovementObject.FindProperty("hasAirControl");
            _PlayerAccleration = playerMovementObject.FindProperty("playerAcceleration");
            
        }

        private void SetPlayerHealth()
        {
            _PlayerMaxHealth = playerHealthObject.FindProperty("maxHealth");
            _PlayerHealthBar = playerHealthObject.FindProperty("healthBar");
            _PlayerOnDeath = playerHealthObject.FindProperty("OnDeath");
            _PlayerTakeAnyDamage = playerHealthObject.FindProperty("onTakeAnyDamage");
        }

        private void SetPlayerGun()
        {
            _PlayerLaserUpTime = playerGunOject.FindProperty("laserUpTime");
            _PlayerGunDumage = playerGunOject.FindProperty("gunDamage");
            _PlayerGunCooldown = playerGunOject.FindProperty("gunCooldown");
            _GunProjectile = playerGunOject.FindProperty("hitBoxToSpawn");
            _GunFireLocation = playerGunOject.FindProperty("gunFireLocation");
            _GunController = playerGunOject.FindProperty("controller");
            _CooldownBar = playerGunOject.FindProperty("cooldownBar");
            _PlayerHealthComp = playerGunOject.FindProperty("playerHealthComp");
        }

        private void SetupPlayerEditor()
        {
            // Find and add player Gameobject to menu
            List<string> prefabsPaths = GeneralFunctions.FindObjectsAtPath("Assets/Player/Player.prefab");

            foreach (string currentPath in prefabsPaths)
            {
                GameObject playerPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(currentPath, typeof(GameObject));

                if (playerPrefab)
                {
                    playerMovementEditor = Editor.CreateEditor(playerPrefab.GetComponent<PlayerMovement>());
                    playerHealthEditor = Editor.CreateEditor(playerPrefab.GetComponent<HealthComponent>());
                    playerGunEditor = Editor.CreateEditor(playerPrefab.GetComponentInChildren<PlayerGun>());

                    playerMovementObject = new SerializedObject(playerPrefab.GetComponent<PlayerMovement>());
                    playerHealthObject = new SerializedObject(playerPrefab.GetComponent<HealthComponent>());
                    playerGunOject = new SerializedObject(playerPrefab.GetComponentInChildren<PlayerGun>());

                    break;
                }
            }
        }

        private void OnGUI()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, true, true, GUILayout.Width(position.width), GUILayout.Height(position.height));

            /// Player area start
            GUILayout.Label("Player Settings", EditorStyles.boldLabel);

            // fetch current values from the target
            playerMovementObject.Update();

            if (playerMovementEditor)
            {
                playerMovementEditor.OnInspectorGUI();
            }

            // fetch current values from the target
            playerHealthObject.Update();

            if (playerHealthEditor)
            {
                playerHealthEditor.OnInspectorGUI();
            }

            // fetch current values from the target
            playerGunOject.Update();

            if (playerGunEditor)
            {
                playerGunEditor.OnInspectorGUI();
            }

            // Apply values to the target
            playerMovementObject.ApplyModifiedProperties();

            // Apply values to the target
            playerHealthObject.ApplyModifiedProperties();

            // Apply values to the target
            playerGunOject.ApplyModifiedProperties();
            /// Player area end

            /// Enemies area start
            GUILayout.Space(50f);

            
            GUILayout.Label("Enemies", EditorStyles.boldLabel);
            /// Enemies area end

            GUILayout.EndScrollView();
        }
    }
}