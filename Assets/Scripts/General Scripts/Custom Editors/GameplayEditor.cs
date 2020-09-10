#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using PlayerCharacter.Controller;
using EnemyCharacter.AI;
using GameplayManagement;
using AuraSystem;

namespace CustomEditors
{
    public class GameplayEditor : EditorWindow
    {
        #region Gameplay Manager Variables
        private SerializedProperty _ManagerTextSpeed;
        private SerializedProperty _ManagerTextUpTime;
        private SerializedProperty _ManagerRandomMinX;
        private SerializedProperty _ManagerRandomMinY;
        private SerializedProperty _ManagerRandomMaxX;
        private SerializedProperty _ManagerRandomMaxY;
        private SerializedProperty _ManagerTextDisappearTime;
        private SerializedProperty _ManagerNameFontSize;
        private SerializedProperty _ManagerDescriptionFontSize;
        private SerializedProperty _ManagerWhatCanBeDamaged;
        private SerializedProperty _ManagerDefaultControllerCheckTimer;
        private SerializedProperty _ManagerDebugSave;
        #endregion

        #region Gameplay Manager Editors
        private Editor gameplayManagerEditor;
        #endregion

        #region Gameplay Manager Objects
        private SerializedObject gameplayManagerObject;
        #endregion

        #region Player Movement Variables
        private SerializedProperty _PlayerJumpForce;
        private SerializedProperty _PlayerRunSpeed;
        private SerializedProperty _MovementSmothing;
        private SerializedProperty _HasAirControl;
        private SerializedProperty _PlayerAccleration;
        #endregion

        #region Player Spear Variables
        private SerializedProperty _PlayerSpearDumage;
        private SerializedProperty _PlayerSpearUpTime;
        private SerializedProperty _PlayerSpearTravelDistance;
        private SerializedProperty _PlayerSpearCooldown;
        private SerializedProperty _PlayerHealthComp;
        private SerializedProperty _PlayerSpearGround;
        #endregion

        #region Player Health Variables
        private SerializedProperty _PlayerMaxHealth;
        #endregion

        #region Player Objects
        private SerializedObject playerMovementObject;
        private SerializedObject playerHealthObject;
        private SerializedObject playerSpearObject;
        #endregion

        #region Player Editors
        private Editor playerMovementEditor = null;
        private Editor playerHealthEditor = null;
        private Editor playerGunEditor = null;
        #endregion

        #region Enemy Variables

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
        #endregion

        #region Leech Editors
        private Editor leechMovmentEditor = null;
        private Editor leechHealthEditor = null;
        #endregion

        #region Leech Father Health Variables
        private SerializedProperty _LeechFatherMaxHealth;
        private SerializedProperty _LeechFatherHealthBar;
        #endregion

        #region Leech Father Movement Variables
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

        #region Leech Father Shooting Variables
        private SerializedProperty _LeechFatherShootIntervale;
        private SerializedProperty _LeechFatherProjectileDamage;
        private SerializedProperty _LeechFatherProjectileSpeed;
        private SerializedProperty _LeechFatherProjectileToSpawn;
        #endregion

        #region Leech Mother Health Variables
        private SerializedProperty _LeechMotherMaxHealth;
        private SerializedProperty _LeechMotherHealthBar;
        #endregion

        #region Leech Mother Movement Variables
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

        #region Leech Mother Shooting Variables
        private SerializedProperty _LeechMotherShootIntervale;
        private SerializedProperty _LeechMotherProjectileDamage;
        private SerializedProperty _LeechMotherProjectileSpeed;
        private SerializedProperty _LeechMotherProjectileToSpawn;
        #endregion

        #region Shaman Objects
        private SerializedObject shamanObject;
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
        #endregion

        #region Shaman Editors
        private Editor shamanHealthEditor = null;
        private Editor shamanEditor = null;
        #endregion

