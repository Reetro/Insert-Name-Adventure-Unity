using UnityEngine;
using UnityEngine.SceneManagement;
using PlayerCharacter.Controller;
using PlayerUI;

namespace PlayerCharacter.GameSaving
{
    public class PlayerState : MonoBehaviour
    {
        private static float currentHealth = 0f;
        private static float maxHealth = 0f;
        private static int checkpointIndex = 0;
        private static bool isSceneLoading = false;

        public PlayerController player = null;
        public HealthBar playerHPBar = null;

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
        public int CheckpointInedex { get { return checkpointIndex; } }
        /// <summary>
        /// Checks to see if a scene is currently loading
        /// </summary>
        public bool IsLoadingScene { get { return isSceneLoading; } }

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
            checkpointIndex = index;
        }

        public void SetSceneLoading(bool value)
        {
            isSceneLoading = value;
        }

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
    }
}
