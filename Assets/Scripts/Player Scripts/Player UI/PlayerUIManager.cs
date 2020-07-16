using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    [Header("Tooltip Settings")]
    public float toolTipTextPadding = 4f;

    private List<BuffIcon> buffIcons = new List<BuffIcon>();
    private List<DebuffIcon> debuffIcons = new List<DebuffIcon>();
    private LevelLoader levelLoader = null;

    private void Awake()
    {
        HideDeathUI();

        levelLoader = FindObjectOfType<LevelLoader>();

        loadCheckpointBTN.onClick.AddListener(loadCheckpoint_onclick);
    }

    private void loadCheckpoint_onclick()
    {
        levelLoader.LoadCheckpoint();
    }

    public BuffIcon AddBuffIcon(ScriptableBuff buff)
    {
        BuffIcon icon = Instantiate(buffIconPrefab, buffGridLayoutGroup.transform);
        buffIcons.Add(icon);

        icon.StartCooldown(buff);

        return icon;
    }

    public void RemoveBuffIcon(BuffIcon iconToRemove)
    {
        buffIcons.Remove(iconToRemove);

        Destroy(iconToRemove.gameObject);
    }

    public BuffIcon FindBuffIconByType(ScriptableBuff buff)
    {
        BuffIcon localIcon = null;

        foreach (BuffIcon icon in buffIcons)
        {
            if (icon.GetBuff().buffType == buff.buffType)
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

    public DebuffIcon AddDebuffIcon(ScriptableDebuff debuff, bool hasFillAmount, bool useTick)
    {
        DebuffIcon icon = Instantiate(debuffIconPrefab, debuffGridLayoutGroup.transform);
        debuffIcons.Add(icon);

        icon.StartCooldown(debuff, hasFillAmount, useTick);

        return icon;
    }

    public void RemoveDebuffIcon(DebuffIcon iconToRemove)
    {
        debuffIcons.Remove(iconToRemove);

        Destroy(iconToRemove.gameObject);
    }

    public DebuffIcon FindDebuffIconByType(ScriptableDebuff debuff)
    {
        DebuffIcon localIcon = null;

        foreach (DebuffIcon icon in debuffIcons)
        {
            if (icon.GetDebuff().debuffType == debuff.debuffType)
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

    public HealthBar HPBar { get { return healthBar; }}

    public void HideDeathUI()
    {
        loadCheckpointBTN.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
    }

    public void ShowDeathUI()
    {
        loadCheckpointBTN.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(true);
    }
}