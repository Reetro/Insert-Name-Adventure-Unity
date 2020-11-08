using UnityEngine;
using StatusEffects;
using PlayerUI;
using LevelObjects.SceneLoading;
using LevelObjects.Saving;
using PlayerCharacter.GameSaving;
using PlayerCharacter.Controller;
using EnemyCharacter;
using GameplayManagement.Assets;
using EnemyCharacter.AI;

namespace PlayerCharacter.SceneLoading
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

            playerController = GetComponent<PlayerController>();

            playerController.MyPlayerState = playerState.GetComponent<PlayerState>();

            FindObjectOfType<PlayerSpear>().OnSceneLoaded();

            var checkpoint = FindObjectOfType<Checkpoint>();

            // Sets the player state in active checkpoint 
            if (checkpoint)
            {
                checkpoint.ConstructCheckpoint();
            }

            if (createUI)
            {
                playerHud = Instantiate(playerHud, Vector2.zero, Quaternion.identity);
            }

            myHealthComp.MyPlayerState = playerState.GetComponent<PlayerState>();

            myHealthComp.ConstructHealthComponent(playerHud.GetComponent<PlayerUIManager>().HPBar);
        }
        /// <summary>
        /// Sets up all gameplay related components
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

            var wormSegments = FindObjectsOfType<WormSegment>();

            foreach (WormSegment wormSegment in wormSegments)
            {
                if (wormSegment)
                {
                    wormSegment.OnSceneCreated();
                }
            }

            var wormMovementComponents = FindObjectsOfType<WormMovement>();

            foreach (WormMovement wormMovement in wormMovementComponents)
            {
                if (wormMovement)
                {
                    wormMovement.OnSceneCreated();
                }
            }

            Instantiate(toolTipObject, Vector2.zero, Quaternion.identity);

            var auraManagers = FindObjectsOfType<AuraManager>();

            foreach (AuraManager auraManager in auraManagers)
            {
                // Set all Aura Components UI reference to the player UI
                auraManager.SetUIManager(playerHud.GetComponent<PlayerUIManager>());
            }

            GameAssets.UpdateReferences();
        }
    }
}