        #region Axe Thrower Objects
        private SerializedObject axeThrowerObject;
        private SerializedObject axeThrowerHealthObject;
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

        #region Axe Thrower Editors
        private Editor axeThrowerHealthEditor = null;
        private Editor axeThrowerEditor = null;
        #endregion

        #region Slug Health Variables
        private SerializedProperty __SlugMaxHealth;
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
        private SerializedObject slugHealthObject;
        #endregion

        #region Axe Thrower Editors
        private Editor slugMovementEditor = null;
        private Editor slugHealthEditor = null;
        #endregion

        #region Worm Variables
        private SerializedProperty _WormDamage;
        private SerializedProperty _WormSegmentHealth;
        private SerializedProperty _WormPushDelay;
        private SerializedProperty _WormPushUpSpeed;
        private SerializedProperty _WormWhatIsGround;
        #endregion

        #region Worm Rotation Variables
        private SerializedProperty _WormRotationSpeed;
        private SerializedProperty _WormRotationOffset;
        private SerializedProperty _WormReturnHomeDelay;
        private SerializedProperty _WormRotationDelay;
        private SerializedProperty _WormRotationTargetLeft;
        private SerializedProperty _WormRotationTargetRight;
        private SerializedProperty _WormDrawDebug;
        #endregion

        #region Worm Squish Variables
        private SerializedProperty _WormSquishScale;
        private SerializedProperty _WormDebuff;
        #endregion

        #region Worm Objects
        private SerializedObject wormObject;
        #endregion

        #region Worm Editors
        private Editor wormEdior = null;
        #endregion

        #endregion

        #region Buffs / Debuffs Variables
        private Editor LeechingEditor = null;
        private Editor BreatherEditor = null;
        private Editor PlayerSlowingEditor = null;
        #endregion

        #region Local Variables
        private Vector2 scrollPosition = Vector2.zero;
        private const float foldoutSpaceing = 20f;

        #region Foldout Bools
        private static bool showEnemyEditor = false;
        private static bool showPlayerSettings = false;
        private static bool showLeechSettings = false;
        private static bool showLeechFatherSettings = false;
        private static bool showLeechMotherSettings = false;
        private static bool showAxeThrowerSettings = false;
        private static bool showShamanSettings = false;
        private static bool showManagerSettings = false;
        private static bool showSlugSettings = false;
        private static bool showWormSettings = false;
        private static bool showBuffDebuffSettings = false;
        private static bool showBuffSettings = false;
        private static bool showDebuffSettings = false;
        private static bool showBreatherSettings = false;
        private static bool showLeechingSettings = false;
        private static bool showSlowingSettings = false;
        #endregion
        #endregion

        [MenuItem("Window/Gameplay Editor")]
        public static void ShowWindow()
        {
            GetWindow<GameplayEditor>("Gameplay Editor");
        }

        private void OnEnable()
        {
            SetupGameplayManager();

            SetupPlayer();

            SetupEnemies();

            SetupDebuffs();

            SetupBuffs();
        }

