using UnityEngine;
using UnityEngine.UI;

public class DebuffIcon : MonoBehaviour
{
    [SerializeField] private Image durationImage = null;
    [SerializeField] private Image icon = null;
    private ScriptableDebuff debuff = null;

    public void StartCooldown(ScriptableDebuff debuff)
    {
        this.debuff = debuff;
        icon.sprite = this.debuff.artwork;
        durationImage.fillAmount = 1;
    }

    void Update()
    {
        durationImage.fillAmount -= 1 / debuff.GetTotalTime() * Time.deltaTime;
    }
}
