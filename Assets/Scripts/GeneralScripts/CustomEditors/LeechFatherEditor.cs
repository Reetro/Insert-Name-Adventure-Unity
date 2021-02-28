using EnemyScripts.LeechScripts;
using GeneralScripts.GeneralComponents;
using UnityEditor;
using UnityEngine;

namespace GeneralScripts.CustomEditors
{
    public class LeechFatherEditor : Editor
    {
        #region Leech Father Health Variables
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty leechFatherMaxHealth;
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty leechFatherHealthBar;
        #endregion

        #region Leech Father Movement Variables
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty leechFatherFlySpeed;
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty leechFatherRandomYmin;
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty leechFatherRandomYmax;
        private SerializedProperty leechFatherScale;
        #endregion

        #region Leech Father Objects
        private SerializedObject leechFatherMovementObject;
        private SerializedObject leechFatherHealthObject;
        private SerializedObject leechFatherObject;
        private SerializedObject leechFatherScaleObject;
        private SerializedObject leechFatherRigidBodyObject;
        #endregion

        #region Leech Father Editors
        private Editor leechFatherMomentEditor;
        private Editor leechFatherHealthEditor;
        private Editor leechFatherEditor;
        private Editor leechFatherRigidBodyEditor;
        #endregion

        #region Leech Father Shooting Variables
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty leechFatherShootIntervale;
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty leechFatherProjectileDamage;
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty leechFatherProjectileSpeed;
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty leechFatherProjectileToSpawn;
        #endregion

        private void OnEnable()
        {
            SetupLeechFatherEditor();

            SetupLeechFatherShooting();

            SetupLeechFatherHealth();

            SetupLeechFatherMovement();
        }

        public override void OnInspectorGUI()
        {
            SetupLeechFatherUI();
        }

        #region Leech Father Functions
        private void SetupLeechFatherEditor()
        {
            var leechFatherPrefab = Resources.Load("Enemies/Leech/Leech Father") as GameObject;

            if (leechFatherPrefab)
            {
                leechFatherMomentEditor = CreateEditor(leechFatherPrefab.GetComponent<LeechMovement>());
                leechFatherHealthEditor = CreateEditor(leechFatherPrefab.GetComponent<HealthComponent>());
                leechFatherEditor = CreateEditor(leechFatherPrefab.GetComponent<LeechFather>());
                leechFatherRigidBodyEditor = CreateEditor(leechFatherPrefab.GetComponent<Rigidbody2D>());

                leechFatherHealthObject = new SerializedObject(leechFatherPrefab.GetComponent<HealthComponent>());
                leechFatherMovementObject = new SerializedObject(leechFatherPrefab.GetComponent<LeechMovement>());
                leechFatherObject = new SerializedObject(leechFatherPrefab.GetComponent<LeechFather>());
                leechFatherScaleObject = new SerializedObject(leechFatherPrefab.transform.GetComponent<Transform>());
                leechFatherRigidBodyObject = new SerializedObject(leechFatherPrefab.transform.GetComponent<Rigidbody2D>());
            }
            else
            {
                Debug.LogError("Failed to get leechFatherPrefab in Gameplay Editor");
            }
        }

        private void SetupLeechFatherShooting()
        {
            leechFatherShootIntervale = leechFatherObject.FindProperty("shootIntervale");
            leechFatherProjectileSpeed = leechFatherObject.FindProperty("projectileSpeed");
            leechFatherProjectileDamage = leechFatherObject.FindProperty("projectileDamage");
            leechFatherProjectileToSpawn = leechFatherObject.FindProperty("projectileToSpawn");
            leechFatherScale = leechFatherScaleObject.FindProperty("m_LocalScale");
        }

        private void SetupLeechFatherHealth()
        {
            leechFatherMaxHealth = leechFatherHealthObject.FindProperty("maxHealth");
            leechFatherHealthBar = leechFatherHealthObject.FindProperty("healthBar");
        }

        private void SetupLeechFatherMovement()
        {
            leechFatherFlySpeed = leechFatherMovementObject.FindProperty("leechFlySpeed");
            leechFatherRandomYmin = leechFatherMovementObject.FindProperty("randomYMin");
            leechFatherRandomYmax = leechFatherMovementObject.FindProperty("randomYMax");
        }
        #endregion

        #region Leech Father UI
        private void SetupLeechFatherUI()
        {
            // fetch current values from the target
            leechFatherScaleObject.Update();

            // Start a code block to check for GUI changes
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(leechFatherScale, new GUIContent("Leech Father Scale"));

            if (EditorGUI.EndChangeCheck())
            {
                // Apply values to the target
                leechFatherScaleObject.ApplyModifiedProperties();
            }

            // fetch current values from the target
            leechFatherHealthObject.Update();

            // fetch current values from the target
            leechFatherMovementObject.Update();

            // fetch current values from the target
            leechFatherObject.Update();

            if (leechFatherHealthEditor)
            {
                // fetch current values from the target
                leechFatherHealthObject.Update();

                // Start a code block to check for GUI changes
                EditorGUI.BeginChangeCheck();

                leechFatherHealthEditor.OnInspectorGUI();

                if (EditorGUI.EndChangeCheck())
                {
                    // Apply values to the target
                    leechFatherHealthObject.ApplyModifiedProperties();

                    // fetch current values from the target
                    leechFatherHealthObject.Update();
                }
            }

            if (leechFatherMomentEditor)
            {
                // fetch current values from the target
                leechFatherMovementObject.Update();

                // Start a code block to check for GUI changes
                EditorGUI.BeginChangeCheck();

                leechFatherMomentEditor.OnInspectorGUI();

                if (EditorGUI.EndChangeCheck())
                {
                    // Apply values to the target
                    leechFatherMovementObject.ApplyModifiedProperties();

                    // fetch current values from the target
                    leechFatherMovementObject.Update();
                }
            }

            if (leechFatherEditor)
            {
                GUILayout.Space(5f);

                // fetch current values from the target
                leechFatherObject.Update();

                // Start a code block to check for GUI changes
                EditorGUI.BeginChangeCheck();

                leechFatherEditor.OnInspectorGUI();

                if (EditorGUI.EndChangeCheck())
                {
                    // Apply values to the target
                    leechFatherObject.ApplyModifiedProperties();

                    // fetch current values from the target
                    leechFatherObject.Update();
                }
            }

            if (leechFatherRigidBodyEditor)
            {
                GUILayout.Space(5f);

                EditorGUILayout.LabelField("RigidBody Settings", EditorStyles.boldLabel);

                // fetch current values from the target
                leechFatherRigidBodyObject.Update();

                // Start a code block to check for GUI changes
                EditorGUI.BeginChangeCheck();

                leechFatherRigidBodyEditor.OnInspectorGUI();

                if (EditorGUI.EndChangeCheck())
                {
                    // Apply values to the target
                    leechFatherRigidBodyObject.ApplyModifiedProperties();

                    // fetch current values from the target
                    leechFatherRigidBodyObject.Update();
                }
            }

            // Apply values to the target
            leechFatherHealthObject.ApplyModifiedProperties();

            // Apply values to the target
            leechFatherMovementObject.ApplyModifiedProperties();

            // Apply values to the target
            leechFatherObject.ApplyModifiedProperties();

            // Apply values to the target
            leechFatherRigidBodyObject.ApplyModifiedProperties();

            GUILayout.Space(CustomEditorUtilities.FoldoutSpacing);
        }
    }
    #endregion
}