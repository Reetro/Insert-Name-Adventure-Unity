using ComponentLibrary;
using EnemyCharacter.AI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CustomEditors
{
    public class TikiHeadEditor : Editor
    {
        #region Tiki Head Health Variables
        private SerializedProperty _TikiHeadMaxHealth;
        #endregion

        #region Tiki Head Movement Variables
        private SerializedProperty _TikiHeadLaunchDelay;
        private SerializedProperty _TikiHeadLaunchSpeed;
        private SerializedProperty _TikiHeadLaunchDistanceMultiplier;
        private SerializedProperty _TikiHeadMoveToGroundDelay;
        private SerializedProperty _TikiHeadYDistanceToTolerance;
        private SerializedProperty _TikiHeadFollowDelay;
        private SerializedProperty _TikiHeadFollowSpeed;
        private SerializedProperty _TikiHeadPlayerSquishScale;
        private SerializedProperty _TikiHeadDamageToApply;
        private SerializedProperty _TikiHeadSquishEffect;
        private SerializedProperty _TikiHeadSpriteOpacity;
        private SerializedProperty _TikiHeadKnockBackMultiplier;
        private SerializedProperty _TikiHeadTraceDistance;
        private SerializedProperty _TikiHeadSightLayers;
        private SerializedProperty _TikiHeadSightRange;
        private SerializedProperty _TikiHeadDrawDebug;
        private SerializedProperty _TikiHeadWhatIsGround;
        private SerializedProperty _TikiHeadShakeTime;
        private SerializedProperty _TikiHeadShakeIntensity;
        #endregion

        #region Tiki Head Editors
        private Editor tikiHeadMovmentEditor = null;
        private Editor tikiHeadHealthEditor = null;
        private Editor tikiHeadRigidBodyEditor = null;
        #endregion

        #region Tiki Head Scale Variables
        private SerializedProperty _TikiHeadScale;
        #endregion

        #region Tiki Head Objects
        private SerializedObject tikiHeadObject;
        private SerializedObject tikiHeadScaleObject;
        private SerializedObject tikiHeadHealthObject;
        private SerializedObject tikiHeadRigidBodyObject;
        #endregion

        private void OnEnable()
        {
            SetupTikiHeadEditor();

            SetupTikiHeadHealthEditor();

            SetupTikiHeadMovementEditor();
        }

        public override void OnInspectorGUI()
        {
            SetupTikiHeadUI();
        }

        #region Tiki Head Functions
        private void SetupTikiHeadEditor()
        {
            var tikiHead = Resources.Load("Enemies/Tiki Head/Tiki Head") as GameObject;

            if (tikiHead)
            {
                tikiHeadMovmentEditor = CreateEditor(tikiHead.GetComponent<TikiHead>());
                tikiHeadHealthEditor = CreateEditor(tikiHead.GetComponent<HealthComponent>());
                tikiHeadRigidBodyEditor = CreateEditor(tikiHead.GetComponent<Rigidbody2D>());

                tikiHeadHealthObject = new SerializedObject(tikiHead.GetComponent<HealthComponent>());
                tikiHeadObject = new SerializedObject(tikiHead.GetComponent<TikiHead>());
                tikiHeadScaleObject = new SerializedObject(tikiHead.GetComponent<Transform>());
                tikiHeadRigidBodyObject = new SerializedObject(tikiHead.GetComponent<Rigidbody2D>());
            }
            else
            {
                Debug.LogError("Failed to get slugPrefab in Gameplay Editor");
            }
        }

        private void SetupTikiHeadHealthEditor()
        {
            _TikiHeadMaxHealth = tikiHeadHealthObject.FindProperty("maxHealth");
        }

        private void SetupTikiHeadMovementEditor()
        {
            _TikiHeadLaunchDelay = tikiHeadObject.FindProperty("launchDelay");
            _TikiHeadSightLayers = tikiHeadObject.FindProperty("sightLayers");
            _TikiHeadMoveToGroundDelay = tikiHeadObject.FindProperty("moveToGroundDelay");
            _TikiHeadPlayerSquishScale = tikiHeadObject.FindProperty("playerSquishScale");
            _TikiHeadSquishEffect = tikiHeadObject.FindProperty("squishEffect");
            _TikiHeadLaunchSpeed = tikiHeadObject.FindProperty("launchSpeed");
            _TikiHeadKnockBackMultiplier = tikiHeadObject.FindProperty("knockBackMultiplier");
            _TikiHeadLaunchDistanceMultiplier = tikiHeadObject.FindProperty("launchDistanceMultiplier");
            _TikiHeadSightRange = tikiHeadObject.FindProperty("sightRange");
            _TikiHeadFollowDelay = tikiHeadObject.FindProperty("followDelay");
            _TikiHeadFollowSpeed = tikiHeadObject.FindProperty("followSpeed");
            _TikiHeadYDistanceToTolerance = tikiHeadObject.FindProperty("yDistanceTolerance");
            _TikiHeadSpriteOpacity = tikiHeadObject.FindProperty("spriteOpacity");
            _TikiHeadDamageToApply = tikiHeadObject.FindProperty("damageToApply");
            _TikiHeadMoveToGroundDelay = tikiHeadObject.FindProperty("moveToGroundDelay");
            _TikiHeadWhatIsGround = tikiHeadObject.FindProperty("whatIsGround");
            _TikiHeadSightLayers = tikiHeadObject.FindProperty("sightLayers");
            _TikiHeadDrawDebug = tikiHeadObject.FindProperty("drawDebug");
            _TikiHeadShakeIntensity = tikiHeadObject.FindProperty("cameraShakeIntensity");
            _TikiHeadShakeTime = tikiHeadObject.FindProperty("cameraShakeTime");
            _TikiHeadScale = tikiHeadScaleObject.FindProperty("m_LocalScale");
        }
        #endregion

        #region Tiki Head UI
        private void SetupTikiHeadUI()
        {
            // fetch current values from the target
            tikiHeadScaleObject.Update();

            // Start a code block to check for GUI changes
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(_TikiHeadScale, new GUIContent("Tiki Head Scale"));

            if (EditorGUI.EndChangeCheck())
            {
                // Apply values to the target
                tikiHeadScaleObject.ApplyModifiedProperties();

                EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
            }

            // fetch current values from the target
            tikiHeadHealthObject.Update();

            // fetch current values from the target
            tikiHeadObject.Update();

            if (tikiHeadMovmentEditor)
            {
                // Start a code block to check for GUI changes
                EditorGUI.BeginChangeCheck();

                tikiHeadMovmentEditor.OnInspectorGUI();

                if (EditorGUI.EndChangeCheck())
                {
                    // Apply values to the target
                    tikiHeadObject.ApplyModifiedProperties();

                    // fetch current values from the target
                    tikiHeadObject.Update();

                    EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
                }
            }

            if (tikiHeadHealthEditor)
            {
                // Start a code block to check for GUI changes
                EditorGUI.BeginChangeCheck();

                tikiHeadHealthEditor.OnInspectorGUI();

                if (EditorGUI.EndChangeCheck())
                {
                    // Apply values to the target
                    tikiHeadHealthObject.ApplyModifiedProperties();

                    // fetch current values from the target
                    tikiHeadHealthObject.Update();

                    EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
                }
            }

            if (tikiHeadRigidBodyEditor)
            {
                GUILayout.Space(5f);

                EditorGUILayout.LabelField("RigidBody Settings", EditorStyles.boldLabel);

                // Start a code block to check for GUI changes
                EditorGUI.BeginChangeCheck();

                tikiHeadRigidBodyEditor.OnInspectorGUI();

                if (EditorGUI.EndChangeCheck())
                {
                    // Apply values to the target
                    tikiHeadRigidBodyObject.ApplyModifiedProperties();

                    // fetch current values from the target
                    tikiHeadRigidBodyObject.Update();

                    EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
                }
            }

            // Apply values to the target
            tikiHeadHealthObject.ApplyModifiedProperties();

            // Apply values to the target
            tikiHeadObject.ApplyModifiedProperties();

            // Apply values to the target
            tikiHeadRigidBodyObject.ApplyModifiedProperties();

            GUILayout.Space(CustomEditorUtilities.foldoutSpaceing);
        }
        #endregion
    }
}