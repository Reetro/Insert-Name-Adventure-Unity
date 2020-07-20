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

        #region Leech Mother Health Varaibles
        private SerializedProperty _LeechMotherMaxHealth;
        private SerializedProperty _LeechMotherHealthBar;
        private SerializedProperty _LeechMotherOnDeath;
        private SerializedProperty _LeechMotherTakeAnyDamage;
        #endregion

        #region Leech Mother Movement Varaibles
        private SerializedProperty _LeechMotherFlySpeed;
        private SerializedProperty _LeechMotherRandomYmin;
        private SerializedProperty _LeechMotherRandomYmax;
        #endregion

        #region Leech Mother Objects
        private SerializedObject leechMotherMovementObject;
        private SerializedObject leechMotherHealthObject;
        private SerializedObject leechMotherObject;
        #endregion

        #region Leech Mother Editors
        private Editor leechMotherMovmentEditor = null;
        private Editor leechMotherHealthEditor = null;
        private Editor leechMotherEditor = null;
        #endregion

        #region Leech Mother Shooting Varaibles
        private SerializedProperty _LeechMotherProjectilePrefab;
        private SerializedProperty _LeechMotherShootIntervale;
        private SerializedProperty _LeechMotherProjectileDamage;
        private SerializedProperty _LeechMotherFirePoint;
        private SerializedProperty _LeechMotherProjectileSpeed;
        #endregion

        #region Shaman Objects
        private SerializedObject shamanObject;
        private SerializedObject shamanHealthObject;
        #endregion

        #region Shaman Health Varaibles
        private SerializedProperty _ShamanMaxHealth;
        private SerializedProperty _ShamanHealthBar;
        private SerializedProperty _ShamanOnDeath;
        private SerializedProperty _ShamanTakeAnyDamage;
        #endregion

        #region Shaman Shooting Varaibles
        private SerializedProperty _ShamanProjectilePrefab;
        private SerializedProperty _ShamanTeleportOffset;
        private SerializedProperty _ShamanBoomerangSpeed;
        private SerializedProperty _ShamanHitsBeforeTeleport;
        private SerializedProperty _ShamanMinRandom;
        private SerializedProperty _ShamanMaxRandom;
        private SerializedProperty _ShamanDamage;
        #endregion

        #region Shaman Editors
        private Editor shamanHealthEditor = null;
        private Editor shamanEditor = null;
        #endregion

        #region Local Varaibles
        private Vector2 scrollPosition = Vector2.zero;
        private static bool showPlayerSettings = false;
        private static bool showLeechSettings = false;
        private static bool showLeechFatherSettings = false;
        private static bool showLeechMotherSettings = false;
        private static bool showShamanSettings = false;
        #endregion

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

        #region Enemy Functions
        private void SetupEnemies()
        {
            SetupLeechEditor();

            SetLeechHealth();

            SetLeechMovement();

            SetupLeechFatherEditor();

            SetupLeechFatherShooting();

            SetupLeechFatherHealth();

            SetupLeechMotherEditor();

            SetupLeechMotherHealth();

            SetupLeechMotherShooting();

            SetupShamanEditor();

            SetupShamanHealth();

            SetupShamanShooting();
        }

        #region Leech Functions
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
        #endregion

        #region Leech Father Functions
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

        private void SetupLeechFatherHealth()
        {
            _LeechFatherMaxHealth = leechFatherHealthObject.FindProperty("maxHealth");
            _LeechFatherHealthBar = leechFatherHealthObject.FindProperty("healthBar");
            _LeechFatherOnDeath = leechFatherHealthObject.FindProperty("OnDeath");
            _LeechFatherTakeAnyDamage = leechFatherHealthObject.FindProperty("onTakeAnyDamage");
        }
        #endregion

        #region Leech Mother Functions
        private void SetupLeechMotherEditor()
        {
            List<string> prefabsPaths = GeneralFunctions.FindObjectsAtPath("Assets/Enemies/Leech/Leech Mother.prefab");

            foreach (string currentPath in prefabsPaths)
            {
                GameObject leechPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(currentPath, typeof(GameObject));

                if (leechPrefab)
                {
                    leechMotherMovmentEditor = Editor.CreateEditor(leechPrefab.GetComponent<LeechMovement>());
                    leechMotherHealthEditor = Editor.CreateEditor(leechPrefab.GetComponent<HealthComponent>());
                    leechMotherEditor = Editor.CreateEditor(leechPrefab.GetComponent<LeechMother>());

                    leechMotherHealthObject = new SerializedObject(leechPrefab.GetComponent<HealthComponent>());
                    leechMotherMovementObject = new SerializedObject(leechPrefab.GetComponent<LeechMovement>());
                    leechMotherObject = new SerializedObject(leechPrefab.GetComponent<LeechMother>());

                    break;
                }
            }
        }

        private void SetupLeechMotherShooting()
        {
            _LeechMotherFirePoint = leechFatherObject.FindProperty("firePoint");
            _LeechMotherProjectilePrefab = leechFatherObject.FindProperty("projectilePrefab");
            _LeechMotherShootIntervale = leechFatherObject.FindProperty("shootIntervale");
            _LeechMotherProjectileSpeed = leechFatherObject.FindProperty("projectileSpeed");
            _LeechMotherProjectileDamage = leechFatherObject.FindProperty("projectileDamage");
        }

        private void SetupLeechMotherHealth()
        {
            _LeechMotherMaxHealth = leechFatherHealthObject.FindProperty("maxHealth");
            _LeechMotherHealthBar = leechFatherHealthObject.FindProperty("healthBar");
            _LeechMotherOnDeath = leechFatherHealthObject.FindProperty("OnDeath");
            _LeechMotherTakeAnyDamage = leechFatherHealthObject.FindProperty("onTakeAnyDamage");
        }
        #endregion

        #region Shaman Functions
        private void SetupShamanEditor()
        {
            List<string> prefabsPaths = GeneralFunctions.FindObjectsAtPath("Assets/Enemies/Shaman/Shaman.prefab");

            foreach (string currentPath in prefabsPaths)
            {
                GameObject shamanPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(currentPath, typeof(GameObject));

                if (shamanPrefab)
                {
                    shamanEditor = Editor.CreateEditor(shamanPrefab.GetComponent<Shaman>());
                    shamanHealthEditor = Editor.CreateEditor(shamanPrefab.GetComponent<HealthComponent>());

                    shamanHealthObject = new SerializedObject(shamanPrefab.GetComponent<HealthComponent>());

                    shamanObject = new SerializedObject(shamanPrefab.GetComponent<Shaman>());

                    break;
                }
            }
        }

        private void SetupShamanHealth()
        {
            _ShamanMaxHealth = shamanHealthObject.FindProperty("maxHealth");
            _ShamanHealthBar = shamanHealthObject.FindProperty("healthBar");
            _ShamanOnDeath = shamanHealthObject.FindProperty("OnDeath");
            _ShamanTakeAnyDamage = shamanHealthObject.FindProperty("onTakeAnyDamage");
        }

        private void SetupShamanShooting()
        {
            _ShamanBoomerangSpeed = shamanObject.FindProperty("boomerangSpeed");
            _ShamanDamage = shamanObject.FindProperty("boomerangDamage");
            _ShamanHitsBeforeTeleport = shamanObject.FindProperty("maxHitsBeforeTeleport");
            _ShamanTeleportOffset = shamanObject.FindProperty("teleportOffset");
            _ShamanMinRandom = shamanObject.FindProperty("boomerangMinRandomFactor");
            _ShamanMaxRandom = shamanObject.FindProperty("boomerangMaxRandomFactor");
            _ShamanProjectilePrefab = shamanObject.FindProperty("boomerangToSpawn");
        }
        #endregion
        #endregion

        #region Player Functions
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
        #endregion

        private void OnGUI()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, true, true, GUILayout.Width(position.width), GUILayout.Height(position.height));

            #region PlayerUI
            showPlayerSettings = EditorGUILayout.Foldout(showPlayerSettings, "Player Settings");

            if (showPlayerSettings)
            {
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

                GUILayout.Space(50f);
            }
            #endregion

            #region Leech UI
            showLeechSettings = EditorGUILayout.Foldout(showLeechSettings, "Leech Settings");

            if (showLeechSettings)
            {
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
            }

            #endregion

            #region Leech Father UI
            showLeechFatherSettings = EditorGUILayout.Foldout(showLeechFatherSettings, "Leech Father Settings");

            if (showLeechFatherSettings)
            {
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
            }
            #endregion

            #region Leech Mother UI
            showLeechMotherSettings = EditorGUILayout.Foldout(showLeechMotherSettings, "Leech Mother Settings");

            if (showLeechMotherSettings)
            {
                // fetch current values from the target
                leechMotherHealthObject.Update();

                // fetch current values from the target
                leechMotherMovementObject.Update();

                // fetch current values from the target
                leechMotherObject.Update();

                if (leechMotherHealthEditor)
                {
                    leechMotherHealthEditor.OnInspectorGUI();
                }

                if (leechMotherMovmentEditor)
                {
                    leechMotherMovmentEditor.OnInspectorGUI();
                }

                if (leechMotherEditor)
                {
                    GUILayout.Space(5f);

                    leechMotherEditor.OnInspectorGUI();
                }

                // Apply values to the target
                leechMotherHealthObject.ApplyModifiedProperties();

                // Apply values to the target
                leechMotherMovementObject.ApplyModifiedProperties();

                // Apply values to the target
                leechMotherObject.ApplyModifiedProperties();

                GUILayout.Space(50f);
            }
            #endregion

            #region Shaman UI
            showShamanSettings = EditorGUILayout.Foldout(showShamanSettings, "Shaman Settings");

            if (showShamanSettings)
            {
                // fetch current values from the target
                shamanObject.Update();

                // fetch current values from the target
                shamanHealthObject.Update();

                if (shamanHealthEditor)
                {
                    shamanHealthEditor.OnInspectorGUI();
                }

                if (shamanEditor)
                {
                    shamanEditor.OnInspectorGUI();
                }

                // Apply values to the target
                shamanObject.ApplyModifiedProperties();

                // Apply values to the target
                shamanHealthObject.ApplyModifiedProperties();
            }
            #endregion

            GUILayout.EndScrollView();
        }
    }
}