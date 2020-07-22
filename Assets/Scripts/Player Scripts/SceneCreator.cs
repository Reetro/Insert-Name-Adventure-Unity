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
        /// Goes through 3 stages of setup Level, Player, Gameplay Components on each stage checks to see if the given assets currently exist will call there construction scripts if not it will spawn them in
        /// </summary>
        private void SetupScene()
        {
            SetupLevel();

            SetUpPlayer();

            SetupGameplayComponents();
        }
        /// <summary>
        /// Checks if the level loader and level exit currently exist in the spawn
        /// </summary>
        private void SetupLevel()
        {
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

            Instantiate(playerState, Vector2.zero, Quaternion.identity);

            var checkpoint = FindObjectOfType<Checkpoint>();

            // Sets the player state in active checkpoint 
            if (checkpoint)
            {
                checkpoint.ConstructCheckpoint();
            }

            playerHud = Instantiate(playerHud, Vector2.zero, Quaternion.identity);

            myHealthComp.FindPlayerState(playerHud.GetComponent<PlayerUIManager>().HPBar);

            var toolTipCount = FindObjectsOfType<TooltipPopup>().Length;

            Instantiate(toolTipObject, Vector2.zero, Quaternion.identity);
        }
        /// <summary>
        /// Sets up all gameplay related components
        /// </summary>
        private void SetupGameplayComponents()
        {
            var auraManagers = FindObjectsOfType<AuraManager>();

            foreach (AuraManager auraManager in auraManagers)
            {
                // Set all Aura Components UI reference to the player UI
                auraManager.SetUIManager(playerHud.GetComponent<PlayerUIManager>());
            }
        }
    }
}