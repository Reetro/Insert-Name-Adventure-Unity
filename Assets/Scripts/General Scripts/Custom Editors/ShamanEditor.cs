using ComponentLibrary;
using EnemyCharacter.AI;
using UnityEditor;
using UnityEngine;

namespace CustomEditors
{
    public class ShamanEditor : Editor
    {
        #region Shaman Objects
        private SerializedObject shamanObject;
        private SerializedObject shamanRigidBodyObject;
        private SerializedObject shamanHealthObject;
        #endregion

        #region Shaman Health Variables
        private SerializedProperty _ShamanMaxHealth;
        #endregion

        #region Shaman Shooting Variables
        private SerializedProperty _ShamanTeleportOffset;
        private SerializedProperty _ShamanBoomerangSpeed;
        private SerializedProperty _ShamanHitsBeforeTeleport;
        private SerializedProperty _ShamanDamage;
        private SerializedProperty _ShamanBommerangSpeedMultipler;
        private SerializedProperty _ShamanBommerangMaxSpeed;
        private SerializedProperty _ShamanBoomerangDamageDelay;
        private SerializedProperty _ShamanBoomerangToSpawn;
        private SerializedProperty _ShamanScale;
        private SerializedObject shamanScaleObject;
        #endregion

        #region Shaman Editors
        private Editor shamanHealthEditor = null;
        private Editor shamanRigidBodyEditor = null;
        private Editor shamanEditor = null;
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
            _ShamanMaxHealth = shamanHealthObject.FindProperty("maxHealth");
        }

        private void SetupShamanShooting()
        {
            _ShamanBoomerangSpeed = shamanObject.FindProperty("boomerangSpeed");
            _ShamanDamage = shamanObject.FindProperty("boomerangDamage");
            _ShamanHitsBeforeTeleport = shamanObject.FindProperty("maxHitsBeforeTeleport");
            _ShamanTeleportOffset = shamanObject.FindProperty("teleportOffset");
            _ShamanBommerangSpeedMultipler = shamanObject.FindProperty("bommerangSpeedMultipler");
            _ShamanBoomerangToSpawn = shamanObject.FindProperty("boomerangToSpawn");
            _ShamanBommerangMaxSpeed = shamanObject.FindProperty("bommerangMaxSpeedMagnitude");
            _ShamanBoomerangDamageDelay = shamanObject.FindProperty("DamageDelay");
            _ShamanScale = shamanScaleObject.FindProperty("m_LocalScale");
        }
        #endregion

        #region Shaman UI
        private void SetupShamanUI()
        {
            // fetch current values from the target
            shamanScaleObject.Update();

            // Start a code block to check for GUI changes
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(_ShamanScale, new GUIContent("Shaman Scale"));

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

            GUILayout.Space(CustomEditorUtilities.foldoutSpaceing);
        }
        #endregion
    }
}