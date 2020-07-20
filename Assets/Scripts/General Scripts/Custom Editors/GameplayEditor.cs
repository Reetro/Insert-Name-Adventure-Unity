using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using PlayerCharacter.Controller;
using EnemyCharacter.AI;

namespace CustomEditors
{
    public class GameplayEditor : CustomEditorBase
    {
        #region Player Movement Varaibles
        private SerializedProperty _PlayerJumpForce;
        private SerializedProperty _PlayerRunSpeed;
        private SerializedProperty _MovementSmothing;
        private SerializedProperty _HasAirControl;
        private SerializedProperty _PlayerAccleration;
        #endregion

        #region Player Gun Varaibles
        private SerializedProperty _PlayerLaserUpTime;
        private SerializedProperty _PlayerGunDumage;
        private SerializedProperty _PlayerGunCooldown;
        private SerializedProperty _GunProjectile;
        private SerializedProperty _GunFireLocation;
        private SerializedProperty _GunController;
        private SerializedProperty _CooldownBar;
        private SerializedProperty _PlayerHealthComp;
        #endregion

        #region Player Health Varaibles
        private SerializedProperty _PlayerMaxHealth;
        private SerializedProperty _PlayerHealthBar;
        private SerializedProperty _PlayerOnDeath;
        private SerializedProperty _PlayerTakeAnyDamage;
        #endregion

        #region Player Objects
        private SerializedObject playerMovementObject;
        private SerializedObject playerHealthObject;
        private SerializedObject playerGunOject;
        #endregion

        #region Player Editors
        private Editor playerMovementEditor = null;
        private Editor playerHealthEditor = null;
        private Editor playerGunEditor = null;
        #endregion

        #region Leech Health Varaibles
        private SerializedProperty _LeechMaxHealth;
        private SerializedProperty _LeechHealthBar;
        private SerializedProperty _LeechOnDeath;
        private SerializedProperty _LeechTakeAnyDamage;
        #endregion

        #region Leech Movement Varaibles
        private SerializedProperty _LeechFlySpeed;
        private SerializedProperty _LeechRandomYmin;
        private SerializedProperty _LeechRandomYmax;
        #endregion

        #region Leech Objects
        private SerializedObject leechMovementObject;
        private SerializedObject leechHealthObject;
        #endregion

        #region Leech Editors
        private Editor leechMovmentEditor = null;
        private Editor leechHealthEditor = null;
        #endregion

        #region Leech Father Health Varaibles
        private SerializedProperty _LeechFatherMaxHealth;
        private SerializedProperty _LeechFatherHealthBar;
        private SerializedProperty _LeechFatherOnDeath;
        private SerializedProperty _LeechFatherTakeAnyDamage;
        #endregion

        #region Leech Father Movement Varaibles
        private SerializedProperty _LeechFatherFlySpeed;
        private SerializedProperty _LeechFatherRandomYmin;
        private SerializedProperty _LeechFatherRandomYmax;
        #endregion

        #region Leech Father Objects
        private SerializedObject leechFatherMovementObject;
        private SerializedObject leechFatherHealthObject;
        private SerializedObject leechFatherObject;
        #endregion

        #region Leech Father Editors
        private Editor leechFatherMovmentEditor = null;
        private Editor leechFatherHealthEditor = null;
        private Editor leechFatherEditor = null;
        #endregion

        #region Leech Father Shooting Varaibles
        private SerializedProperty _LeechFatherProjectilePrefab;
        private SerializedProperty _LeechFatherShootIntervale;
        private SerializedProperty _LeechFatherProjectileDamage;
        private SerializedProperty _LeechFatherFirePoint;
        private SerializedProperty _LeechFatherProjectileSpeed;
        #endregion

        private Vector2 scrollPosition = Vector2.zero;

        [MenuItem("Window/Gameplay Editor")]
        public static void ShowWindow()
        {
            GetWindow<GameplayEditor>("Gameplay Editor");
        }

        public override void OnEnable()
        {
            base.OnEnable();

            SetupPlayer();

            SetupEnemies();
        }

        public override void UpdateWindow()
        {
            SetupPlayer();

            SetupEnemies();
        }

        private void SetupEnemies()
        {
            SetupLeechEditor();

            SetLeechHealth();

            SetLeechMovement();

            SetupLeechFatherEditor();

            SetupLeechFatherShooting();
        }

