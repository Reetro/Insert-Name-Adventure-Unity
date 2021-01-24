using ComponentLibrary;
using EnemyCharacter.AI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CustomEditors
{
    public class LeechMotherEditor : Editor
    {
        #region Leech Mother Health Variables
        private SerializedProperty _LeechMotherMaxHealth;
        private SerializedProperty _LeechMotherHealthBar;
        #endregion

        #region Leech Mother Movement Variables
        private SerializedProperty _LeechMotherScale;
        private SerializedProperty _LeechMotherFlySpeed;
        private SerializedProperty _LeechMotherRandomYmin;
        private SerializedProperty _LeechMotherRandomYmax;
        #endregion

        #region Leech Mother Objects
        private SerializedObject leechMotherMovementObject;
        private SerializedObject leechMotherHealthObject;
        private SerializedObject leechMotherObject;
        private SerializedObject leechMotherScaleObject;
        private SerializedObject leechMotherRigidBodyObject;
        #endregion

        #region Leech Mother Editors
        private Editor leechMotherMovmentEditor = null;
        private Editor leechMotherHealthEditor = null;
        private Editor leechMotherEditor = null;
        private Editor leechMotherRigidBodyEditor = null;
        #endregion

        #region Leech Mother Shooting Variables
        private SerializedProperty _LeechMotherShootIntervale;
        private SerializedProperty _LeechMotherProjectileDamage;
        private SerializedProperty _LeechMotherProjectileSpeed;
        private SerializedProperty _LeechMotherProjectileToSpawn;
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
                leechMotherMovmentEditor = CreateEditor(leechMotherPrefab.GetComponent<LeechMovement>());
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
            _LeechMotherShootIntervale = leechMotherObject.FindProperty("shootIntervale");
            _LeechMotherProjectileSpeed = leechMotherObject.FindProperty("projectileSpeed");
            _LeechMotherProjectileDamage = leechMotherObject.FindProperty("projectileDamage");
            _LeechMotherProjectileToSpawn = leechMotherObject.FindProperty("projectileToSpawn");
        }

        private void SetupLeechMotherHealth()
        {
            _LeechMotherMaxHealth = leechMotherHealthObject.FindProperty("maxHealth");
            _LeechMotherHealthBar = leechMotherHealthObject.FindProperty("healthBar");
        }

        private void SetupLeechMotherMovement()
        {
            _LeechMotherFlySpeed = leechMotherMovementObject.FindProperty("leechFlySpeed");
            _LeechMotherRandomYmin = leechMotherMovementObject.FindProperty("randomYMin");
            _LeechMotherRandomYmax = leechMotherMovementObject.FindProperty("randomYMax");
            _LeechMotherScale = leechMotherScaleObject.FindProperty("m_LocalScale");
        }
        #endregion

        #region Leech Mother UI
        private void SetupLeechMotherUI()
        {
            // fetch current values from the target
            leechMotherScaleObject.Update();

            // Start a code block to check for GUI changes
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(_LeechMotherScale, new GUIContent("Leech Mother Scale"));

            if (EditorGUI.EndChangeCheck())
            {
                // Apply values to the target
                leechMotherScaleObject.ApplyModifiedProperties();

                EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
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

                    EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
                }
            }

            if (leechMotherMovmentEditor)
            {
                // Start a code block to check for GUI changes
                EditorGUI.BeginChangeCheck();

                leechMotherMovmentEditor.OnInspectorGUI();

                if (EditorGUI.EndChangeCheck())
                {
                    // Apply values to the target
                    leechMotherMovementObject.ApplyModifiedProperties();

                    // fetch current values from the target
                    leechMotherMovementObject.Update();

                    EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
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

                    EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
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

                    EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
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

            GUILayout.Space(CustomEditorUtilities.foldoutSpaceing);
        }
        #endregion
    }
}