using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayerUI.Icons;
using LevelObjects.SceneLoading;
using AuraSystem;

namespace PlayerUI
{
    public class PlayerUIManager : MonoBehaviour
    {
        [Header("Aura System UI Elements")]
        [SerializeField] GridLayoutGroup buffGridLayoutGroup = null;
        [SerializeField] GridLayoutGroup debuffGridLayoutGroup = null;
        [SerializeField] BuffIcon buffIconPrefab = null;
        [SerializeField] DebuffIcon debuffIconPrefab = null;

        [Header("GameOver UI Elements")]
        [SerializeField] Button loadCheckpointBTN = null;
        [SerializeField] TextMeshProUGUI gameOverText = null;

        [Header("Player UI")]
        [SerializeField] HealthBar healthBar = null;

        private List<BuffIcon> buffIcons = new List<BuffIcon>();
        private List<DebuffIcon> debuffIcons = new List<DebuffIcon>();
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
        /// Creates a buff icon and adds it to the screen
        /// </summary>
        /// <param name="buff"></param>
        /// <returns>The spawned Icon</returns>
        public BuffIcon AddBuffIcon(ScriptableBuff buff)
        {
            BuffIcon icon = Instantiate(buffIconPrefab, buffGridLayoutGroup.transform);
            buffIcons.Add(icon);

            icon.StartCooldown(buff);

            return icon;
        }
        /// <summary>
        /// Removes the given buff icon from the screen
        /// </summary>
        /// <param name="iconToRemove"></param>
        public void RemoveBuffIcon(BuffIcon iconToRemove)
        {
            buffIcons.Remove(iconToRemove);

            Destroy(iconToRemove.gameObject);
        }
        /// <summary>
        /// Finds a buff that is the same type as the given buff 
        /// </summary>
        /// <param name="buff"></param>
        public BuffIcon FindBuffIconByType(ScriptableBuff buff)
        {
            BuffIcon localIcon = null;

            foreach (BuffIcon icon in buffIcons)
            {
                if (icon.Buff.buffType == buff.buffType)
                {
                    localIcon = icon;
                    break;
                }
                else
                {
                    localIcon = null;
                    continue;
                }
            }
            return localIcon;
        }
        /// <summary>
        /// Creates a debuff icon and adds it to the screen
        /// </summary>
        /// <param name="debuff"></param>
        /// <param name="hasFillAmount"></param>
        /// <param name="useTick"></param>
        /// <returns>The spawned Icon</returns>
        public DebuffIcon AddDebuffIcon(ScriptableDebuff debuff, bool hasFillAmount)
        {
            DebuffIcon icon = Instantiate(debuffIconPrefab, debuffGridLayoutGroup.transform);
            debuffIcons.Add(icon);

            icon.StartCooldown(debuff, hasFillAmount);

            return icon;
        }
        /// <summary>
        /// Removes the given debuff icon from the screen
        /// </summary>
        /// <param name="iconToRemove"></param>
        public void RemoveDebuffIcon(DebuffIcon iconToRemove)
        {
            if (iconToRemove)
            {
                debuffIcons.Remove(iconToRemove);

                Destroy(iconToRemove.gameObject);
            }
        }
        /// <summary>
        /// Finds a debuff that is the same type as the given debuff
        /// </summary>
        /// <param name="debuff"></param>
        public DebuffIcon FindDebuffIconByType(ScriptableDebuff debuff)
        {
            DebuffIcon localIcon = null;

            foreach (DebuffIcon icon in debuffIcons)
            {
                if (icon.Debuff.GetType() == debuff.GetType())
                {
                    localIcon = icon;
                    break;
                }
            }
            return localIcon;
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