        private void SetupPlayer()
        {
            SetupPlayerEditor();

            SetPlayerMovement();

            SetPlayerHealth();

            SetPlayerGun();
        }

        private void SetPlayerMovement()
        {
            _PlayerJumpForce = playerMovementObject.FindProperty("jumpForce");
            _PlayerRunSpeed = playerMovementObject.FindProperty("runSpeed");
            _MovementSmothing = playerMovementObject.FindProperty("movementSmoothing");
            _HasAirControl = playerMovementObject.FindProperty("hasAirControl");
            _PlayerAccleration = playerMovementObject.FindProperty("playerAcceleration");
        }

        private void SetPlayerHealth()
        {
            _PlayerMaxHealth = playerHealthObject.FindProperty("maxHealth");
            _PlayerHealthBar = playerHealthObject.FindProperty("healthBar");
            _PlayerOnDeath = playerHealthObject.FindProperty("OnDeath");
            _PlayerTakeAnyDamage = playerHealthObject.FindProperty("onTakeAnyDamage");
        }

        private void SetPlayerGun()
        {
            _PlayerLaserUpTime = playerGunOject.FindProperty("laserUpTime");
            _PlayerGunDumage = playerGunOject.FindProperty("gunDamage");
            _PlayerGunCooldown = playerGunOject.FindProperty("gunCooldown");
            _GunProjectile = playerGunOject.FindProperty("hitBoxToSpawn");
            _GunFireLocation = playerGunOject.FindProperty("gunFireLocation");
            _GunController = playerGunOject.FindProperty("controller");
            _CooldownBar = playerGunOject.FindProperty("cooldownBar");
            _PlayerHealthComp = playerGunOject.FindProperty("playerHealthComp");
        }

        private void SetupPlayerEditor()
        {
            // Find and add player Gameobject to menu
            List<string> prefabsPaths = GeneralFunctions.FindObjectsAtPath("Assets/Player/Player.prefab");

            foreach (string currentPath in prefabsPaths)
            {
                GameObject playerPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(currentPath, typeof(GameObject));

                if (playerPrefab)
                {
                    playerMovementEditor = Editor.CreateEditor(playerPrefab.GetComponent<PlayerMovement>());
                    playerHealthEditor = Editor.CreateEditor(playerPrefab.GetComponent<HealthComponent>());
                    playerGunEditor = Editor.CreateEditor(playerPrefab.GetComponentInChildren<PlayerGun>());

                    playerMovementObject = new SerializedObject(playerPrefab.GetComponent<PlayerMovement>());
                    playerHealthObject = new SerializedObject(playerPrefab.GetComponent<HealthComponent>());
                    playerGunOject = new SerializedObject(playerPrefab.GetComponentInChildren<PlayerGun>());

                    break;
                }
            }
        }

        private void SetupLeechEditor()
        {
            List<string> prefabsPaths = GeneralFunctions.FindObjectsAtPath("Assets/Enemies/Leech/Leech.prefab");

            foreach (string currentPath in prefabsPaths)
            {
                GameObject leechPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(currentPath, typeof(GameObject));

                if (leechPrefab)
                {
                    leechMovmentEditor = Editor.CreateEditor(leechPrefab.GetComponent<LeechMovement>());
                    leechHealthEditor = Editor.CreateEditor(leechPrefab.GetComponent<HealthComponent>());

                    leechHealthObject = new SerializedObject(leechPrefab.GetComponent<HealthComponent>());
                    leechMovementObject = new SerializedObject(leechPrefab.GetComponent<LeechMovement>());

                    break;
                }
            }
        }

        private void SetupLeechFatherEditor()
        {
            List<string> prefabsPaths = GeneralFunctions.FindObjectsAtPath("Assets/Enemies/Leech/Leech Father.prefab");

            foreach (string currentPath in prefabsPaths)
            {
                GameObject leechPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(currentPath, typeof(GameObject));

                if (leechPrefab)
                {
                    leechFatherMovmentEditor = Editor.CreateEditor(leechPrefab.GetComponent<LeechMovement>());
                    leechFatherHealthEditor = Editor.CreateEditor(leechPrefab.GetComponent<HealthComponent>());
                    leechFatherEditor = Editor.CreateEditor(leechPrefab.GetComponent<LeechFather>());

                    leechFatherHealthObject = new SerializedObject(leechPrefab.GetComponent<HealthComponent>());
                    leechFatherMovementObject = new SerializedObject(leechPrefab.GetComponent<LeechMovement>());
                    leechFatherObject = new SerializedObject(leechPrefab.GetComponent<LeechFather>());

                    break;
                }
            }
        }

