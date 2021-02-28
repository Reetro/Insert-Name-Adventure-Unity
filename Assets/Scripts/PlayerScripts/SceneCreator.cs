using EnemyScripts.BasicEnemyScripts;
using GameplayScripts;
using GameplayScripts.LevelScripts;
using GeneralScripts.GeneralComponents;
using GeneralScripts.UI;
using PlayerScripts.PlayerCombat;
using PlayerScripts.PlayerControls;
using Spells;
using UnityEngine;

namespace PlayerScripts
{
    public class SceneCreator : MonoBehaviour
    {
        [Header("Scene assets")]
        [SerializeField] private GameObject leechCollision;
        [SerializeField] private GameObject playerState;
        [SerializeField] private GameObject playerHud;
        [SerializeField] private GameObject levelLoader;
        [SerializeField] private HealthComponent myHealthComp;
        [SerializeField] private GameObject toolTipObject;
        [SerializeField] private bool createUI = true;

        private PlayerController playerController;

        /// <summary>
        /// Goes through 3 stages of setup Level, Player, Gameplay Components on each stage checks to see if the given assets currently exist will call there construction scripts if not it will spawn them in
        /// </summary>
        void Awake()
        {
            SetupLevel();

            SetUpPlayer();

            SetupGameplayComponents();
        }
        /// <summary>
        /// Checks if the level loader and level exit currently exist in the spawn and update all cached references in GameAssets
        /// </summary>
        private void SetupLevel()
        {
            GameAssets.UpdateReferences();

            levelLoader = Instantiate(levelLoader, Vector2.zero, Quaternion.identity);

            if (!levelLoader)
            {
                Debug.LogError("Scene Creator failed to spawn Level Loader");
            }

            var levelExit = FindObjectOfType<LevelExit>();

            // Only setup the level exit if it is currently valid
            if (levelExit)
            {
                levelExit.ConstructExit(levelLoader.GetComponent<LevelLoader>());
            }
        }
        /// <summary>
        /// Spawn in leech collision and setup checkpoint if valid then spawns in the player hud
        /// </summary>
        private void SetUpPlayer()
        {
            Instantiate(leechCollision, Vector2.zero, Quaternion.identity);

            playerState = Instantiate(playerState, Vector2.zero, Quaternion.identity);

            GameAssets.UpdateReferences();

            playerController = GetComponent<PlayerController>();

            if (!playerState)
            {
                Debug.LogError("Scene Creator failed to spawn Player State");
            }

            if (playerController)
            {
                playerController.OnSceneCreated();
            }
            else
            {
                Debug.LogError("Scene Creator failed to get Player Controller");
            }

            playerController.MyPlayerState = playerState.GetComponent<PlayerState>();

            var spear = FindObjectOfType<PlayerSpear>();

            if (spear)
            {
                spear.OnSceneLoaded();
            }
            else
            {
                Debug.LogError("Scene Creator Failed to find Player Spear");
            }

            var checkpoint = FindObjectOfType<Checkpoint>();

            // Sets the player state in active checkpoint 
            if (checkpoint)
            {
                checkpoint.ConstructCheckpoint();
            }

            if (createUI)
            {
                var hudInLevel = FindObjectOfType<PlayerUIManager>();

                if (!hudInLevel)
                {
                    playerHud = Instantiate(playerHud, Vector2.zero, Quaternion.identity);

                    if (playerHud)
                    {
                        playerHud.GetComponent<PlayerUIManager>().OnSceneCreated();

                        DontDestroyOnLoad(playerHud);
                    }
                    else
                    {
                        Debug.LogError("Scene Creator Failed to spawn playerHud");
                    }
                }
                else
                {
                    hudInLevel.OnSceneCreated();
                }

                myHealthComp.ConstructHealthComponentForPlayer();
            }

            myHealthComp.MyPlayerState = playerState.GetComponent<PlayerState>();
        }
        /// <summary>
        /// Sets up all gameplay related components then update spell call backs
        /// </summary>
        private void SetupGameplayComponents()
        {
            var enemies = FindObjectsOfType<EnemyBase>();

            foreach (EnemyBase enemyBase in enemies)
            {
                if (enemyBase)
                {
                    enemyBase.OnSceneCreated();
                }
            }

            Instantiate(toolTipObject, Vector2.zero, Quaternion.identity);

            var itemToolTips = FindObjectsOfType<ItemTooltip>();

            foreach (ItemTooltip itemTooltip in itemToolTips)
            {
                itemTooltip.OnSceneCreated();
            }

            var auraManagers = FindObjectsOfType<AuraManager>();

            foreach (AuraManager auraManager in auraManagers)
            {
                // Set all Aura Components UI reference to the player UI
                auraManager.SetUIManager(playerHud.GetComponent<PlayerUIManager>());
            }

            GameAssets.UpdateReferences();

            var activeSpells = FindObjectsOfType<Spell>();

            foreach (Spell spell in activeSpells)
            {
                if (spell)
                {
                    spell.SetupCallBacks();
                }
            }

            foreach (EnemyBase enemy in enemies)
            {
                if (enemy)
                {
                    enemy.SetupCallbacks();
                }
            }

            var tileDestroyers = FindObjectsOfType<TileDestroyer>();

            foreach (var tileDestroyer in tileDestroyers)
            {
                if (tileDestroyer)
                {
                    tileDestroyer.OnSceneCreated();
                }
            }

            var cameraColliders = FindObjectsOfType<CameraCollider>();

            foreach (CameraCollider cameraCollider in cameraColliders)
            {
                if (cameraCollider)
                {
                    cameraCollider.OnSceneCreated();
                }
            }
        }
    }
}