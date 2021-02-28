using GeneralScripts.GeneralComponents;
using PlayerScripts.PlayerCombat;
using PlayerScripts.PlayerControls;
using UnityEditor;
using UnityEngine;

namespace GeneralScripts.CustomEditors
{
    public class PlayerEditor : Editor
    {
        #region Player Movement Variables
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty playerJumpForce;
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty playerRunSpeed;
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty movementSmoothing;
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty hasAirControl;
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty playerAcceleration;
        #endregion

        #region Player Scale Variables
        private SerializedProperty playerTransformScale;
        private SerializedObject playerScaleObject;
        #endregion

        #region Player Spear Variables
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty playerSpearDamage;
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty playerSpearUpTime;
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty playerSpearTravelDistance;
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty playerSpearCooldown;
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty playerHealthComp;
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty playerSpearGround;
        #endregion

        #region Player Health Variables
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty playerMaxHealth;
        #endregion

        #region Player Leg Variables
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty playerLegIsGround;
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty playerLegBoxSize;
        #endregion

        #region Player Objects
        private SerializedObject playerMovementObject;
        private SerializedObject playerHealthObject;
        private SerializedObject playerSpearObject;
        private SerializedObject playerLegObject;
        private SerializedObject playerRigidBodyObject;
        #endregion

        #region Player Editors
        private Editor playerMovementEditor;
        private Editor playerHealthEditor;
        private Editor playerSpearEditor;
        private Editor playerLegEditor;
        private Editor playerRigidBodyEditor;
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
            playerJumpForce = playerMovementObject.FindProperty("jumpForce");
            playerRunSpeed = playerMovementObject.FindProperty("runSpeed");
            movementSmoothing = playerMovementObject.FindProperty("movementSmoothing");
            hasAirControl = playerMovementObject.FindProperty("hasAirControl");
            playerAcceleration = playerMovementObject.FindProperty("playerAcceleration");
        }

        private void SetPlayerHealth()
        {
            playerMaxHealth = playerHealthObject.FindProperty("maxHealth");
        }

        private void SetPlayerSpear()
        {
            playerSpearUpTime = playerSpearObject.FindProperty("spearReturnDelay");
            playerSpearDamage = playerSpearObject.FindProperty("spearDamage");
            playerSpearCooldown = playerSpearObject.FindProperty("spearCooldown");
            playerHealthComp = playerSpearObject.FindProperty("playerHealthComp");
            playerSpearGround = playerSpearObject.FindProperty("BlockLayers");
            playerSpearTravelDistance = playerSpearObject.FindProperty("spearTravelDistance");
        }

        private void SetPlayerLegs()
        {
            playerLegIsGround = playerLegObject.FindProperty("whatIsGround");
            playerLegBoxSize = playerLegObject.FindProperty("boxSize");
        }

        private void SetPlayerScale()
        {
            playerTransformScale = playerScaleObject.FindProperty("m_LocalScale");
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

            EditorGUILayout.PropertyField(playerTransformScale, new GUIContent("Player Scale"));

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

                GUILayout.Space(CustomEditorUtilities.FoldoutSpacing);
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


                GUILayout.Space(CustomEditorUtilities.FoldoutSpacing);
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

                GUILayout.Space(CustomEditorUtilities.FoldoutSpacing);
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

                GUILayout.Space(CustomEditorUtilities.FoldoutSpacing);
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

                GUILayout.Space(CustomEditorUtilities.FoldoutSpacing);

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