using UnityEngine;
using AuraSystem;
using PlayerUI.ToolTipUI;
using PlayerUI;
using PlayerCharacter.GameSaving;
using LevelObjects.SceneLoading;
using LevelObjects.Saving;

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

        void Awake()
        {
            SetupScene();
        }
        /// <summary>
        /// Goes through 3 stages of setup Level, Player, Player Aura Manager on each stage checks to see if the given assets currently exist will call there construction scripts if not it will spawn them in
        /// </summary>
        private void SetupScene()
        {
            SetupLevel();

            SetUpPlayer();

            SetUpPlayerAuraManager();
        }
        /// <summary>
        /// Checks if the level loader and level exit currently exist in the spawn
        /// </summary>
        private void SetupLevel()
        {
            var loaderCount = FindObjectsOfType<LevelLoader>().Length;

            if (loaderCount <= 0)
            {
                levelLoader = Instantiate(levelLoader, Vector2.zero, Quaternion.identity);
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

            var playerStateCount = FindObjectsOfType<PlayerState>().Length;

            if (playerStateCount <= 0)
            {
                Instantiate(playerState, Vector2.zero, Quaternion.identity);
            }

            var checkpoint = FindObjectOfType<Checkpoint>();

            // Sets the player state in active checkpoint 
            if (checkpoint)
            {
                checkpoint.ConstructCheckpoint();
            }

            var hudCount = FindObjectsOfType<PlayerUIManager>().Length;

            if (hudCount <= 0)
            {
                playerHud = Instantiate(playerHud, Vector2.zero, Quaternion.identity);
            }

            myHealthComp.FindPlayerState(playerHud.GetComponent<PlayerUIManager>().HPBar);

            var toolTipCount = FindObjectsOfType<TooltipPopup>().Length;

            if (toolTipCount <= 0)
            {
                Instantiate(toolTipObject, Vector2.zero, Quaternion.identity);
            }
        }
        /// <summary>
        /// Setup the player's aura manager component
        /// </summary>
        private void SetUpPlayerAuraManager()
        {
            var auraManager = GetComponent<AuraManager>();

            if (auraManager)
            {
                auraManager.SetUIManager(playerHud.GetComponent<PlayerUIManager>());
            }
            else
            {
                Debug.LogWarning("Player has no Aura manager");
            }
        }
    }
}