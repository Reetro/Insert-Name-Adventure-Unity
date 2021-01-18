﻿#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using PlayerCharacter.Controller;
using EnemyCharacter.AI;
using GameplayManagement;
using StatusEffects;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using ComponentLibrary;

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
        private SerializedProperty _ManagerPlayerStartingSpells;
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
        #endregion

        #region Tiki Head Scale Variables
        private SerializedProperty _TikiHeadScale;
        #endregion

        #region Tiki Head Objects
        private SerializedObject tikiHeadObject;
        private SerializedObject tikiHeadScaleObject;
        private SerializedObject tikiHeadHealthObject;
        #endregion
        #endregion

        #region Status Effects Editors
        private Editor leechingSEEditor = null;
        private Editor playerSlowSEEditor = null;
        private Editor healSEEditor = null;
        #endregion

        #region Spell Editors
        private Editor playerDashEditor = null;
        #endregion

        #region Local Variables
        private Vector2 scrollPosition = Vector2.zero;
        private const float foldoutSpaceing = 10f;
        int tabs = 0;
        private const int indentLevel = 1;
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

            SetupStatusEffects();

            SetupSpells();
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
            _ManagerPlayerStartingSpells = gameplayManagerObject.FindProperty("startingSpells");
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

            SetupTikiHeadEditor();

            SetupTikiHeadHealthEditor();

            SetupTikiHeadMovementEditor();
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

        #region Tiki Head Functions
        private void SetupTikiHeadEditor()
        {
            var tikiHead = Resources.Load("Enemies/Tiki Head/Tiki Head") as GameObject;

            if (tikiHead)
            {
                tikiHeadMovmentEditor = Editor.CreateEditor(tikiHead.GetComponent<TikiHead>());
                tikiHeadHealthEditor = Editor.CreateEditor(tikiHead.GetComponent<HealthComponent>());

                tikiHeadHealthObject = new SerializedObject(tikiHead.GetComponent<HealthComponent>());
                tikiHeadObject = new SerializedObject(tikiHead.GetComponent<TikiHead>());
                tikiHeadScaleObject = new SerializedObject(tikiHead.GetComponent<Transform>());
            }
            else
            {
                Debug.LogError("Failed to get slugPrefab in Gameplay Editor");
            }
        }

        private void SetupTikiHeadHealthEditor()
        {
            _TikiHeadMaxHealth = slugHealthObject.FindProperty("maxHealth");
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
            _TikiHeadWhatIsGround  = tikiHeadObject.FindProperty("whatIsGround");
            _TikiHeadSightLayers = tikiHeadObject.FindProperty("sightLayers");
            _TikiHeadDrawDebug = tikiHeadObject.FindProperty("drawDebug");
            _TikiHeadShakeIntensity = tikiHeadObject.FindProperty("cameraShakeIntensity");
            _TikiHeadShakeTime = tikiHeadObject.FindProperty("cameraShakeTime");
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
            // Set Slug scale var
            _SlugScale = slugScaleObject.FindProperty("m_LocalScale");
            // Set Tiki Head scale var
            _TikiHeadScale = tikiHeadScaleObject.FindProperty("m_LocalScale");
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

        #region Status Effect Functions
        private void SetupStatusEffects()
        {
            var leechEffect = Resources.Load("Status Effects/Leeching_SSE") as ScriptableStatusEffect;
            var playerSlowEffect = Resources.Load("Status Effects/PlayerSlowing_SSE") as ScriptableStatusEffect;
            var healEffect = Resources.Load("Status Effects/Heal_SSE");

            if (leechEffect)
            {
                leechingSEEditor = Editor.CreateEditor(leechEffect);
            }
            else
            {
                Debug.LogError("Failed to get leechEffect in SetupStatusEffects Function in GameplayEditor");
            }

            if (playerSlowEffect)
            {
                playerSlowSEEditor = Editor.CreateEditor(playerSlowEffect);
            }
            else
            {
                Debug.LogError("Failed to get playerSlowEffect in SetupStatusEffects Function in GameplayEditor");
            }

            if (healEffect)
            {
                healSEEditor = Editor.CreateEditor(healEffect);
            }
            else
            {
                Debug.LogError("Failed to get healEffect in SetupStatusEffects Function in GameplayEditor");
            }
        }
        #endregion

        #region Spell Functions
        private void SetupSpells()
        {
            var playerDash = Resources.Load("Spells/Player_Dash_S");

            if (playerDash)
            {
                playerDashEditor = Editor.CreateEditor(playerDash);
            }
            else
            {
                Debug.LogError("Failed to get playerDash in Spell Functions in GameplayEditor");
            }
        }
        #endregion

        public void OnGUI()
        {
            tabs = GUILayout.Toolbar(tabs, new string[] { "Player Settings", "Enemy Settings", "Game Settings", "Status Effects", "Spells" });

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

                    SetupStatusEffectsUI();

                    GUILayout.EndScrollView();
                    break;
                case 4:
                    scrollPosition = GUILayout.BeginScrollView(scrollPosition, true, true, GUILayout.Width(position.width), GUILayout.Height(position.height));

                    SetupSpellUI();

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
            if (Foldout("Leech Settings", leechMovementObject.targetObject))
            {
                EditorGUI.indentLevel += indentLevel;

                // fetch current values from the target
                leechScaleObject.Update();

                // Start a code block to check for GUI changes
                EditorGUI.BeginChangeCheck();

                EditorGUILayout.PropertyField(_LeechScale, new GUIContent("Leech Scale"));

                if (EditorGUI.EndChangeCheck())
                {
                    // Apply values to the target
                    leechScaleObject.ApplyModifiedProperties();

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

                // Apply values to the target
                leechHealthObject.ApplyModifiedProperties();

                // Apply values to the target
                leechMovementObject.ApplyModifiedProperties();

                GUILayout.Space(foldoutSpaceing);
            }
            #endregion

            #region Leech Father UI
            if (Foldout("Leech Father Settings", leechFatherMovementObject.targetObject))
            {
                EditorGUI.indentLevel += indentLevel;

                // fetch current values from the target
                leechFatherScaleObject.Update();

                // Start a code block to check for GUI changes
                EditorGUI.BeginChangeCheck();

                EditorGUILayout.PropertyField(_LeechFatherScale, new GUIContent("Leech Father Scale"));

                if (EditorGUI.EndChangeCheck())
                {
                    // Apply values to the target
                    leechFatherScaleObject.ApplyModifiedProperties();

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
            if (Foldout("Leech Mother Settings", leechMotherMovementObject.targetObject))
            {
                EditorGUI.indentLevel += indentLevel;

                // fetch current values from the target
                leechMotherScaleObject.Update();

                // Start a code block to check for GUI changes
                EditorGUI.BeginChangeCheck();

                EditorGUILayout.PropertyField(_LeechMotherScale, new GUIContent("Leech Mother Scale"));

                if (EditorGUI.EndChangeCheck())
                {
                    // Apply values to the target
                    leechMotherScaleObject.ApplyModifiedProperties();

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
            if (Foldout("Shaman Settings", shamanObject.targetObject))
            {
                EditorGUI.indentLevel += indentLevel;

                // fetch current values from the target
                shamanScaleObject.Update();

                // Start a code block to check for GUI changes
                EditorGUI.BeginChangeCheck();

                EditorGUILayout.PropertyField(_ShamanScale, new GUIContent("Shaman Scale"));

                if (EditorGUI.EndChangeCheck())
                {
                    // Apply values to the target
                    shamanScaleObject.ApplyModifiedProperties();

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

                // Apply values to the target
                shamanObject.ApplyModifiedProperties();

                // Apply values to the target
                shamanHealthObject.ApplyModifiedProperties();

                GUILayout.Space(foldoutSpaceing);
            }
            #endregion

            #region Axe Thrower UI
            if (Foldout("Axe Thrower Settings", axeThrowerObject.targetObject))
            {
                EditorGUI.indentLevel += indentLevel;

                // fetch current values from the target
                axeThrowerScaleObject.Update();

                // Start a code block to check for GUI changes
                EditorGUI.BeginChangeCheck();

                EditorGUILayout.PropertyField(_AxeThrowerScale, new GUIContent("Axe Thrower Scale"));

                if (EditorGUI.EndChangeCheck())
                {
                    // Apply values to the target
                    axeThrowerScaleObject.ApplyModifiedProperties();

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
            if (Foldout("Slug Settings", slugMovementObject.targetObject))
            {
                EditorGUI.indentLevel += indentLevel;

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
                    }
                }

                // Apply values to the target
                slugHealthObject.ApplyModifiedProperties();

                // Apply values to the target
                slugMovementObject.ApplyModifiedProperties();

                GUILayout.Space(foldoutSpaceing);
            }
            #endregion

            #region Tiki Head UI
            if (Foldout("Tiki Head Settings", tikiHeadObject.targetObject))
            {
                EditorGUI.indentLevel += indentLevel;

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
                    }
                }

                // Apply values to the target
                tikiHeadHealthObject.ApplyModifiedProperties();

                // Apply values to the target
                tikiHeadObject.ApplyModifiedProperties();

                GUILayout.Space(foldoutSpaceing);
            }
            #endregion
        }
        #endregion

        #region Status Effect UI
        private void SetupStatusEffectsUI()
        {
            if (Foldout("Leeching", leechingSEEditor.target))
            {
                EditorGUI.indentLevel += indentLevel;

                leechingSEEditor.OnInspectorGUI();
            }

            if (Foldout("Player Slow", playerSlowSEEditor.target))
            {
                EditorGUI.indentLevel += indentLevel;

                playerSlowSEEditor.OnInspectorGUI();
            }

            if (Foldout("Heal", healSEEditor.target))
            {
                EditorGUI.indentLevel += indentLevel;

                healSEEditor.OnInspectorGUI();
            }
        }
        #endregion

        #region Spell UI
        private void SetupSpellUI()
        {
            if (Foldout("Player Dash", playerDashEditor.target))
            {
                EditorGUI.indentLevel += indentLevel;

                playerDashEditor.OnInspectorGUI();
            }
        }
        #endregion

        private bool Foldout(string name, Object @object)
        {
            string prefKey = @object.GetInstanceID() + ".Foldout." + name;
            bool foldoutState = EditorPrefs.GetBool(prefKey, false);
            bool newFoldoutState = EditorGUILayout.Foldout(foldoutState, name, true);
            if (newFoldoutState != foldoutState)
            {
                EditorPrefs.SetBool(prefKey, newFoldoutState);
            }
            return newFoldoutState;
        }
    }
}
#endif
      