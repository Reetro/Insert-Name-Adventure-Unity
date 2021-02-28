using GeneralScripts;
using PlayerScripts.PlayerControls;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameplayScripts.LevelScripts
{
    public class Checkpoint : MonoBehaviour
    {
        [Tooltip("Amount to heal the player")]
        public float healAmount = 3f;
        [Tooltip("The amount of health the player has to have or below to be healed")]
        public float healThreshold = 8f;

        private PlayerState playerState = null;
        private int currentLevelIndex = 0;
        private bool healedPlayer = false;

        public void ConstructCheckpoint()
        {
            playerState = FindObjectOfType<PlayerState>();
            currentLevelIndex = SceneManager.GetActiveScene().buildIndex;

            healedPlayer = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (healedPlayer) return;
            if (!collision.gameObject.CompareTag("Player")) return;
            if (GeneralFunctions.IsObjectDead(collision.gameObject)) return;
            playerState.SetCheckpointIndex(currentLevelIndex);

            if (!(GeneralFunctions.GetGameObjectHealthComponent(GeneralFunctions.GetPlayerGameObject()).CurrentHealth < healThreshold)) return;
            GeneralFunctions.HealTarget(collision.gameObject, healAmount);

            healedPlayer = true;
        }
    }
}