using ComponentLibrary;
using EnemyCharacter.AI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CustomEditors
{
    public class AxeThrowerEditor : Editor
    {
        #region Axe Thrower Objects
        private SerializedObject axeThrowerObject;
        private SerializedObject axeThrowerHealthObject;
        private SerializedObject axeThrowerRigidBodyObject;
        #endregion

        #region Axe Thrower Shooting Variables
        private SerializedProperty _AxeThrowerShootIntervale;
        private SerializedProperty _AxeThrowerProjectileDamage;
        private SerializedProperty _AxeThrowerProjectileSpeed;
        private SerializedProperty _AxeThrowerSightLayers;
        private SerializedProperty _AxeThrowerProjectileToSpawn;
        private SerializedProperty _AxeThrowerDebug;
        private SerializedProperty _AxeThrowerSightRange;
        #endregion

        #region Axe Thrower Health Variables
        private SerializedProperty _AxeThrowerMaxHealth;
        #endregion

        #region Axe Thrower Scale Variables
        private SerializedProperty _AxeThrowerScale;
        private SerializedObject axeThrowerScaleObject;
        #endregion

        #region Axe Thrower Editors
        private Editor axeThrowerHealthEditor = null;
        private Editor axeThrowerEditor = null;
        private Editor axeThrowerRigidBodyEditor = null;
        #endregion

        private void OnEnable()
        {
            SetupAxeThrowerEditor();

            SetupAxeThrowerHealth();

            SetupAxeThrowerShooting();
        }

        public override void OnInspectorGUI()
        {
            SetupAxethrowerUI();
        }

        #region Axe Thrower Functions
        private void SetupAxeThrowerEditor()
        {
            var axeThrowerPrefab = Resources.Load("Enemies/Axe Thrower/Axe Thrower") as GameObject;

            if (axeThrowerPrefab)
            {
                axeThrowerHealthEditor = CreateEditor(axeThrowerPrefab.GetComponent<HealthComponent>());
                axeThrowerEditor = CreateEditor(axeThrowerPrefab.GetComponent<AxeThrower>());
                axeThrowerRigidBodyEditor = CreateEditor(axeThrowerPrefab.GetComponent<Rigidbody2D>());


                axeThrowerObject = new SerializedObject(axeThrowerPrefab.GetComponent<AxeThrower>());
                axeThrowerHealthObject = new SerializedObject(axeThrowerPrefab.GetComponent<HealthComponent>());
                axeThrowerScaleObject = new SerializedObject(axeThrowerPrefab.GetComponent<Transform>());
                axeThrowerRigidBodyObject = new SerializedObject(axeThrowerPrefab.GetComponent<Rigidbody2D>());
            }
            else
            {
                Debug.LogError("Failed to get axeThrowerPrefab in Gameplay Editor");
            }
        }

        private void SetupAxeThrowerHealth()
        {
            _AxeThrowerMaxHealth = axeThrowerHealthObject.FindProperty("maxHealth");
        }

        private void SetupAxeThrowerShooting()
        {
            _AxeThrowerShootIntervale = axeThrowerObject.FindProperty("shootIntervale");
            _AxeThrowerProjectileSpeed = axeThrowerObject.FindProperty("projectileSpeed");
            _AxeThrowerProjectileDamage = axeThrowerObject.FindProperty("projectileDamage");
            _AxeThrowerSightLayers = axeThrowerObject.FindProperty("sightLayers");
            _AxeThrowerProjectileToSpawn = axeThrowerObject.FindProperty("projectileToSpawn");
            _AxeThrowerDebug = axeThrowerObject.FindProperty("drawDebug");
            _AxeThrowerSightRange = axeThrowerObject.FindProperty("sightRange");
            _AxeThrowerScale = axeThrowerScaleObject.FindProperty("m_LocalScale");
        }
        #endregion

        #region Axe Throw UI
        private void SetupAxethrowerUI()
        {
            // fetch current values from the target
            axeThrowerScaleObject.Update();

            // Start a code block to check for GUI changes
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(_AxeThrowerScale, new GUIContent("Axe Thrower Scale"));

            if (EditorGUI.EndChangeCheck())
            {
                // Apply values to the target
                axeThrowerScaleObject.ApplyModifiedProperties();

                EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
            }

            // fetch current values from the target
            axeThrowerObject.Update();

            // fetch current values from the target
            axeThrowerHealthObject.Update();

            if (axeThrowerHealthEditor)
            {
                // Start a code block to check for GUI changes
                EditorGUI.BeginChangeCheck();

                axeThrowerHealthEditor.OnInspectorGUI();

                if (EditorGUI.EndChangeCheck())
                {
                    // Apply values to the target
                    axeThrowerHealthObject.ApplyModifiedProperties();

                    // fetch current values from the target
                    axeThrowerHealthObject.Update();

                    EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
                }
            }

            if (axeThrowerEditor)
            {
                // Start a code block to check for GUI changes
                EditorGUI.BeginChangeCheck();

                axeThrowerEditor.OnInspectorGUI();

                if (EditorGUI.EndChangeCheck())
                {
                    // Apply values to the target
                    axeThrowerObject.ApplyModifiedProperties();

                    // fetch current values from the target
                    axeThrowerObject.Update();

                    EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
                }
            }

            if (axeThrowerRigidBodyEditor)
            {
                GUILayout.Space(5f);

                EditorGUILayout.LabelField("RigidBody Settings", EditorStyles.boldLabel);

                // Start a code block to check for GUI changes
                EditorGUI.BeginChangeCheck();

                axeThrowerRigidBodyEditor.OnInspectorGUI();

                if (EditorGUI.EndChangeCheck())
                {
                    // Apply values to the target
                    axeThrowerRigidBodyObject.ApplyModifiedProperties();

                    // fetch current values from the target
                    axeThrowerRigidBodyObject.Update();

                    EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
                }
            }

            // Apply values to the target
            axeThrowerObject.ApplyModifiedProperties();

            // Apply values to the target
            axeThrowerHealthObject.ApplyModifiedProperties();

            // Apply values to the target
            axeThrowerRigidBodyObject.ApplyModifiedProperties();

            GUILayout.Space(CustomEditorUtilities.foldoutSpaceing);
        }
        #endregion
    }
}