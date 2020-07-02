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

    public ScriptableDebuff GetDebuff()
    {
        return debuff;
    }

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

                UpdateTimerText(duration);
            }
        }
        else
        {
            durationImage.enabled = false;
            timer.enabled = false;
        }
    }

    private void UpdateTimerText(float currentDuration)
    {
        timer.text = currentDuration.ToString("F1");
    }

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

    public void ResetFill()
    {
        durationImage.fillAmount = 1;
        duration = defaultDuration;

        UpdateTimerText(duration);
    }

    void Update()
    {
        if (hasFillAmount)
        {
            durationImage.fillAmount -= 1 / debuff.GetTotalTime() * Time.deltaTime;

            if (timer.enabled)
            {
                duration -= Time.deltaTime;

                UpdateTimerText(duration);
            }
        }
    }
}