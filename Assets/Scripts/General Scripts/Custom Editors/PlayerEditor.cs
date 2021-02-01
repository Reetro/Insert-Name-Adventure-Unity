using ComponentLibrary;
using PlayerCharacter.Controller;
using UnityEditor;
using UnityEngine;

namespace CustomEditors
{
    public class PlayerEditor : Editor
    {
        #region Player Movement Variables
        private SerializedProperty _PlayerJumpForce;
        private SerializedProperty _PlayerRunSpeed;
        private SerializedProperty _MovementSmothing;
        private SerializedProperty _HasAirControl;
        private SerializedProperty _PlayerAccleration;
        #endregion

        #region Player Scale Variables
        private SerializedProperty _PlayerTransformScale;
        private SerializedObject playerScaleObject;
        #endregion

        #region Player Spear Variables
        private SerializedProperty _PlayerSpearDumage;
        private SerializedProperty _PlayerSpearUpTime;
        private SerializedProperty _PlayerSpearTravelDistance;
        private SerializedProperty _PlayerSpearCooldown;
        private SerializedProperty _PlayerHealthComp;
        private SerializedProperty _PlayerSpearGround;
        #endregion

        #region Player Health Variables
        private SerializedProperty _PlayerMaxHealth;
        #endregion

        #region Player Leg Variables
        private SerializedProperty _PlayerLegIsGround;
        private SerializedProperty _PlayerLegBoxSize;
        #endregion

        #region Player Objects
        private SerializedObject playerMovementObject;
        private SerializedObject playerHealthObject;
        private SerializedObject playerSpearObject;
        private SerializedObject playerLegObject;
        private SerializedObject playerRigidBodyObject;
        #endregion

        #region Player Editors
        private Editor playerMovementEditor = null;
        private Editor playerHealthEditor = null;
        private Editor playerSpearEditor = null;
        private Editor playerLegEditor = null;
        private Editor playerRigidBodyEditor = null;
        #endregion

        private void OnEnable()
        {
            SetupPlayer();
        }

        public override void OnInspectorGUI()
        {
            SetupPlayerUI();
        }

        #region Player Functions
        private void SetupPlayer()
        {
            SetupPlayerEditor();

            SetPlayerMovement();

            SetPlayerHealth();

            SetPlayerSpear();

            SetPlayerLegs();

            SetPlayerScale();
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
        }

        private void SetPlayerSpear()
        {
            _PlayerSpearUpTime = playerSpearObject.FindProperty("spearReturnDelay");
            _PlayerSpearDumage = playerSpearObject.FindProperty("spearDamage");
            _PlayerSpearCooldown = playerSpearObject.FindProperty("spearCooldown");
            _PlayerHealthComp = playerSpearObject.FindProperty("playerHealthComp");
            _PlayerSpearGround = playerSpearObject.FindProperty("BlockLayers");
            _PlayerSpearTravelDistance = playerSpearObject.FindProperty("spearTravelDistance");
        }

        private void SetPlayerLegs()
        {
            _PlayerLegIsGround = playerLegObject.FindProperty("whatIsGround");
            _PlayerLegBoxSize = playerLegObject.FindProperty("boxSize");
        }

        private void SetPlayerScale()
        {
            _PlayerTransformScale = playerScaleObject.FindProperty("m_LocalScale");
        }

        private void SetupPlayerEditor()
        {
            var playerPrefab = Resources.Load("Player/Player") as GameObject;

            if (playerPrefab)
            {
                playerMovementEditor = CreateEditor(playerPrefab.GetComponent<PlayerMovement>());
                playerHealthEditor = CreateEditor(playerPrefab.GetComponent<HealthComponent>());
                playerSpearEditor = CreateEditor(playerPrefab.GetComponentInChildren<PlayerSpear>());
                playerLegEditor = CreateEditor(playerPrefab.transform.GetChild(0).GetComponent<PlayerLegs>());
                playerRigidBodyEditor = CreateEditor(playerPrefab.transform.GetComponent<Rigidbody2D>());

                playerMovementObject = new SerializedObject(playerPrefab.GetComponent<PlayerMovement>());
                playerHealthObject = new SerializedObject(playerPrefab.GetComponent<HealthComponent>());
                playerSpearObject = new SerializedObject(playerPrefab.GetComponentInChildren<PlayerSpear>());
                playerLegObject = new SerializedObject(playerPrefab.transform.GetChild(0).GetComponent<PlayerLegs>());
                playerScaleObject = new SerializedObject(playerPrefab.transform.GetComponent<Transform>());
                playerRigidBodyObject = new SerializedObject(playerPrefab.transform.GetComponent<Rigidbody2D>());
            }
            else
            {
                Debug.LogError("Failed to get playerPrefab in Gameplay Editor");
            }
        }
        #endregion

