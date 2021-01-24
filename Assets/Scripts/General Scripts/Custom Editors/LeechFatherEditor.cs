using ComponentLibrary;
using EnemyCharacter.AI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CustomEditors
{
    public class LeechFatherEditor : Editor
    {
        #region Leech Father Health Variables
        private SerializedProperty _LeechFatherMaxHealth;
        private SerializedProperty _LeechFatherHealthBar;
        #endregion

        #region Leech Father Movement Variables
        private SerializedProperty _LeechFatherFlySpeed;
        private SerializedProperty _LeechFatherRandomYmin;
        private SerializedProperty _LeechFatherRandomYmax;
        private SerializedProperty _LeechFatherScale;
        #endregion

        #region Leech Father Objects
        private SerializedObject leechFatherMovementObject;
        private SerializedObject leechFatherHealthObject;
        private SerializedObject leechFatherObject;
        private SerializedObject leechFatherScaleObject;
        private SerializedObject leechFatherRigidBodyObject;
        #endregion

        #region Leech Father Editors
        private Editor leechFatherMovmentEditor = null;
        private Editor leechFatherHealthEditor = null;
        private Editor leechFatherEditor = null;
        private Editor leechFatherRigidBodyEditor = null;
        #endregion

        #region Leech Father Shooting Variables
        private SerializedProperty _LeechFatherShootIntervale;
        private SerializedProperty _LeechFatherProjectileDamage;
        private SerializedProperty _LeechFatherProjectileSpeed;
        private SerializedProperty _LeechFatherProjectileToSpawn;
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
                leechFatherMovmentEditor = CreateEditor(leechFatherPrefab.GetComponent<LeechMovement>());
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
            _LeechFatherShootIntervale = leechFatherObject.FindProperty("shootIntervale");
            _LeechFatherProjectileSpeed = leechFatherObject.FindProperty("projectileSpeed");
            _LeechFatherProjectileDamage = leechFatherObject.FindProperty("projectileDamage");
            _LeechFatherProjectileToSpawn = leechFatherObject.FindProperty("projectileToSpawn");
            _LeechFatherScale = leechFatherScaleObject.FindProperty("m_LocalScale");
        }

        private void SetupLeechFatherHealth()
        {
            _LeechFatherMaxHealth = leechFatherHealthObject.FindProperty("maxHealth");
            _LeechFatherHealthBar = leechFatherHealthObject.FindProperty("healthBar");
        }

        private void SetupLeechFatherMovement()
        {
            _LeechFatherFlySpeed = leechFatherMovementObject.FindProperty("leechFlySpeed");
            _LeechFatherRandomYmin = leechFatherMovementObject.FindProperty("randomYMin");
            _LeechFatherRandomYmax = leechFatherMovementObject.FindProperty("randomYMax");
        }
        #endregion

        #region Leech Father UI
        private void SetupLeechFatherUI()
        {
            // fetch current values from the target
            leechFatherScaleObject.Update();

            // Start a code block to check for GUI changes
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(_LeechFatherScale, new GUIContent("Leech Father Scale"));

            if (EditorGUI.EndChangeCheck())
            {
                // Apply values to the target
                leechFatherScaleObject.ApplyModifiedProperties();

                EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
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

                    EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
                }
            }

            if (leechFatherMovmentEditor)
            {
                // fetch current values from the target
                leechFatherMovementObject.Update();

                // Start a code block to check for GUI changes
                EditorGUI.BeginChangeCheck();

                leechFatherMovmentEditor.OnInspectorGUI();

                if (EditorGUI.EndChangeCheck())
                {
                    // Apply values to the target
                    leechFatherMovementObject.ApplyModifiedProperties();

                    // fetch current values from the target
                    leechFatherMovementObject.Update();

                    EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
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

                    EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
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

                    EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
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

            GUILayout.Space(CustomEditorUtilities.foldoutSpaceing);
        }
    }
    #endregion
}