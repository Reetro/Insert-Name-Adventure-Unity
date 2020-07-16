using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebuffIcon : MonoBehaviour
{
    [SerializeField] private Image durationImage = null;
    [SerializeField] private Image icon = null;
    [SerializeField] TextMeshProUGUI stackText = null;
    [SerializeField] TextMeshProUGUI timer = null;

    private ScriptableDebuff debuff = null;
    bool hasFillAmount = true;
    private float duration = 0f;
    private float defaultDuration = 0f;

    private void Start()
    {
        UpdateStackCount(1);
    }

    void Update()
    {
        if (hasFillAmount)
        {
            durationImage.fillAmount -= 1 / debuff.GetTotalTime() * Time.deltaTime;

            if (timer.enabled)
            {
                duration -= Time.deltaTime;

                UpdateTimerText();
            }
        }
    }
    /// <summary>
    /// Sets all needed values then checks if the given debuff has a tick count if it does will hide both the durationImage and the timer
    /// </summary>
    /// <param name="debuff"></param>
    /// <param name="hasFillAmount"></param>
    /// <param name="useTick"></param>
    public void StartCooldown(ScriptableDebuff debuff, bool hasFillAmount, bool useTick)
    {
        this.debuff = debuff;
        icon.sprite = this.debuff.artwork;
        this.hasFillAmount = hasFillAmount;

        if (useTick)
        {
            durationImage.enabled = true;
            durationImage.fillAmount = 1;
            
            if (debuff.GetTotalTime() > 0)
            {
                timer.enabled = true;

                duration = debuff.GetTotalTime();
                defaultDuration = duration;

                UpdateTimerText();
            }
        }
        else
        {
            durationImage.enabled = false;
            timer.enabled = false;
        }
    }
    /// <summary>
    /// Add the given amount to the current stack count if current stack count is less than 1 stack count will be hidden on the icon
    /// </summary>
    /// <param name="stackCount"></param>
    public void UpdateStackCount(int stackCount)
    {
        if (stackText)
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
    /// Update timer text to match current duration count
    /// </summary>
    private void UpdateTimerText()
    {
        timer.text = duration.ToString("F1");
    }
    /// <summary>
    /// Get the current debuff attach to this icon
    /// </summary>
    public ScriptableDebuff GetDebuff()
    {
        return debuff;
    }
}