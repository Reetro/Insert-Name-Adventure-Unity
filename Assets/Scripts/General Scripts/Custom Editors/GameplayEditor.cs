using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using PlayerCharacter.Controller;

namespace CustomEditors
{
    public class GameplayEditor : CustomEditorBase
    {
        // player editors
        private Editor playerMovementEditor = null;
        private Editor playerHealthEditor = null;

        // Editable player variables
        private SerializedProperty _PlayerJumpForce;
        private SerializedProperty _PlayerRunSpeed;
        private SerializedProperty _MovementSmothing;
        private SerializedProperty _HasAirControl;
        private SerializedProperty _PlayerAccleration;
        private SerializedProperty _PlayerMaxHealth;
        private SerializedProperty _PlayerHealthBar;
        private SerializedProperty _PlayerOnDeath;
        private SerializedProperty _PlayerTakeAnyDamage;

        private SerializedObject playerMovementObject;
        private SerializedObject playerHealthObject;

        private Vector2 scrollPosition = Vector2.zero;

        public override void OnEnable()
        {
            base.OnEnable();

            AddGameObjects();

            SetPlayerPropertyValues();
        }

        public override void UpdateWindow()
        {
            AddGameObjects();

            SetPlayerPropertyValues();
        }

        [MenuItem("Window/Gameplay Editor")]
        public static void ShowWindow()
        {
            GetWindow<GameplayEditor>("Gameplay Editor");
        }

        private void SetPlayerPropertyValues()
        {
            // Set player properties
            _PlayerJumpForce = playerMovementObject.FindProperty("jumpForce");
            _PlayerRunSpeed = playerMovementObject.FindProperty("runSpeed");
            _MovementSmothing = playerMovementObject.FindProperty("movementSmoothing");
            _HasAirControl = playerMovementObject.FindProperty("hasAirControl");
            _PlayerAccleration = playerMovementObject.FindProperty("playerAcceleration");
            _PlayerMaxHealth = playerHealthObject.FindProperty("maxHealth");
            _PlayerHealthBar = playerHealthObject.FindProperty("healthBar");
            _PlayerOnDeath = playerHealthObject.FindProperty("OnDeath");
            _PlayerTakeAnyDamage = playerHealthObject.FindProperty("onTakeAnyDamage");
        }

        private void AddGameObjects()
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

                    playerMovementObject = new SerializedObject(playerPrefab.GetComponent<PlayerMovement>());
                    playerHealthObject = new SerializedObject(playerPrefab.GetComponent<HealthComponent>());

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

            // Apply values to the target
            playerMovementObject.ApplyModifiedProperties();

            // Apply values to the target
            playerHealthObject.ApplyModifiedProperties();
            /// Player area end

            GUILayout.Space(50f);

            /// Enemies area end
            GUILayout.Label("Enemies", EditorStyles.boldLabel);




            GUILayout.EndScrollView();
        }
    }
}