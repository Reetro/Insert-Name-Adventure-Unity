﻿using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuffIcon : MonoBehaviour
{
    [SerializeField] private Image durationImage = null;
    [SerializeField] private Image icon = null;
    [SerializeField] TextMeshProUGUI stackText = null;
    private ScriptableBuff buff = null;
    private bool canFill = true;

    public void UpdatePause()
    {
        canFill = !canFill;
    }

    private void Start()
    {
        UpdateStackCount(1);
    }

    public void StartCooldown(ScriptableBuff buff)
    {
        this.buff = buff;
        icon.sprite = this.buff.artwork;
        canFill = true;
        durationImage.fillAmount = 1;
    }

    public void UpdateStackCount(int stackCount)
    {
        if (stackCount > 1)
        {
            stackText.enabled = true;

            stackText.text = stackCount.ToString();
        }
        else
        {
            stackText.enabled = false;
        }
    }

    public void ResetFill()
    {
        durationImage.fillAmount = 1;
    }

    void Update()
    {
        if (canFill)
        {
            durationImage.fillAmount -= 1 / buff.duration * Time.deltaTime;
        }
    }
}
