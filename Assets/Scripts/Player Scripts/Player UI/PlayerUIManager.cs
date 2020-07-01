﻿using System.Collections.Generic;
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

    private List<BuffIcon> buffIcons = new List<BuffIcon>();
    private List<DebuffIcon> debuffIcons = new List<DebuffIcon>();

    private void Start()
    {
        HideDeathUI();
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