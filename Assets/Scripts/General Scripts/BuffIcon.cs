using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffIcon : MonoBehaviour
{
    [SerializeField] private Image durationImage = null;
    [SerializeField] private Image icon = null;

    public ScriptableBuff buff { get; private set; }

    public void Initialize(ScriptableBuff buff)
    {
        this.buff = buff;
        icon.sprite = buff.artwork;
        durationImage.fillAmount = 0;
    }

    void Update()
    {
        durationImage.fillAmount -= 1 / buff.duration * Time.deltaTime;
    }
}
