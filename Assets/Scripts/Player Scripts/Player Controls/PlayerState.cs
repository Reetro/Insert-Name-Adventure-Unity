using UnityEngine;
using UnityEngine.SceneManagement;
using PlayerCharacter.Controller;
using PlayerUI;

namespace PlayerCharacter.GameSaving
{
    public class PlayerState : MonoBehaviour
    {
        public PlayerController player = null;
        public HealthBar playerHPBar = null;

        #region Local Variables
        private static float currentHealth = 0f;
        private static float maxHealth = 0f;
        private static int levelIndex = 0;
        private static bool isSceneLoading = false;
        #endregion

        #region Properites
        /// <summary>
        /// Get the players current health
        /// </summary>
        public float CurrentHealth { get { return currentHealth; } }
        /// <summary>
        /// Get the players max health
        /// </summary>
        public float MaxHealth { get { return maxHealth; } }
        /// <summary>
        /// Get the players current checkpoint
        /// </summary>
        public int LevelIndex { get { return levelIndex; } }
        /// <summary>
        /// Checks to see if a scene is currently loading
        /// </summary>
        public bool IsLoadingScene { get { return isSceneLoading; } }
        #endregion

        #region Setup Functions
        private void Awake()
        {
            int playerStateCount = FindObjectsOfType<PlayerState>().Length;

            if (playerStateCount > 1)
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
            }
        }

        public void SetCheckpointIndex(int index)
        {
            levelIndex = index;
        }

        public void SetSceneLoading(bool value)
        {
            isSceneLoading = value;
        }
        #endregion

        #region Health Functions
        public void SetPlayerHealth()
        {
            player = FindObjectOfType<PlayerController>();

            player.GetComponent<HealthComponent>().SetHealth(currentHealth, maxHealth);

            playerHPBar.SetHealth(currentHealth);
        }

        public void ResetHealthToMax()
        {
            currentHealth = maxHealth;
        }

        public void UpdatePlayerStateHP(float currentHP, float maxHP)
        {
            currentHealth = currentHP;
            maxHealth = maxHP;
        }
        #endregion

        #region Level Loading Events
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        }

        void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            isSceneLoading = false;

            SetPlayerHealth();
        }
        #endregion

        #region Game Saving / Loading Functions
        /// <summary>
        /// Saves all player game data
        /// </summary>
        public void SaveGame()
        {
            SaveSystem.SaveGame(this, player.gameObject);
        }
        /// <summary>
        /// Gets saved data then sets all local Variables and load the saved level
        /// </summary>
        public void LoadGame()
        {
            var loadedData = SaveSystem.LoadGame();

            levelIndex = loadedData.LevelIndex;

            currentHealth = loadedData.CurrentHealth;
            maxHealth = loadedData.MaxHealth;

            SceneManager.sceneLoaded += LoadPlayerPostion;

            GeneralFunctions.GetLevelLoader().LoadLevelAtIndex(levelIndex);
        }
        /// <summary>
        /// Set the player position to the saved player position when scene is done loading
        /// </summary>
        private void LoadPlayerPostion(Scene scene, LoadSceneMode mode)
        {
            var loadedData = SaveSystem.LoadGame();

            Vector3 position;
            position.x = loadedData.PlayerPosition[0];
            position.y = loadedData.PlayerPosition[1];
            position.z = loadedData.PlayerPosition[2];

            player.transform.position = position;

            SceneManager.sceneLoaded -= LoadPlayerPostion;
        }
        #endregion
    }
}