        #region Gameplay Manager Functions
        private void SetupGameplayManager()
        {
            SetupGameplayManagerEditor();

            SetGameplayManagerVars();
        }
        private void SetupGameplayManagerEditor()
        {
            var managerPrefab = Resources.Load("Player/Player") as GameObject;

            if (managerPrefab)
            {
                gameplayManagerEditor = Editor.CreateEditor(managerPrefab.GetComponentInChildren<GameplayManager>());

                gameplayManagerObject = new SerializedObject(managerPrefab.GetComponentInChildren<GameplayManager>());
            }
            else
            {
                Debug.LogError("Failed to get managerPrefab in Gameplay Editor");
            }
        }
        private void SetGameplayManagerVars()
        {
            _ManagerDescriptionFontSize = gameplayManagerObject.FindProperty("descriptionFontSize");
            _ManagerNameFontSize = gameplayManagerObject.FindProperty("nameFontSize");
            _ManagerWhatCanBeDamaged = gameplayManagerObject.FindProperty("whatCanBeDamaged");
            _ManagerTextSpeed = gameplayManagerObject.FindProperty("combatTextSpeed");
            _ManagerTextUpTime = gameplayManagerObject.FindProperty("combatTextUpTime");
            _ManagerRandomMinX = gameplayManagerObject.FindProperty("combatRandomVectorMinX");
            _ManagerRandomMaxX = gameplayManagerObject.FindProperty("combatRandomVectorMaxX");
            _ManagerRandomMinY = gameplayManagerObject.FindProperty("combatRandomVectorMinY");
            _ManagerRandomMaxY = gameplayManagerObject.FindProperty("combatRandomVectorMaxY");
            _ManagerTextDisappearTime = gameplayManagerObject.FindProperty("disappearTime");
            _ManagerDefaultControllerCheckTimer = gameplayManagerObject.FindProperty("defaultControllerCheckTimer");
            _ManagerDebugSave = gameplayManagerObject.FindProperty("debugSave");
        }
        #endregion

        #region Enemy Functions
        private void SetupEnemies()
        {
            SetupLeechEditor();

            SetLeechHealth();

            SetLeechMovement();

            SetupLeechFatherEditor();

            SetupLeechFatherShooting();

            SetupLeechFatherHealth();

            SetupLeechFatherMovement();

            SetupLeechMotherEditor();

            SetupLeechMotherHealth();

            SetupLeechMotherMovement();

            SetupLeechMotherShooting();

            SetupShamanEditor();

            SetupShamanHealth();

            SetupShamanShooting();

            SetupAxeThrowerEditor();

            SetupAxeThrowerHealth();

            SetupAxeThrowerShooting();

            SetupSlugEditor();

            SetupSlugHealthEditor();

            SetupSlugMovementEditor();

            SetupWormEditor();

            SetupWormObjectEditor();
        }

