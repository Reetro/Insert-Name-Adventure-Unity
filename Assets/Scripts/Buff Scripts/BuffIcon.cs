using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuffIcon : MonoBehaviour
{
    [SerializeField] private Image durationImage = null;
    [SerializeField] private Image icon = null;
    [SerializeField] TextMeshProUGUI stackText = null;
    [SerializeField] TextMeshProUGUI timer = null;

    private ScriptableBuff buff = null;
    private bool canFill = true;
    private float duration = 0f;
    private float defaultDuration = 0f;

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

        duration = buff.duration;
        defaultDuration = duration;

        if (buff.duration > 0)
        {
            timer.enabled = true;

            UpdateTimerText();
        }
        else
        {
            timer.enabled = false;
        }
    }

    private void UpdateTimerText()
    {
        timer.text = duration.ToString("F1");
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
        duration = defaultDuration;

        UpdateTimerText();
    }

    void Update()
    {
        if (canFill)
        {
            durationImage.fillAmount -= 1 / buff.duration * Time.deltaTime;

            if (timer.enabled)
            {
                duration -= Time.deltaTime;

                UpdateTimerText();
            }
        }
    }
}