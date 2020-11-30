using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LevelObjects.SceneLoading;
using StatusEffects;

namespace PlayerUI
{
    public class PlayerUIManager : MonoBehaviour
    {
        [Header("GameOver UI Elements")]
        [SerializeField] Button loadCheckpointBTN = null;
        [SerializeField] TextMeshProUGUI gameOverText = null;

        [Header("Player UI")]
        [SerializeField] HealthBar healthBar = null;

        private LevelLoader levelLoader = null;

        private void Awake()
        {
            HideDeathUI();

            levelLoader = FindObjectOfType<LevelLoader>();

            loadCheckpointBTN.onClick.AddListener(loadCheckpoint_onclick);
        }
        /// <summary>
        /// Load the current checkpoint index
        /// </summary>
        private void loadCheckpoint_onclick()
        {
            levelLoader.LoadCheckpoint();
        }
        /// <summary>
        /// Set the internal health bar value to the player HP bar
        /// </summary>
        public HealthBar HPBar => healthBar;
        /// <summary>
        /// Hide the player death screen UI
        /// </summary>
        public void HideDeathUI()
        {
            loadCheckpointBTN.gameObject.SetActive(false);
            gameOverText.gameObject.SetActive(false);
        }
        /// <summary>
        /// Show the player death UI
        /// </summary>
        public void ShowDeathUI()
        {
            loadCheckpointBTN.gameObject.SetActive(true);
            gameOverText.gameObject.SetActive(true);
        }
    }
}