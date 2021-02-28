using EnemyScripts;
using GeneralScripts.GeneralComponents;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GeneralScripts.CustomEditors
{
    public class SlugEditor : Editor
    {
        #region Slug Health Variables
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty slugMaxHealth;
        #endregion

        #region Slug Scale Variables
        private SerializedProperty slugScale;
        private SerializedObject slugScaleObject;
        #endregion

        #region Slug Movement Variables
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty slugMoveSpeed;
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty slugTraceDistance;
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty slugCanSee;
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty slugDebug;
        #endregion

        #region Slug Damage Variables
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty slugDamage;
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty slugKnockbackForce;
        #endregion

        #region Slug Objects
        private SerializedObject slugMovementObject;
        private SerializedObject slugRigidBodyObject;
        private SerializedObject slugHealthObject;
        #endregion

        #region Slug Editors
        private Editor slugMovementEditor;
        private Editor slugHealthEditor;
        private Editor slugRigidBodyEditor;
        #endregion

        private void OnEnable()
        {
            SetupSlugEditor();

            SetupSlugHealthEditor();

            SetupSlugMovementEditor();
        }

        public override void OnInspectorGUI()
        {
            SetupSlugUI();
        }

        #region Slug Functions
        private void SetupSlugEditor()
        {
            var slugPrefab = Resources.Load("Enemies/Slug/Slug") as GameObject;

            if (slugPrefab)
            {
                slugMovementEditor = CreateEditor(slugPrefab.GetComponent<SlugMovement>());
                slugHealthEditor = CreateEditor(slugPrefab.GetComponent<HealthComponent>());
                slugRigidBodyEditor = CreateEditor(slugPrefab.GetComponent<Rigidbody2D>());

                slugHealthObject = new SerializedObject(slugPrefab.GetComponent<HealthComponent>());
                slugMovementObject = new SerializedObject(slugPrefab.GetComponent<SlugMovement>());
                slugScaleObject = new SerializedObject(slugPrefab.GetComponent<Transform>());
                slugRigidBodyObject = new SerializedObject(slugPrefab.GetComponent<Rigidbody2D>());
            }
            else
            {
                Debug.LogError("Failed to get slugPrefab in Gameplay Editor");
            }
        }

        private void SetupSlugHealthEditor()
        {
            slugMaxHealth = slugHealthObject.FindProperty("maxHealth");
        }

        private void SetupSlugMovementEditor()
        {
            slugDamage = slugMovementObject.FindProperty("damageToPlayer");
            slugCanSee = slugMovementObject.FindProperty("whatCanSlugSee");
            slugKnockbackForce = slugMovementObject.FindProperty("knockBackForce");
            slugMoveSpeed = slugMovementObject.FindProperty("moveSpeed");
            slugTraceDistance = slugMovementObject.FindProperty("traceDistance");
            slugDebug = slugMovementObject.FindProperty("drawDebug");
            slugScale = slugScaleObject.FindProperty("m_LocalScale");
        }
        #endregion

        #region Slug UI
        private void SetupSlugUI()
        {
            // fetch current values from the target
            slugScaleObject.Update();

            // Start a code block to check for GUI changes
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(slugScale, new GUIContent("Slug Scale"));

            if (EditorGUI.EndChangeCheck())
            {
                // Apply values to the target
                slugScaleObject.ApplyModifiedProperties();

                EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
            }

            // fetch current values from the target
            slugHealthObject.Update();

            // fetch current values from the target
            slugMovementObject.Update();

            if (slugMovementEditor)
            {
                // Start a code block to check for GUI changes
                EditorGUI.BeginChangeCheck();

                slugMovementEditor.OnInspectorGUI();

                if (EditorGUI.EndChangeCheck())
                {
                    // Apply values to the target
                    slugMovementObject.ApplyModifiedProperties();

                    // fetch current values from the target
                    slugMovementObject.Update();

                    EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
                }
            }

            if (slugHealthEditor)
            {
                // Start a code block to check for GUI changes
                EditorGUI.BeginChangeCheck();

                slugHealthEditor.OnInspectorGUI();

                if (EditorGUI.EndChangeCheck())
                {
                    // Apply values to the target
                    slugHealthObject.ApplyModifiedProperties();

                    // fetch current values from the target
                    slugHealthObject.Update();

                    EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
                }
            }

            if (slugRigidBodyEditor)
            {
                GUILayout.Space(5f);

                EditorGUILayout.LabelField("RigidBody Settings", EditorStyles.boldLabel);

                // Start a code block to check for GUI changes
                EditorGUI.BeginChangeCheck();

                slugRigidBodyEditor.OnInspectorGUI();

                if (EditorGUI.EndChangeCheck())
                {
                    // Apply values to the target
                    slugRigidBodyObject.ApplyModifiedProperties();

                    // fetch current values from the target
                    slugRigidBodyObject.Update();

                    EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
                }
            }

            // Apply values to the target
            slugHealthObject.ApplyModifiedProperties();

            // Apply values to the target
            slugMovementObject.ApplyModifiedProperties();

            // Apply values to the target
            slugRigidBodyObject.ApplyModifiedProperties();

            GUILayout.Space(CustomEditorUtilities.FoldoutSpacing);
        }
        #endregion
    }
}