        #region Leech Functions
        private void SetupLeechEditor()
        {
            var leechPrefab = Resources.Load("Enemies/Leech/Leech") as GameObject;

            if (leechPrefab)
            {
                leechMovmentEditor = Editor.CreateEditor(leechPrefab.GetComponent<LeechMovement>());
                leechHealthEditor = Editor.CreateEditor(leechPrefab.GetComponent<HealthComponent>());

                leechHealthObject = new SerializedObject(leechPrefab.GetComponent<HealthComponent>());
                leechMovementObject = new SerializedObject(leechPrefab.GetComponent<LeechMovement>());
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
        }
        #endregion

        #region Leech Father Functions
        private void SetupLeechFatherEditor()
        {
            var leechFatherPrefab = Resources.Load("Enemies/Leech/Leech Father") as GameObject;

            if (leechFatherPrefab)
            {
                leechFatherMovmentEditor = Editor.CreateEditor(leechFatherPrefab.GetComponent<LeechMovement>());
                leechFatherHealthEditor = Editor.CreateEditor(leechFatherPrefab.GetComponent<HealthComponent>());
                leechFatherEditor = Editor.CreateEditor(leechFatherPrefab.GetComponent<LeechFather>());

                leechFatherHealthObject = new SerializedObject(leechFatherPrefab.GetComponent<HealthComponent>());
                leechFatherMovementObject = new SerializedObject(leechFatherPrefab.GetComponent<LeechMovement>());
                leechFatherObject = new SerializedObject(leechFatherPrefab.GetComponent<LeechFather>());
            }
            else
            {
                Debug.LogError("Failed to get leechFatherPrefab in Gameplay Editor");
            }
        }

        private void SetupLeechFatherShooting()
        {
            _LeechFatherShootIntervale = leechFatherObject.FindProperty("shootIntervale");
            _LeechFatherProjectileSpeed = leechFatherObject.FindProperty("projectileSpeed");
            _LeechFatherProjectileDamage = leechFatherObject.FindProperty("projectileDamage");
            _LeechFatherProjectileToSpawn = leechFatherObject.FindProperty("projectileToSpawn");
        }

        private void SetupLeechFatherHealth()
        {
            _LeechFatherMaxHealth = leechFatherHealthObject.FindProperty("maxHealth");
            _LeechFatherHealthBar = leechFatherHealthObject.FindProperty("healthBar");
        }

        private void SetupLeechFatherMovement()
        {
            _LeechFatherFlySpeed = leechFatherMovementObject.FindProperty("leechFlySpeed");
            _LeechFatherRandomYmin = leechFatherMovementObject.FindProperty("randomYMin");
            _LeechFatherRandomYmax = leechFatherMovementObject.FindProperty("randomYMax");
        }
        #endregion

        #region Leech Mother Functions
        private void SetupLeechMotherEditor()
        {
            var leechMotherPrefab = Resources.Load("Enemies/Leech/Leech Mother") as GameObject;

            if (leechMotherPrefab)
            {
                leechMotherMovmentEditor = Editor.CreateEditor(leechMotherPrefab.GetComponent<LeechMovement>());
                leechMotherHealthEditor = Editor.CreateEditor(leechMotherPrefab.GetComponent<HealthComponent>());
                leechMotherEditor = Editor.CreateEditor(leechMotherPrefab.GetComponent<LeechMother>());

                leechMotherHealthObject = new SerializedObject(leechMotherPrefab.GetComponent<HealthComponent>());
                leechMotherMovementObject = new SerializedObject(leechMotherPrefab.GetComponent<LeechMovement>());
                
                leechMotherObject = new SerializedObject(leechMotherPrefab.GetComponent<LeechMother>());
            }
            else
            {
                Debug.LogError("Failed to get leechMotherPrefab in Gameplay Editor");
            }
        }

        private void SetupLeechMotherShooting()
        {
            _LeechMotherShootIntervale = leechMotherObject.FindProperty("shootIntervale");
            _LeechMotherProjectileSpeed = leechMotherObject.FindProperty("projectileSpeed");
            _LeechMotherProjectileDamage = leechMotherObject.FindProperty("projectileDamage");
            _LeechMotherProjectileToSpawn = leechMotherObject.FindProperty("projectileToSpawn");
        }

        private void SetupLeechMotherHealth()
        {
            _LeechMotherMaxHealth = leechMotherHealthObject.FindProperty("maxHealth");
            _LeechMotherHealthBar = leechMotherHealthObject.FindProperty("healthBar");
        }

        private void SetupLeechMotherMovement()
        {
            _LeechMotherFlySpeed = leechMotherMovementObject.FindProperty("leechFlySpeed");
            _LeechMotherRandomYmin = leechMotherMovementObject.FindProperty("randomYMin");
            _LeechMotherRandomYmax = leechMotherMovementObject.FindProperty("randomYMax");
        }
        #endregion

        #region Shaman Functions
        private void SetupShamanEditor()
        {
            var shamanPrefab = Resources.Load("Enemies/Shaman/Shaman") as GameObject;

            if (shamanPrefab)
            {
                shamanEditor = Editor.CreateEditor(shamanPrefab.GetComponent<Shaman>());
                shamanHealthEditor = Editor.CreateEditor(shamanPrefab.GetComponent<HealthComponent>());

                shamanHealthObject = new SerializedObject(shamanPrefab.GetComponent<HealthComponent>());
                shamanObject = new SerializedObject(shamanPrefab.GetComponent<Shaman>());
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
        }
        #endregion

        #region Axe Thrower Functions
        private void SetupAxeThrowerEditor()
        {
            var axeThrowerPrefab = Resources.Load("Enemies/Axe Thrower/Axe Thrower") as GameObject;

            if (axeThrowerPrefab)
            {
                axeThrowerHealthEditor = Editor.CreateEditor(axeThrowerPrefab.GetComponent<HealthComponent>());
                axeThrowerEditor = Editor.CreateEditor(axeThrowerPrefab.GetComponent<AxeThrower>());

                axeThrowerObject = new SerializedObject(axeThrowerPrefab.GetComponent<AxeThrower>());
                axeThrowerHealthObject = new SerializedObject(axeThrowerPrefab.GetComponent<HealthComponent>());
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
        }
        #endregion

        #region Slug Functions
        private void SetupSlugEditor()
        {
            var slugPrefab = Resources.Load("Enemies/Slug/Slug") as GameObject;

            if (slugPrefab)
            {
                slugMovementEditor = Editor.CreateEditor(slugPrefab.GetComponent<SlugMovement>());
                slugHealthEditor = Editor.CreateEditor(slugPrefab.GetComponent<HealthComponent>());

                slugHealthObject = new SerializedObject(slugPrefab.GetComponent<HealthComponent>());
                slugMovementObject = new SerializedObject(slugPrefab.GetComponent<SlugMovement>());
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
        }
        #endregion

        #region Worm Functions
        private void SetupWormEditor()
        {
            var wormPrefab = Resources.Load("Enemies/Worm/Worm Object") as GameObject;

            if (wormPrefab)
            {
                wormEdior = Editor.CreateEditor(wormPrefab.GetComponent<WormMovement>());

                wormObject = new SerializedObject(wormPrefab.GetComponent<WormMovement>());
            }
            else
            {
                Debug.LogError("Failed to get wormPrefab in Gameplay Editor");
            }
        }
        private void SetupWormObjectEditor()
        {
            _WormDamage = wormObject.FindProperty("damage");
            _WormSegmentHealth = wormObject.FindProperty("segmentHealth");
            _WormPushDelay = wormObject.FindProperty("pushDelay");
            _WormPushUpSpeed = wormObject.FindProperty("pushUpSpeed");
            _WormWhatIsGround = wormObject.FindProperty("whatIsGround");
            _WormRotationDelay = wormObject.FindProperty("rotationDelay");
            _WormReturnHomeDelay = wormObject.FindProperty("returnHomeDelay");
            _WormRotationOffset = wormObject.FindProperty("rotationOffset");
            _WormRotationSpeed = wormObject.FindProperty("rotationSpeed");
            _WormRotationTargetLeft = wormObject.FindProperty("targetLeftAngle");
            _WormRotationTargetRight = wormObject.FindProperty("targetRightAngle");
            _WormSquishScale = wormObject.FindProperty("SquishScale");
            _WormDebuff = wormObject.FindProperty("debuffToApply");
        }
        #endregion
        #endregion

        #region Player Functions
        private void SetupPlayer()
        {
            SetupPlayerEditor();

            SetPlayerMovement();

            SetPlayerHealth();

            SetPlayerSpear();
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
        }

        private void SetPlayerSpear()
        {
            _PlayerSpearUpTime = playerSpearObject.FindProperty("spearReturnDelay");
            _PlayerSpearDumage = playerSpearObject.FindProperty("spearDamage");
            _PlayerSpearCooldown = playerSpearObject.FindProperty("spearCooldown");
            _PlayerHealthComp = playerSpearObject.FindProperty("playerHealthComp");
            _PlayerSpearGround = playerSpearObject.FindProperty("whatIsGround");
            _PlayerSpearTravelDistance = playerSpearObject.FindProperty("spearTravelDistance");
        }

        private void SetupPlayerEditor()
        {
            var playerPrefab = Resources.Load("Player/Player") as GameObject;

            if (playerPrefab)
            {
                playerMovementEditor = Editor.CreateEditor(playerPrefab.GetComponent<PlayerMovement>());
                playerHealthEditor = Editor.CreateEditor(playerPrefab.GetComponent<HealthComponent>());
                playerGunEditor = Editor.CreateEditor(playerPrefab.GetComponentInChildren<PlayerSpear>());

                playerMovementObject = new SerializedObject(playerPrefab.GetComponent<PlayerMovement>());
                playerHealthObject = new SerializedObject(playerPrefab.GetComponent<HealthComponent>());
                playerSpearObject = new SerializedObject(playerPrefab.GetComponentInChildren<PlayerSpear>());
            }
            else
            {
                Debug.LogError("Failed to get playerPrefab in Gameplay Editor");
            }
        }
        #endregion

        #region Buffs / Debuff Functions
        private void SetupDebuffs()
        {
            var leechingDebuff = Resources.Load("Aura System/Debuffs/Leeching_D") as ScriptableDebuff;
            var playerSlowingDebuff = Resources.Load("Aura System/Debuffs/PlayerSlowing_D") as ScriptableDebuff;

            if (leechingDebuff)
            {
                LeechingEditor = Editor.CreateEditor(leechingDebuff);
            }
            else
            {
                Debug.LogError("Failed to get leechingDebuff in SetupDebuffs Function in GameplayEditor");
            }

            if (playerSlowingDebuff)
            {
                PlayerSlowingEditor = Editor.CreateEditor(playerSlowingDebuff);
            }
            else
            {
                Debug.LogError("Failed to get slowingDebuff in SetupDebuffs Function in GameplayEditor");
            }
        }
        private void SetupBuffs()
        {
            var breatherBuff = Resources.Load("Aura System/Buffs/Breather_B") as ScriptableBuff;

            if (breatherBuff)
            {
                BreatherEditor = Editor.CreateEditor(breatherBuff);
            }
            else
            {
                Debug.LogError("Failed to get breatherBuff in SetupBuffs Function in GameplayEditor");
            }
        }
        #endregion

        private void OnGUI()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, true, true, GUILayout.Width(position.width), GUILayout.Height(position.height));

            #region Gameplay Manager UI
            showManagerSettings = EditorGUILayout.Foldout(showManagerSettings, "Gameplay Manager", true);

            if (showManagerSettings)
            {
                // fetch current values from the target
                gameplayManagerObject.Update();

                if (gameplayManagerEditor)
                {
                    gameplayManagerEditor.OnInspectorGUI();
                }

                // Apply values to the target
                gameplayManagerObject.ApplyModifiedProperties();

                GUILayout.Space(foldoutSpaceing);
            }
            #endregion

            #region PlayerUI
            showPlayerSettings = EditorGUILayout.Foldout(showPlayerSettings, "Player Settings", true);

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
                playerSpearObject.Update();

                if (playerGunEditor)
                {
                    playerGunEditor.OnInspectorGUI();
                }

                // Apply values to the target
                playerMovementObject.ApplyModifiedProperties();

                // Apply values to the target
                playerHealthObject.ApplyModifiedProperties();

                // Apply values to the target
                playerSpearObject.ApplyModifiedProperties();

                GUILayout.Space(foldoutSpaceing);
            }
            #endregion

            #region Enemy Editors

            showEnemyEditor = EditorGUILayout.Foldout(showEnemyEditor, "Enemy Editor", true);

            if (showEnemyEditor)
            {
                #region Leech UI
                showLeechSettings = EditorGUILayout.Foldout(showLeechSettings, "Leech Settings", true);

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

                    GUILayout.Space(foldoutSpaceing);
                }

                #endregion

                #region Leech Father UI
                showLeechFatherSettings = EditorGUILayout.Foldout(showLeechFatherSettings, "Leech Father Settings", true);

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

                    GUILayout.Space(foldoutSpaceing);
                }
                #endregion

                #region Leech Mother UI
                showLeechMotherSettings = EditorGUILayout.Foldout(showLeechMotherSettings, "Leech Mother Settings", true);

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

                    GUILayout.Space(foldoutSpaceing);
                }
                #endregion

