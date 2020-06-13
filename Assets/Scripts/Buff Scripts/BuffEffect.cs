using UnityEngine;

public class BuffEffect : MonoBehaviour
{
    private float duration = 0f;
    private float buffAmount = 0f;
    private AuraManager auraManager = null;
    private ScriptableBuff buff = null;

    private bool buffIsRuning = false;
    public virtual void StartBuff(float buffAmount, float duration, AuraManager auraManager, ScriptableBuff buff)
    {
        this.duration = duration;
        this.buffAmount = buffAmount;
        this.auraManager = auraManager;
        this.buff = buff;

        buffIsRuning = true;
    }

    public virtual void ApplyBuffEffect(float buffAmount)
    {
        // To be overridden in children
        Debug.LogWarning("Buff Effect: " + gameObject.name + "has no buff effect being applied");
    }

    public virtual void OnBuffEnd()
    {
        auraManager.RemoveBuff(gameObject, this);
    }

    private void Update()
    {
        if (buffIsRuning)
        {
            if (duration > 0)
            {
                duration -= Time.deltaTime;
                ApplyBuffEffect(buffAmount);
            }
            else
            {
                buffIsRuning = false;
                duration = 0;
                OnBuffEnd();
            }
        }
    }

    public ScriptableBuff GetBuff()
    {
        return buff;
    }

    public bool GetBuffRunning()
    {
        return buffIsRuning;
    }

    public float GetDuration()
    {
        return duration;
    }

    public float GetBuffAmount()
    {
        return buffAmount;
    }

    public AuraManager GetAuraManager()
    {
        return auraManager;
    }
}
