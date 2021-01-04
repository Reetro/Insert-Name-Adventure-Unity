using UnityEngine;
using StatusEffects;
using PlayerUI;
using LevelObjects.SceneLoading;
using LevelObjects.Saving;
using PlayerCharacter.GameSaving;
using PlayerCharacter.Controller;
using EnemyCharacter;
using GameplayManagement.Assets;
using PlayerUI.ToolTipUI;
using Spells;
using ComponentLibrary;

namespace GameplayManagement.SceneLoading
{
    public class SceneCreator : MonoBehaviour
    {
        [Header("Scene assets")]
        [SerializeField] private GameObject leechCollision = null;
        [SerializeField] private GameObject playerState = null;
        [SerializeField] private GameObject playerHud = null;
        [SerializeField] private GameObject levelLoader = null;
        [SerializeField] private HealthComponent myHealthComp = null;
        [SerializeField] private GameObject toolTipObject = null;
        [SerializeField] private bool createUI = true;

        private PlayerController playerController = null;

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
                levelExit.ConsturctExit(levelLoader.GetComponent<LevelLoader>());
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

            var tileDestoyers = FindObjectsOfType<TileDestroyer>();

            foreach (TileDestroyer tileDestroyer in tileDestoyers)
            {
                if (tileDestroyer)
                {
                    tileDestroyer.OnSceneCreated();
                }
            }
        }
    }
}