                #region Shaman UI
                showShamanSettings = EditorGUILayout.Foldout(showShamanSettings, "Shaman Settings", true);

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

                    GUILayout.Space(foldoutSpaceing);
                }
                #endregion

                #region Axe Thrower UI
                showAxeThrowerSettings = EditorGUILayout.Foldout(showAxeThrowerSettings, "Axe Thrower Settings", true);

                if (showAxeThrowerSettings)
                {
                    // fetch current values from the target
                    axeThrowerObject.Update();

                    // fetch current values from the target
                    axeThrowerHealthObject.Update();

                    if (axeThrowerHealthEditor)
                    {
                        axeThrowerHealthEditor.OnInspectorGUI();
                    }

                    if (axeThrowerEditor)
                    {
                        axeThrowerEditor.OnInspectorGUI();
                    }

                    // Apply values to the target
                    axeThrowerObject.ApplyModifiedProperties();

                    // Apply values to the target
                    axeThrowerHealthObject.ApplyModifiedProperties();

                    GUILayout.Space(foldoutSpaceing);
                }
                #endregion

                #region Slug UI
                showSlugSettings = EditorGUILayout.Foldout(showSlugSettings, "Slug Settings", true);

                if (showSlugSettings)
                {
                    // fetch current values from the target
                    slugHealthObject.Update();

                    // fetch current values from the target
                    slugMovementObject.Update();

                    if (slugMovementEditor)
                    {
                        slugMovementEditor.OnInspectorGUI();
                    }

                    if (slugHealthEditor)
                    {
                        slugHealthEditor.OnInspectorGUI();
                    }

                    // Apply values to the target
                    slugHealthObject.ApplyModifiedProperties();

                    // Apply values to the target
                    slugMovementObject.ApplyModifiedProperties();

                    GUILayout.Space(foldoutSpaceing);
                }

