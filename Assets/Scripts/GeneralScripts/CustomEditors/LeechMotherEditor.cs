using EnemyScripts.LeechScripts;
using GeneralScripts.GeneralComponents;
using UnityEditor;
using UnityEngine;

namespace GeneralScripts.CustomEditors
{
    public class LeechMotherEditor : Editor
    {
        #region Leech Mother Health Variables
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty leechMotherMaxHealth;
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty leechMotherHealthBar;
        #endregion

        #region Leech Mother Movement Variables
        private SerializedProperty leechMotherScale;
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty leechMotherFlySpeed;
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty leechMotherRandomYmin;
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty leechMotherRandomYmax;
        #endregion

        #region Leech Mother Objects
        private SerializedObject leechMotherMovementObject;
        private SerializedObject leechMotherHealthObject;
        private SerializedObject leechMotherObject;
        private SerializedObject leechMotherScaleObject;
        private SerializedObject leechMotherRigidBodyObject;
        #endregion

        #region Leech Mother Editors
        private Editor leechMotherMovementEditor;
        private Editor leechMotherHealthEditor;
        private Editor leechMotherEditor;
        private Editor leechMotherRigidBodyEditor;
        #endregion

        #region Leech Mother Shooting Variables
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty leechMotherShootIntervale;
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty leechMotherProjectileDamage;
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty leechMotherProjectileSpeed;
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty leechMotherProjectileToSpawn;
        #endregion

        private void OnEnable()
        {
            SetupLeechMotherEditor();

            SetupLeechMotherHealth();

            SetupLeechMotherMovement();

            SetupLeechMotherShooting();
        }

        public override void OnInspectorGUI()
        {
            SetupLeechMotherUI();
        }

        #region Leech Mother Functions
        private void SetupLeechMotherEditor()
        {
            var leechMotherPrefab = Resources.Load("Enemies/Leech/Leech Mother") as GameObject;

            if (leechMotherPrefab)
            {
                leechMotherMovementEditor = CreateEditor(leechMotherPrefab.GetComponent<LeechMovement>());
                leechMotherHealthEditor = CreateEditor(leechMotherPrefab.GetComponent<HealthComponent>());
                leechMotherEditor = CreateEditor(leechMotherPrefab.GetComponent<LeechMother>());
                leechMotherRigidBodyEditor = CreateEditor(leechMotherPrefab.GetComponent<Rigidbody2D>());

                leechMotherHealthObject = new SerializedObject(leechMotherPrefab.GetComponent<HealthComponent>());
                leechMotherMovementObject = new SerializedObject(leechMotherPrefab.GetComponent<LeechMovement>());
                leechMotherRigidBodyObject = new SerializedObject(leechMotherPrefab.GetComponent<Rigidbody2D>());

                leechMotherObject = new SerializedObject(leechMotherPrefab.GetComponent<LeechMother>());
                leechMotherScaleObject = new SerializedObject(leechMotherPrefab.transform.GetComponent<Transform>());
            }
            else
            {
                Debug.LogError("Failed to get leechMotherPrefab in Gameplay Editor");
            }
        }

        private void SetupLeechMotherShooting()
        {
            leechMotherShootIntervale = leechMotherObject.FindProperty("shootIntervale");
            leechMotherProjectileSpeed = leechMotherObject.FindProperty("projectileSpeed");
            leechMotherProjectileDamage = leechMotherObject.FindProperty("projectileDamage");
            leechMotherProjectileToSpawn = leechMotherObject.FindProperty("projectileToSpawn");
        }

        private void SetupLeechMotherHealth()
        {
            leechMotherMaxHealth = leechMotherHealthObject.FindProperty("maxHealth");
            leechMotherHealthBar = leechMotherHealthObject.FindProperty("healthBar");
        }

        private void SetupLeechMotherMovement()
        {
            leechMotherFlySpeed = leechMotherMovementObject.FindProperty("leechFlySpeed");
            leechMotherRandomYmin = leechMotherMovementObject.FindProperty("randomYMin");
            leechMotherRandomYmax = leechMotherMovementObject.FindProperty("randomYMax");
            leechMotherScale = leechMotherScaleObject.FindProperty("m_LocalScale");
        }
        #endregion

        #region Leech Mother UI
        private void SetupLeechMotherUI()
        {
            // fetch current values from the target
            leechMotherScaleObject.Update();

            // Start a code block to check for GUI changes
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(leechMotherScale, new GUIContent("Leech Mother Scale"));

            if (EditorGUI.EndChangeCheck())
            {
                // Apply values to the target
                leechMotherScaleObject.ApplyModifiedProperties();
            }

            // fetch current values from the target
            leechMotherHealthObject.Update();

            // fetch current values from the target
            leechMotherMovementObject.Update();

            // fetch current values from the target
            leechMotherObject.Update();

            if (leechMotherHealthEditor)
            {
                // Start a code block to check for GUI changes
                EditorGUI.BeginChangeCheck();

                leechMotherHealthEditor.OnInspectorGUI();

                if (EditorGUI.EndChangeCheck())
                {
                    // Apply values to the target
                    leechMotherHealthObject.ApplyModifiedProperties();

                    // fetch current values from the target
                    leechMotherHealthObject.Update();
                }
            }

            if (leechMotherMovementEditor)
            {
                // Start a code block to check for GUI changes
                EditorGUI.BeginChangeCheck();

                leechMotherMovementEditor.OnInspectorGUI();

                if (EditorGUI.EndChangeCheck())
                {
                    // Apply values to the target
                    leechMotherMovementObject.ApplyModifiedProperties();

                    // fetch current values from the target
                    leechMotherMovementObject.Update();
                }
            }

            if (leechMotherEditor)
            {
                GUILayout.Space(5f);

                // Start a code block to check for GUI changes
                EditorGUI.BeginChangeCheck();

                leechMotherEditor.OnInspectorGUI();

                if (EditorGUI.EndChangeCheck())
                {
                    // Apply values to the target
                    leechMotherObject.ApplyModifiedProperties();

                    // fetch current values from the target
                    leechMotherObject.Update();
                }
            }

            if (leechMotherRigidBodyEditor)
            {
                GUILayout.Space(5f);

                EditorGUILayout.LabelField("RigidBody Settings", EditorStyles.boldLabel);

                // Start a code block to check for GUI changes
                EditorGUI.BeginChangeCheck();

                leechMotherRigidBodyEditor.OnInspectorGUI();

                if (EditorGUI.EndChangeCheck())
                {
                    // Apply values to the target
                    leechMotherRigidBodyObject.ApplyModifiedProperties();

                    // fetch current values from the target
                    leechMotherRigidBodyObject.Update();
                }
            }

            // Apply values to the target
            leechMotherHealthObject.ApplyModifiedProperties();

            // Apply values to the target
            leechMotherMovementObject.ApplyModifiedProperties();

            // Apply values to the target
            leechMotherObject.ApplyModifiedProperties();

            // Apply values to the target
            leechMotherRigidBodyObject.ApplyModifiedProperties();

            GUILayout.Space(CustomEditorUtilities.FoldoutSpacing);
        }
        #endregion
    }
}