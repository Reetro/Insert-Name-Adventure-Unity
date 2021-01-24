using ComponentLibrary;
using EnemyCharacter.AI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CustomEditors
{
    public class SlugEditor : Editor
    {
        #region Slug Health Variables
        private SerializedProperty __SlugMaxHealth;
        #endregion

        #region Slug Scale Variables
        private SerializedProperty _SlugScale;
        private SerializedObject slugScaleObject;
        #endregion

        #region Slug Movement Variables
        private SerializedProperty _SlugMoveSpeed;
        private SerializedProperty _SlugTraceDistance;
        private SerializedProperty _SlugCanSee;
        private SerializedProperty _SlugDebug;
        #endregion

        #region Slug Damage Variables
        private SerializedProperty _SlugDamage;
        private SerializedProperty _SlugKnockbackForce;
        #endregion

        #region Slug Objects
        private SerializedObject slugMovementObject;
        private SerializedObject slugRigidBodyObject;
        private SerializedObject slugHealthObject;
        #endregion

        #region Slug Editors
        private Editor slugMovementEditor = null;
        private Editor slugHealthEditor = null;
        private Editor slugRigidBodyEditor = null;
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
            __SlugMaxHealth = slugHealthObject.FindProperty("maxHealth");
        }

        private void SetupSlugMovementEditor()
        {
            _SlugDamage = slugMovementObject.FindProperty("damageToPlayer");
            _SlugCanSee = slugMovementObject.FindProperty("whatCanSlugSee");
            _SlugKnockbackForce = slugMovementObject.FindProperty("knockBackForce");
            _SlugMoveSpeed = slugMovementObject.FindProperty("moveSpeed");
            _SlugTraceDistance = slugMovementObject.FindProperty("traceDistance");
            _SlugDebug = slugMovementObject.FindProperty("drawDebug");
            _SlugScale = slugScaleObject.FindProperty("m_LocalScale");
        }
        #endregion

        #region Slug UI
        private void SetupSlugUI()
        {
            // fetch current values from the target
            slugScaleObject.Update();

            // Start a code block to check for GUI changes
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(_SlugScale, new GUIContent("Slug Scale"));

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

            GUILayout.Space(CustomEditorUtilities.foldoutSpaceing);
        }
        #endregion
    }
}