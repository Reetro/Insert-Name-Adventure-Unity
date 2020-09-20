#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using PlayerCharacter.Controller;
using EnemyCharacter.AI;
using GameplayManagement;
using AuraSystem;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

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

        #region Player Scale Variables
        private SerializedProperty _PlayerTransformScale;
        private SerializedObject playerScaleObject;
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

        #region Player Leg Variables
        private SerializedProperty _PlayerLegIsGround;
        private SerializedProperty _PlayerLegBoxSize;
        private SerializedProperty _PlayerLegCollisionTransform;
        #endregion

        #region Player Objects
        private SerializedObject playerMovementObject;
        private SerializedObject playerHealthObject;
        private SerializedObject playerSpearObject;
        private SerializedObject playerLegObject;
        #endregion

        #region Player Editors
        private Editor playerMovementEditor = null;
        private Editor playerHealthEditor = null;
        private Editor playerSpearEditor = null;
        private Editor playerLegEditor = null;
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

        #region Leech Scale Variables
        private SerializedProperty _LeechScale;
        private SerializedProperty _LeechFatherScale;
        private SerializedProperty _LeechMotherScale;

        private SerializedObject leechScaleObject;
        private SerializedObject leechMotherScaleObject;
        private SerializedObject leechFatherScaleObject;
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

        #region Axe Thrower Scale Variables
        private SerializedProperty _ShamanScale;
        private SerializedObject shamanScaleObject;
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

        #region Axe Thrower Scale Variables
        private SerializedProperty _AxeThrowerScale;
        private SerializedObject axeThrowerScaleObject;
        #endregion

        #region Axe Thrower Editors
        private Editor axeThrowerHealthEditor = null;
        private Editor axeThrowerEditor = null;
        #endregion

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
        private SerializedObject slugHealthObject;
        #endregion

        #region Slug Editors
        private Editor slugMovementEditor = null;
        private Editor slugHealthEditor = null;
        #endregion

        #region Worm Variables
        private SerializedProperty _WormDamage;
        private SerializedProperty _WormSegmentHealth;
        private SerializedProperty _WormPushDelay;
        private SerializedProperty _WormPushUpSpeed;
        private SerializedProperty _WormWhatIsGround;
        private SerializedProperty _WormDamageCooldown;
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

        #region Worm Scale Variables
        private SerializedProperty _WormScale;
        private SerializedObject wormScaleObject;
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
        private const float foldoutSpaceing = 10f;
        int tabs = 0;

        #region Foldout Bools
        private static bool showLeechSettings = false;
        private static bool showLeechFatherSettings = false;
        private static bool showLeechMotherSettings = false;
        private static bool showAxeThrowerSettings = false;
        private static bool showShamanSettings = false;
        private static bool showSlugSettings = false;
        private static bool showWormSettings = false;
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

            SetupEnemyScale();

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
                leechScaleObject = new SerializedObject(leechPrefab.transform.GetComponent<Transform>());
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
                leechFatherScaleObject = new SerializedObject(leechFatherPrefab.transform.GetComponent<Transform>());
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
                leechMotherScaleObject = new SerializedObject(leechMotherPrefab.transform.GetComponent<Transform>());
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
                shamanScaleObject = new SerializedObject(shamanPrefab.GetComponent<Transform>());
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
                axeThrowerScaleObject = new SerializedObject(axeThrowerPrefab.GetComponent<Transform>());
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
                slugScaleObject = new SerializedObject(slugPrefab.GetComponent<Transform>());
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

                wormScaleObject = new SerializedObject(wormPrefab.GetComponent<Transform>());
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
            _WormDamageCooldown = wormObject.FindProperty("damageCooldown");
        }
        #endregion

        private void SetupEnemyScale()
        {
            // Set leech scale vars
            _LeechScale = leechScaleObject.FindProperty("m_LocalScale");
            _LeechFatherScale = leechFatherScaleObject.FindProperty("m_LocalScale");
            _LeechMotherScale = leechMotherScaleObject.FindProperty("m_LocalScale");
            // Set Shaman scale var
            _ShamanScale = shamanScaleObject.FindProperty("m_LocalScale");
            // Set Axe Thrower scale var
            _AxeThrowerScale = axeThrowerScaleObject.FindProperty("m_LocalScale");
            // Set slug scale var
            _SlugScale = slugScaleObject.FindProperty("m_LocalScale");
            // Set worm scale var
            _WormScale = wormScaleObject.FindProperty("m_LocalScale");
        }
        #endregion

        #region Player Functions
        private void SetupPlayer()
        {
            SetupPlayerEditor();

            SetPlayerMovement();

            SetPlayerHealth();

            SetPlayerSpear();

            SetPlayerLegs();

            SetPlayerScale();
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
            _PlayerSpearGround = playerSpearObject.FindProperty("BlockLayers");
            _PlayerSpearTravelDistance = playerSpearObject.FindProperty("spearTravelDistance");
        }

        private void SetPlayerLegs()
        {
            _PlayerLegIsGround = playerLegObject.FindProperty("whatIsGround");
            _PlayerLegCollisionTransform = playerLegObject.FindProperty("collisionTransform");
            _PlayerLegBoxSize = playerLegObject.FindProperty("boxSize");
        }

        private void SetPlayerScale()
        {
            _PlayerTransformScale = playerScaleObject.FindProperty("m_LocalScale");
        }

        private void SetupPlayerEditor()
        {
            var playerPrefab = Resources.Load("Player/Player") as GameObject;

            if (playerPrefab)
            {
                playerMovementEditor = Editor.CreateEditor(playerPrefab.GetComponent<PlayerMovement>());
                playerHealthEditor = Editor.CreateEditor(playerPrefab.GetComponent<HealthComponent>());
                playerSpearEditor = Editor.CreateEditor(playerPrefab.GetComponentInChildren<PlayerSpear>());
                playerLegEditor = Editor.CreateEditor(playerPrefab.transform.GetChild(0).GetComponent<PlayerLegs>());

                playerMovementObject = new SerializedObject(playerPrefab.GetComponent<PlayerMovement>());
                playerHealthObject = new SerializedObject(playerPrefab.GetComponent<HealthComponent>());
                playerSpearObject = new SerializedObject(playerPrefab.GetComponentInChildren<PlayerSpear>());
                playerLegObject = new SerializedObject(playerPrefab.transform.GetChild(0).GetComponent<PlayerLegs>());
                playerScaleObject = new SerializedObject(playerPrefab.transform.GetComponent<Transform>());
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

        public void OnGUI()
        {
            tabs = GUILayout.Toolbar(tabs, new string[] { "Player", "Enemies", "Gameplay Management", "Buff / Debuffs" });

            switch (tabs)
            {
                case 0:
                    scrollPosition = GUILayout.BeginScrollView(scrollPosition, true, true, GUILayout.Width(position.width), GUILayout.Height(position.height));

                    SetupPlayerUI();

                    GUILayout.EndScrollView();
                    break;

                case 1:
                    scrollPosition = GUILayout.BeginScrollView(scrollPosition, true, true, GUILayout.Width(position.width), GUILayout.Height(position.height));

                    SetupEnemyUI();

                    GUILayout.EndScrollView();
                    break;

                case 2:
                    scrollPosition = GUILayout.BeginScrollView(scrollPosition, true, true, GUILayout.Width(position.width), GUILayout.Height(position.height));

                    SetupGameplayManagerUI();

                    GUILayout.EndScrollView();
                    break;


                case 3:
                    scrollPosition = GUILayout.BeginScrollView(scrollPosition, true, true, GUILayout.Width(position.width), GUILayout.Height(position.height));

                    SetupBuffAndDebuffsUI();

                    GUILayout.EndScrollView();
                    break;
            }
        }

        #region Gameplay Manager UI
        private void SetupGameplayManagerUI()
        {
            // fetch current values from the target
            gameplayManagerObject.Update();

            if (gameplayManagerEditor)
            {
                // Start a code block to check for GUI changes
                EditorGUI.BeginChangeCheck();

                gameplayManagerEditor.OnInspectorGUI();

                if (EditorGUI.EndChangeCheck())
                {
                    // Apply values to the target
                    gameplayManagerObject.ApplyModifiedProperties();

                    // fetch current values from the target
                    gameplayManagerObject.Update();

                    // Save Current scene after update
                    EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
                }
            }

            // Apply values to the target
            gameplayManagerObject.ApplyModifiedProperties();

            GUILayout.Space(foldoutSpaceing);
        }
        #endregion

        #region PlayerUI
        private void SetupPlayerUI()
        {
            // fetch current values from the target
            playerScaleObject.Update();

            // Start a code block to check for GUI changes
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(_PlayerTransformScale, new GUIContent("Player Scale"));

            if (EditorGUI.EndChangeCheck())
            {
                // Apply values to the target
                playerScaleObject.ApplyModifiedProperties();

                // Save Current scene after update
                EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
            }

            // fetch current values from the target
            playerMovementObject.Update();

            if (playerMovementEditor)
            {
                // Start a code block to check for GUI changes
                EditorGUI.BeginChangeCheck();

                playerMovementEditor.OnInspectorGUI();

                if (EditorGUI.EndChangeCheck())
                {
                    // Apply values to the target
                    playerMovementObject.ApplyModifiedProperties();

                    // fetch current values from the target
                    playerMovementObject.Update();

                    // Save Current scene after update
                    EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
                }

                GUILayout.Space(foldoutSpaceing);
            }

            // fetch current values from the target
            playerHealthObject.Update();

            if (playerHealthEditor)
            {
                // Start a code block to check for GUI changes
                EditorGUI.BeginChangeCheck();

                playerHealthEditor.OnInspectorGUI();

                if (EditorGUI.EndChangeCheck())
                {
                    // Apply values to the target
                    playerHealthObject.ApplyModifiedProperties();

                    // fetch current values from the target
                    playerHealthObject.Update();

                    // Save Current scene after update
                    EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
                }


                GUILayout.Space(foldoutSpaceing);
            }

            // fetch current values from the target
            playerSpearObject.Update();

            if (playerSpearEditor)
            {
                // Start a code block to check for GUI changes
                EditorGUI.BeginChangeCheck();

                playerSpearEditor.OnInspectorGUI();

                if (EditorGUI.EndChangeCheck())
                {
                    // Apply values to the target
                    playerSpearObject.ApplyModifiedProperties();

                    // fetch current values from the target
                    playerSpearObject.Update();

                    // Save Current scene after update
                    EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
                }

                GUILayout.Space(foldoutSpaceing);
            }

            // fetch current values from the target
            playerLegObject.Update();

            if (playerLegEditor)
            {
                // Start a code block to check for GUI changes
                EditorGUI.BeginChangeCheck();

                playerLegEditor.OnInspectorGUI();

                if (EditorGUI.EndChangeCheck())
                {
                    // Apply values to the target
                    playerLegObject.ApplyModifiedProperties();

                    // fetch current values from the target
                    playerLegObject.Update();

                    // Save Current scene after update
                    EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
                }

                GUILayout.Space(foldoutSpaceing);
            }

            // Apply values to the target
            playerMovementObject.ApplyModifiedProperties();

            // Apply values to the target
            playerHealthObject.ApplyModifiedProperties();

            // Apply values to the target
            playerSpearObject.ApplyModifiedProperties();

            // Apply values to the target
            playerLegObject.ApplyModifiedProperties();
        }
        #endregion

        #region Enemy Editors
        private void SetupEnemyUI()
        {
            #region Leech UI
            showLeechSettings = EditorGUILayout.Foldout(showLeechSettings, "Leech Settings", true);

            if (showLeechSettings)
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

                    // Save Current scene after update
                    EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
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

                        // Save Current scene after update
                        EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
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

                        // Save Current scene after update
                        EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
                    }
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
                leechFatherScaleObject.Update();

                // Start a code block to check for GUI changes
                EditorGUI.BeginChangeCheck();

                EditorGUILayout.PropertyField(_LeechFatherScale, new GUIContent("Leech Father Scale"));

                if (EditorGUI.EndChangeCheck())
                {
                    // Apply values to the target
                    leechFatherScaleObject.ApplyModifiedProperties();

                    // Save Current scene after update
                    EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
                }

                // fetch current values from the target
                leechFatherHealthObject.Update();

                // fetch current values from the target
                leechFatherMovementObject.Update();

                // fetch current values from the target
                leechFatherObject.Update();

                if (leechFatherHealthEditor)
                {
                    // fetch current values from the target
                    leechFatherHealthObject.Update();

                    // Start a code block to check for GUI changes
                    EditorGUI.BeginChangeCheck();

                    leechFatherHealthEditor.OnInspectorGUI();

                    if (EditorGUI.EndChangeCheck())
                    {
                        // Apply values to the target
                        leechFatherHealthObject.ApplyModifiedProperties();

                        // fetch current values from the target
                        leechFatherHealthObject.Update();

                        // Save Current scene after update
                        EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
                    }
                }

                if (leechFatherMovmentEditor)
                {
                    // fetch current values from the target
                    leechFatherMovementObject.Update();

                    // Start a code block to check for GUI changes
                    EditorGUI.BeginChangeCheck();

                    leechFatherMovmentEditor.OnInspectorGUI();

                    if (EditorGUI.EndChangeCheck())
                    {
                        // Apply values to the target
                        leechFatherMovementObject.ApplyModifiedProperties();

                        // fetch current values from the target
                        leechFatherMovementObject.Update();

                        // Save Current scene after update
                        EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
                    }
                }

                if (leechFatherEditor)
                {
                    GUILayout.Space(5f);

                    // fetch current values from the target
                    leechFatherObject.Update();

                    // Start a code block to check for GUI changes
                    EditorGUI.BeginChangeCheck();

                    leechFatherEditor.OnInspectorGUI();

                    if (EditorGUI.EndChangeCheck())
                    {
                        // Apply values to the target
                        leechFatherObject.ApplyModifiedProperties();

                        // fetch current values from the target
                        leechFatherObject.Update();

                        // Save Current scene after update
                        EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
                    }
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
                leechMotherScaleObject.Update();

                // Start a code block to check for GUI changes
                EditorGUI.BeginChangeCheck();

                EditorGUILayout.PropertyField(_LeechMotherScale, new GUIContent("Leech Father Scale"));

                if (EditorGUI.EndChangeCheck())
                {
                    // Apply values to the target
                    leechMotherScaleObject.ApplyModifiedProperties();

                    // Save Current scene after update
                    EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
                }

                // fetch current values from the target
                leechMotherHealthObject.Update();

                // fetch current values from the target
                leechMotherMovementObject.Update();

                // fetch current values from the target
                leechMotherObject.Update();

                if (leechMotherHealthEditor)
                {
                    // Start a code block to check for GUI changes
                    EditorGUI.BeginChangeCheck();

                    leechMotherHealthEditor.OnInspectorGUI();

                    if (EditorGUI.EndChangeCheck())
                    {
                        // Apply values to the target
                        leechMotherHealthObject.ApplyModifiedProperties();

                        // fetch current values from the target
                        leechMotherHealthObject.Update();

                        // Save Current scene after update
                        EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
                    }
                }

                if (leechMotherMovmentEditor)
                {
                    // Start a code block to check for GUI changes
                    EditorGUI.BeginChangeCheck();

                    leechMotherMovmentEditor.OnInspectorGUI();

                    if (EditorGUI.EndChangeCheck())
                    {
                        // Apply values to the target
                        leechMotherMovementObject.ApplyModifiedProperties();

                        // fetch current values from the target
                        leechMotherMovementObject.Update();

                        // Save Current scene after update
                        EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
                    }
                }

                if (leechMotherEditor)
                {
                    GUILayout.Space(5f);

                    // Start a code block to check for GUI changes
                    EditorGUI.BeginChangeCheck();

                    leechMotherEditor.OnInspectorGUI();

                    if (EditorGUI.EndChangeCheck())
                    {
                        // Apply values to the target
                        leechMotherObject.ApplyModifiedProperties();

                        // fetch current values from the target
                        leechMotherObject.Update();

                        // Save Current scene after update
                        EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
                    }
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
                shamanScaleObject.Update();

                // Start a code block to check for GUI changes
                EditorGUI.BeginChangeCheck();

                EditorGUILayout.PropertyField(_ShamanScale, new GUIContent("Shaman Scale"));

                if (EditorGUI.EndChangeCheck())
                {
                    // Apply values to the target
                    shamanScaleObject.ApplyModifiedProperties();

                    // Save Current scene after update
                    EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
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

                        // Save Current scene after update
                        EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
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

                        // Save Current scene after update
                        EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
                    }
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
                axeThrowerScaleObject.Update();

                // Start a code block to check for GUI changes
                EditorGUI.BeginChangeCheck();

                EditorGUILayout.PropertyField(_AxeThrowerScale, new GUIContent("Axe Thrower Scale"));

                if (EditorGUI.EndChangeCheck())
                {
                    // Apply values to the target
                    axeThrowerScaleObject.ApplyModifiedProperties();

                    // Save Current scene after update
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

                        // Save Current scene after update
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

                        // Save Current scene after update
                        EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
                    }
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
                slugScaleObject.Update();

                // Start a code block to check for GUI changes
                EditorGUI.BeginChangeCheck();

                EditorGUILayout.PropertyField(_SlugScale, new GUIContent("Slug Scale"));

                if (EditorGUI.EndChangeCheck())
                {
                    // Apply values to the target
                    slugScaleObject.ApplyModifiedProperties();

                    // Save Current scene after update
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

                        // Save Current scene after update
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

                        // Save Current scene after update
                        EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
                    }
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
                wormScaleObject.Update();

                // Start a code block to check for GUI changes
                EditorGUI.BeginChangeCheck();

                EditorGUILayout.PropertyField(_WormScale, new GUIContent("Worm Scale"));

                if (EditorGUI.EndChangeCheck())
                {
                    // Apply values to the target
                    wormScaleObject.ApplyModifiedProperties();

                    // Save Current scene after update
                    EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
                }

                // fetch current values from the target
                wormObject.Update();

                if (wormEdior)
                {
                    // Start a code block to check for GUI changes
                    EditorGUI.BeginChangeCheck();

                    wormEdior.OnInspectorGUI();

                    if (EditorGUI.EndChangeCheck())
                    {
                        // Apply values to the target
                        wormObject.ApplyModifiedProperties();

                        // fetch current values from the target
                        wormObject.Update();

                        // Save Current scene after update
                        EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
                    }

                    GUILayout.Space(foldoutSpaceing);
                }

                // Apply values to the target
                wormObject.ApplyModifiedProperties();
            }
            #endregion
        }
        #endregion

        #region Buff / Debuff Editors
        private void SetupBuffAndDebuffsUI()
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

    }
}
#endif