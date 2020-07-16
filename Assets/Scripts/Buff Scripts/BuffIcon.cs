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

    private void Start()
    {
        UpdateStackCount(1);
    }

    /// <summary>
    /// Sets all needed values such as the length of the buff
    /// </summary>
    /// <param name="buff"></param>
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
    /// <summary>
    /// Update timer text to match duration 
    /// </summary>
    private void UpdateTimerText()
    {
        timer.text = duration.ToString("F1");
    }
    /// <summary>
    /// Add the given amount to the current stack count if current stack count is less than 1 stack count will be hidden on the icon
    /// </summary>
    /// <param name="stackCount"></param>
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
    /// <summary>
    /// Will reset both the timer and fill image
    /// </summary>
    public void ResetFill()
    {
        durationImage.fillAmount = 1;
        duration = defaultDuration;

        UpdateTimerText();
    }
    /// <summary>
    /// Will toggle between filling and not filling the buff icon
    /// </summary>
    public void UpdatePause()
    {
        canFill = !canFill;
    }

    public ScriptableBuff GetBuff()
    {
        return buff;
    }
}