                #endregion

                #region Worm UI
                showWormSettings = EditorGUILayout.Foldout(showWormSettings, "Worm Settings", true);

                if (showWormSettings)
                {
                    // fetch current values from the target
                    wormObject.Update();

                    if (wormEdior)
                    {
                        wormEdior.OnInspectorGUI();
                    }

                    // Apply values to the target
                    wormObject.ApplyModifiedProperties();
                }
                #endregion
            }

            #endregion

            #region Buff / Debuff Editors

            showBuffDebuffSettings = EditorGUILayout.Foldout(showBuffDebuffSettings, "Buff / Debuff Settings", true);

            if (showBuffDebuffSettings)
            {
                showBuffSettings = EditorGUILayout.Foldout(showBuffSettings, "Buff Settings", true);

                if (showBuffSettings)
                {
                    #region Breather Settings
                    showBreatherSettings = EditorGUILayout.Foldout(showBreatherSettings, "Breather Settings", true);

                    if (showBreatherSettings)
                    {
                        if (BreatherEditor)
                        {
                            BreatherEditor.OnInspectorGUI();
                        }
                    }
                    #endregion
                }

                showDebuffSettings = EditorGUILayout.Foldout(showDebuffSettings, "Debuff Settings", true);

                if (showDebuffSettings)
                {
                    #region Leeching Settings
                    showLeechingSettings = EditorGUILayout.Foldout(showLeechingSettings, "Leeching Settings", true);

                    if (showLeechingSettings)
                    {
                        if (LeechingEditor)
                        {
                            LeechingEditor.OnInspectorGUI();
                        }
                    }
                    #endregion

                    #region Slowing Settings
                    showSlowingSettings = EditorGUILayout.Foldout(showSlowingSettings, "Player Slowing Settings", true);

                    if (showSlowingSettings)
                    {
                        if (PlayerSlowingEditor)
                        {
                            PlayerSlowingEditor.OnInspectorGUI();
                        }
                    }
                    #endregion
                }
            }

            #endregion

            GUILayout.EndScrollView();
        }
    }
}
#endif