        #region PlayerUI
        private void SetupPlayerUI()
        {
            // fetch current values from the target
            playerScaleObject.Update();

            // Start a code block to check for GUI changes
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(_PlayerTransformScale, new GUIContent("Player Scale"));

            if (EditorGUI.EndChangeCheck())
            {
                // Apply values to the target
                playerScaleObject.ApplyModifiedProperties();
            }

            // fetch current values from the target
            playerMovementObject.Update();

            if (playerMovementEditor)
            {
                // Start a code block to check for GUI changes
                EditorGUI.BeginChangeCheck();

                playerMovementEditor.OnInspectorGUI();

                if (EditorGUI.EndChangeCheck())
                {
                    // Apply values to the target
                    playerMovementObject.ApplyModifiedProperties();

                    // fetch current values from the target
                    playerMovementObject.Update();
                }

                GUILayout.Space(CustomEditorUtilities.foldoutSpaceing);
            }

            // fetch current values from the target
            playerHealthObject.Update();

            if (playerHealthEditor)
            {
                // Start a code block to check for GUI changes
                EditorGUI.BeginChangeCheck();

                playerHealthEditor.OnInspectorGUI();

                if (EditorGUI.EndChangeCheck())
                {
                    // Apply values to the target
                    playerHealthObject.ApplyModifiedProperties();

                    // fetch current values from the target
                    playerHealthObject.Update();
                }


                GUILayout.Space(CustomEditorUtilities.foldoutSpaceing);
            }

            // fetch current values from the target
            playerSpearObject.Update();

            if (playerSpearEditor)
            {
                // Start a code block to check for GUI changes
                EditorGUI.BeginChangeCheck();

                playerSpearEditor.OnInspectorGUI();

                if (EditorGUI.EndChangeCheck())
                {
                    // Apply values to the target
                    playerSpearObject.ApplyModifiedProperties();

                    // fetch current values from the target
                    playerSpearObject.Update();
                }

                GUILayout.Space(CustomEditorUtilities.foldoutSpaceing);
            }

            // fetch current values from the target
            playerLegObject.Update();

            if (playerLegEditor)
            {
                // Start a code block to check for GUI changes
                EditorGUI.BeginChangeCheck();

                playerLegEditor.OnInspectorGUI();

                if (EditorGUI.EndChangeCheck())
                {
                    // Apply values to the target
                    playerLegObject.ApplyModifiedProperties();

                    // fetch current values from the target
                    playerLegObject.Update();
                }

                GUILayout.Space(CustomEditorUtilities.foldoutSpaceing);
            }

            if (playerRigidBodyEditor)
            {
                GUILayout.Space(5f);

                EditorGUILayout.LabelField("RigidBody Settings", EditorStyles.boldLabel);

                // Start a code block to check for GUI changes
                EditorGUI.BeginChangeCheck();

                playerRigidBodyEditor.OnInspectorGUI();

                if (EditorGUI.EndChangeCheck())
                {
                    // Apply values to the target
                    playerRigidBodyObject.ApplyModifiedProperties();

                    // fetch current values from the target
                    playerRigidBodyObject.Update();
                }

                GUILayout.Space(CustomEditorUtilities.foldoutSpaceing);

            }

            // Apply values to the target
            playerMovementObject.ApplyModifiedProperties();

            // Apply values to the target
            playerHealthObject.ApplyModifiedProperties();

            // Apply values to the target
            playerSpearObject.ApplyModifiedProperties();

            // Apply values to the target
            playerLegObject.ApplyModifiedProperties();

            // Apply values to the target
            playerRigidBodyObject.ApplyModifiedProperties();
        }
        #endregion
    }
}