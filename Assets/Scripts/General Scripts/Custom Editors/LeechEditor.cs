using ComponentLibrary;
using EnemyCharacter.AI;
using UnityEditor;
using UnityEngine;

namespace CustomEditors
{
    public class LeechEditor : Editor
    {
        #region Leech Health Variables
        private SerializedProperty _LeechMaxHealth;
        #endregion

        #region Leech Movement Variables
        private SerializedProperty _LeechFlySpeed;
        private SerializedProperty _LeechRandomYmin;
        private SerializedProperty _LeechRandomYmax;
        #endregion

        #region Leech Objects
        private SerializedObject leechMovementObject;
        private SerializedObject leechHealthObject;
        private SerializedProperty _LeechScale;
        private SerializedObject leechScaleObject;
        private SerializedObject leechRigidBodyObject;
        #endregion

        #region Leech Editors
        private Editor leechMovmentEditor = null;
        private Editor leechHealthEditor = null;
        private Editor leechRigidBodyEditor = null;
        #endregion

        private void OnEnable()
        {
            SetupLeechEditor();

            SetLeechHealth();

            SetLeechMovement();
        }

        public override void OnInspectorGUI()
        {
            SetupLeechUI();
        }

        #region Leech UI
        private void SetupLeechUI()
        {
            // fetch current values from the target
            leechScaleObject.Update();

            // Start a code block to check for GUI changes
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(_LeechScale, new GUIContent("Leech Scale"));

            if (EditorGUI.EndChangeCheck())
            {
                // Apply values to the target
                leechScaleObject.ApplyModifiedProperties();
            }

            // fetch current values from the target
            leechHealthObject.Update();

            // fetch current values from the target
            leechMovementObject.Update();

            if (leechHealthEditor)
            {
                // Start a code block to check for GUI changes
                EditorGUI.BeginChangeCheck();

                leechHealthEditor.OnInspectorGUI();

                if (EditorGUI.EndChangeCheck())
                {
                    // Apply values to the target
                    leechHealthObject.ApplyModifiedProperties();

                    // fetch current values from the target
                    leechHealthObject.Update();
                }
            }

            if (leechMovmentEditor)
            {
                // Start a code block to check for GUI changes
                EditorGUI.BeginChangeCheck();

                leechMovmentEditor.OnInspectorGUI();

                if (EditorGUI.EndChangeCheck())
                {
                    // Apply values to the target
                    leechMovementObject.ApplyModifiedProperties();

                    // fetch current values from the target
                    leechMovementObject.Update();
                }
            }

            if (leechRigidBodyEditor)
            {
                GUILayout.Space(5f);

                EditorGUILayout.LabelField("RigidBody Settings", EditorStyles.boldLabel);

                // Start a code block to check for GUI changes
                EditorGUI.BeginChangeCheck();

                leechRigidBodyEditor.OnInspectorGUI();

                if (EditorGUI.EndChangeCheck())
                {
                    // Apply values to the target
                    leechRigidBodyObject.ApplyModifiedProperties();

                    // fetch current values from the target
                    leechRigidBodyObject.Update();
                }
            }

            // Apply values to the target
            leechHealthObject.ApplyModifiedProperties();

            // Apply values to the target
            leechMovementObject.ApplyModifiedProperties();

            // Apply values to the target
            leechRigidBodyObject.ApplyModifiedProperties();

            GUILayout.Space(CustomEditorUtilities.foldoutSpaceing);
        }
        #endregion

        #region Leech Functions
        private void SetupLeechEditor()
        {
            var leechPrefab = Resources.Load("Enemies/Leech/Leech") as GameObject;

            if (leechPrefab)
            {
                leechMovmentEditor = CreateEditor(leechPrefab.GetComponent<LeechMovement>());
                leechHealthEditor = CreateEditor(leechPrefab.GetComponent<HealthComponent>());
                leechRigidBodyEditor = CreateEditor(leechPrefab.GetComponent<Rigidbody2D>());

                leechHealthObject = new SerializedObject(leechPrefab.GetComponent<HealthComponent>());
                leechMovementObject = new SerializedObject(leechPrefab.GetComponent<LeechMovement>());
                leechScaleObject = new SerializedObject(leechPrefab.transform.GetComponent<Transform>());
                leechRigidBodyObject = new SerializedObject(leechPrefab.GetComponent<Rigidbody2D>());
            }
            else
            {
                Debug.LogError("Failed to get leechPrefab in Gameplay Editor");
            }
        }

        private void SetLeechHealth()
        {
            _LeechMaxHealth = leechHealthObject.FindProperty("maxHealth");
        }

        private void SetLeechMovement()
        {
            _LeechFlySpeed = leechMovementObject.FindProperty("leechFlySpeed");
            _LeechRandomYmin = leechMovementObject.FindProperty("randomYMin");
            _LeechRandomYmax = leechMovementObject.FindProperty("randomYMax");
            _LeechScale = leechScaleObject.FindProperty("m_LocalScale");
        }
        #endregion
    }
}