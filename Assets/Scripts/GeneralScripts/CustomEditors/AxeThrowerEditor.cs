using EnemyScripts;
using GeneralScripts.GeneralComponents;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GeneralScripts.CustomEditors
{
    public class AxeThrowerEditor : Editor
    {
        #region Axe Thrower Objects
        private SerializedObject axeThrowerObject;
        private SerializedObject axeThrowerHealthObject;
        private SerializedObject axeThrowerRigidBodyObject;
        #endregion

        #region Axe Thrower Shooting Variables
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty axeThrowerShootIntervale;
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty axeThrowerProjectileDamage;
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty axeThrowerProjectileSpeed;
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty axeThrowerSightLayers;
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty axeThrowerProjectileToSpawn;
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty axeThrowerDebug;
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty axeThrowerSightRange;
        #endregion

        #region Axe Thrower Health Variables
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty axeThrowerMaxHealth;
        #endregion

        #region Axe Thrower Scale Variables
        private SerializedProperty axeThrowerScale;
        private SerializedObject axeThrowerScaleObject;
        #endregion

        #region Axe Thrower Editors
        private Editor axeThrowerHealthEditor;
        private Editor axeThrowerEditor;
        private Editor axeThrowerRigidBodyEditor;
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
            axeThrowerMaxHealth = axeThrowerHealthObject.FindProperty("maxHealth");
        }

        private void SetupAxeThrowerShooting()
        {
            axeThrowerShootIntervale = axeThrowerObject.FindProperty("shootIntervale");
            axeThrowerProjectileSpeed = axeThrowerObject.FindProperty("projectileSpeed");
            axeThrowerProjectileDamage = axeThrowerObject.FindProperty("projectileDamage");
            axeThrowerSightLayers = axeThrowerObject.FindProperty("sightLayers");
            axeThrowerProjectileToSpawn = axeThrowerObject.FindProperty("projectileToSpawn");
            axeThrowerDebug = axeThrowerObject.FindProperty("drawDebug");
            axeThrowerSightRange = axeThrowerObject.FindProperty("sightRange");
            axeThrowerScale = axeThrowerScaleObject.FindProperty("m_LocalScale");
        }
        #endregion

        #region Axe Throw UI
        private void SetupAxethrowerUI()
        {
            // fetch current values from the target
            axeThrowerScaleObject.Update();

            // Start a code block to check for GUI changes
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(axeThrowerScale, new GUIContent("Axe Thrower Scale"));

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

            GUILayout.Space(CustomEditorUtilities.FoldoutSpacing);
        }
        #endregion
    }
}