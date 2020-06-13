using UnityEngine;
using UnityEngine.UI;

public class BuffIcon : MonoBehaviour
{
    [SerializeField] private Image durationImage = null;
    [SerializeField] private Image icon = null;
    private ScriptableBuff buff = null;

    public void StartCooldown(ScriptableBuff buff)
    {
        this.buff = buff;
        icon.sprite = this.buff.artwork;
        durationImage.fillAmount = 1;
    }

    void Update()
    {
        durationImage.fillAmount -= 1 / buff.duration * Time.deltaTime;
    }
}