        private void SetupLeechFatherShooting()
        {
            _LeechFatherFirePoint = leechFatherObject.FindProperty("firePoint");
            _LeechFatherProjectilePrefab = leechFatherObject.FindProperty("projectilePrefab");
            _LeechFatherShootIntervale = leechFatherObject.FindProperty("shootIntervale");
            _LeechFatherProjectileSpeed = leechFatherObject.FindProperty("projectileSpeed");
            _LeechFatherProjectileDamage = leechFatherObject.FindProperty("projectileDamage");
        }

        private void SetLeechHealth()
        {
            _LeechHealthBar = leechHealthObject.FindProperty("healthBar");
            _LeechMaxHealth = leechHealthObject.FindProperty("maxHealth");
            _LeechOnDeath = leechHealthObject.FindProperty("OnDeath");
            _LeechTakeAnyDamage = leechHealthObject.FindProperty("onTakeAnyDamage");
        }

        private void SetLeechMovement()
        {
            _LeechFlySpeed = leechMovementObject.FindProperty("leechFlySpeed");
            _LeechRandomYmin = leechMovementObject.FindProperty("randomYMin");
            _LeechRandomYmax = leechMovementObject.FindProperty("randomYMax");
        }

        private void OnGUI()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, true, true, GUILayout.Width(position.width), GUILayout.Height(position.height));

            /// Player area start
            GUILayout.Label("Player Settings", EditorStyles.boldLabel);

            EditorStyles.boldLabel.fontSize = 15;

            // fetch current values from the target
            playerMovementObject.Update();

            if (playerMovementEditor)
            {
                playerMovementEditor.OnInspectorGUI();
            }

            // fetch current values from the target
            playerHealthObject.Update();

            if (playerHealthEditor)
            {
                playerHealthEditor.OnInspectorGUI();
            }

            // fetch current values from the target
            playerGunOject.Update();

            if (playerGunEditor)
            {
                playerGunEditor.OnInspectorGUI();
            }

            // Apply values to the target
            playerMovementObject.ApplyModifiedProperties();

            // Apply values to the target
            playerHealthObject.ApplyModifiedProperties();

            // Apply values to the target
            playerGunOject.ApplyModifiedProperties();
            /// Player area end

            GUILayout.Space(50f);

            /// Enemies area start
            GUILayout.Label("Leech", EditorStyles.boldLabel);

            // fetch current values from the target
            leechHealthObject.Update();

            // fetch current values from the target
            leechMovementObject.Update();

            if (leechHealthEditor)
            {
                leechHealthEditor.OnInspectorGUI();
            }

            if (leechMovmentEditor)
            {
                leechMovmentEditor.OnInspectorGUI();
            }

            // Apply values to the target
            leechHealthObject.ApplyModifiedProperties();

            // Apply values to the target
            leechMovementObject.ApplyModifiedProperties();

            GUILayout.Space(50f);

            GUILayout.Label("Leech Father", EditorStyles.boldLabel);

            // fetch current values from the target
            leechFatherHealthObject.Update();

            // fetch current values from the target
            leechFatherMovementObject.Update();

            // fetch current values from the target
            leechFatherObject.Update();

            if (leechFatherHealthEditor)
            {
                leechFatherHealthEditor.OnInspectorGUI();
            }

            if (leechFatherMovmentEditor)
            {
                leechFatherMovmentEditor.OnInspectorGUI();
            }

            if (leechFatherEditor)
            {
                GUILayout.Space(5f);

                leechFatherEditor.OnInspectorGUI();
            }

            // Apply values to the target
            leechFatherHealthObject.ApplyModifiedProperties();

            // Apply values to the target
            leechFatherMovementObject.ApplyModifiedProperties();

            // Apply values to the target
            leechFatherObject.ApplyModifiedProperties();

            GUILayout.Space(50f);

            GUILayout.Label("Leech Mother", EditorStyles.boldLabel);


            GUILayout.Space(50f);

            GUILayout.Label("Shaman", EditorStyles.boldLabel);


            /// Enemies area end


            GUILayout.EndScrollView();
        }
    }
}