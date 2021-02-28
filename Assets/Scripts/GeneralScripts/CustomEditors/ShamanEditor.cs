using EnemyScripts.ShamanScripts;
using GeneralScripts.GeneralComponents;
using UnityEditor;
using UnityEngine;

namespace GeneralScripts.CustomEditors
{
    public class ShamanEditor : Editor
    {
        #region Shaman Objects
        private SerializedObject shamanObject;
        private SerializedObject shamanRigidBodyObject;
        private SerializedObject shamanHealthObject;
        #endregion

        #region Shaman Health Variables
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty shamanMaxHealth;
        #endregion

        #region Shaman Shooting Variables
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty shamanTeleportOffset;
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty shamanBoomerangSpeed;
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty shamanHitsBeforeTeleport;
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty shamanDamage;
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty shamanBommerangSpeedMultipler;
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty shamanBommerangMaxSpeed;
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty shamanBoomerangDamageDelay;
        // ReSharper disable once NotAccessedField.Local
        private SerializedProperty shamanBoomerangToSpawn;
        private SerializedProperty shamanScale;
        private SerializedObject shamanScaleObject;
        #endregion

        #region Shaman Editors
        private Editor shamanHealthEditor;
        private Editor shamanRigidBodyEditor;
        private Editor shamanEditor;
        #endregion


        private void OnEnable()
        {
            SetupShamanEditor();

            SetupShamanHealth();

            SetupShamanShooting();
        }

        public override void OnInspectorGUI()
        {
            SetupShamanUI();
        }

        #region Shaman Functions
        private void SetupShamanEditor()
        {
            var shamanPrefab = Resources.Load("Enemies/Shaman/Shaman") as GameObject;

            if (shamanPrefab)
            {
                shamanEditor = CreateEditor(shamanPrefab.GetComponent<Shaman>());
                shamanHealthEditor = CreateEditor(shamanPrefab.GetComponent<HealthComponent>());
                shamanRigidBodyEditor = CreateEditor(shamanPrefab.GetComponent<Rigidbody2D>());

                shamanHealthObject = new SerializedObject(shamanPrefab.GetComponent<HealthComponent>());
                shamanObject = new SerializedObject(shamanPrefab.GetComponent<Shaman>());
                shamanScaleObject = new SerializedObject(shamanPrefab.GetComponent<Transform>());
                shamanRigidBodyObject = new SerializedObject(shamanPrefab.GetComponent<Rigidbody2D>());
            }
            else
            {
                Debug.LogError("Failed to get shamanPrefab in Gameplay Editor");
            }
        }

        private void SetupShamanHealth()
        {
            shamanMaxHealth = shamanHealthObject.FindProperty("maxHealth");
        }

        private void SetupShamanShooting()
        {
            shamanBoomerangSpeed = shamanObject.FindProperty("boomerangSpeed");
            shamanDamage = shamanObject.FindProperty("boomerangDamage");
            shamanHitsBeforeTeleport = shamanObject.FindProperty("maxHitsBeforeTeleport");
            shamanTeleportOffset = shamanObject.FindProperty("teleportOffset");
            shamanBommerangSpeedMultipler = shamanObject.FindProperty("bommerangSpeedMultipler");
            shamanBoomerangToSpawn = shamanObject.FindProperty("boomerangToSpawn");
            shamanBommerangMaxSpeed = shamanObject.FindProperty("bommerangMaxSpeedMagnitude");
            shamanBoomerangDamageDelay = shamanObject.FindProperty("DamageDelay");
            shamanScale = shamanScaleObject.FindProperty("m_LocalScale");
        }
        #endregion

        #region Shaman UI
        private void SetupShamanUI()
        {
            // fetch current values from the target
            shamanScaleObject.Update();

            // Start a code block to check for GUI changes
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(shamanScale, new GUIContent("Shaman Scale"));

            if (EditorGUI.EndChangeCheck())
            {
                // Apply values to the target
                shamanScaleObject.ApplyModifiedProperties();
            }

            // fetch current values from the target
            shamanObject.Update();

            // fetch current values from the target
            shamanHealthObject.Update();

            if (shamanHealthEditor)
            {
                // Start a code block to check for GUI changes
                EditorGUI.BeginChangeCheck();

                shamanHealthEditor.OnInspectorGUI();

                if (EditorGUI.EndChangeCheck())
                {
                    // Apply values to the target
                    shamanHealthObject.ApplyModifiedProperties();

                    // fetch current values from the target
                    shamanHealthObject.Update();
                }
            }

            if (shamanEditor)
            {
                // Start a code block to check for GUI changes
                EditorGUI.BeginChangeCheck();

                shamanEditor.OnInspectorGUI();

                if (EditorGUI.EndChangeCheck())
                {
                    // Apply values to the target
                    shamanHealthObject.ApplyModifiedProperties();

                    // fetch current values from the target
                    shamanHealthObject.Update();
                }
            }

            if (shamanRigidBodyEditor)
            {
                GUILayout.Space(5f);

                EditorGUILayout.LabelField("RigidBody Settings", EditorStyles.boldLabel);

                // Start a code block to check for GUI changes
                EditorGUI.BeginChangeCheck();

                shamanRigidBodyEditor.OnInspectorGUI();

                if (EditorGUI.EndChangeCheck())
                {
                    // Apply values to the target
                    shamanRigidBodyObject.ApplyModifiedProperties();

                    // fetch current values from the target
                    shamanRigidBodyObject.Update();
                }
            }

            // Apply values to the target
            shamanObject.ApplyModifiedProperties();

            // Apply values to the target
            shamanHealthObject.ApplyModifiedProperties();

            // Apply values to the target
            shamanRigidBodyObject.ApplyModifiedProperties();

            GUILayout.Space(CustomEditorUtilities.FoldoutSpacing);
        }
        #endregion
    }
}