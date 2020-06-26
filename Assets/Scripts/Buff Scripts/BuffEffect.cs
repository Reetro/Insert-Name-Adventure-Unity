using UnityEngine;

public class BuffEffect : MonoBehaviour
{
    public float duration = 0f;
    private float defaultDuration = 0f;
    private float buffAmount = 0f;
    private int stackCount = 1;
    private AuraManager auraManager = null;
    private ScriptableBuff scriptableBuff = null;
    private GameObject target = null;
    private HealthComponent targetHealth = null;

    protected BuffIcon icon = null;

    private bool buffIsRuning = false;
    private bool isActive = true;

    public virtual void StartBuff(float buffAmount, float duration, AuraManager auraManager, ScriptableBuff buff, BuffIcon icon, GameObject target, bool stack, bool refresh)
    {
        isActive = IsBuffActive(auraManager, buff);

        if (!isActive)
        {
            this.duration = duration;
            defaultDuration = this.duration;
            this.buffAmount = buffAmount;
            this.auraManager = auraManager;
            scriptableBuff = buff;
            this.icon = icon;
            this.target = target;

            targetHealth = target.GetComponent<HealthComponent>();

            buffIsRuning = true;
        }
        else if (refresh)
        {
            this.duration = duration;
            defaultDuration = this.duration;
            this.auraManager = auraManager;
            scriptableBuff = buff;
            this.icon = icon;

            RefreshBuff(true, auraManager, scriptableBuff);
        }
        else if (stack)
        {
            this.auraManager = auraManager;
            scriptableBuff = buff;
            this.icon = icon;

            UpdateStack(true, auraManager, scriptableBuff);
        }
    }

    public virtual void StartBuff(float buffAmount, float duration, AuraManager auraManager, ScriptableBuff buff, GameObject target, bool stack, bool refresh)
    {
        isActive = IsBuffActive(auraManager, buff);

        if (!isActive)
        {
            this.duration = duration;
            defaultDuration = this.duration; 
            this.buffAmount = buffAmount;
            this.auraManager = auraManager;
            scriptableBuff = buff;
            this.target = target;

            targetHealth = target.GetComponent<HealthComponent>();

            buffIsRuning = true;
        }
        else if (refresh)
        {
            this.duration = duration;
            defaultDuration = this.duration;
            this.auraManager = auraManager;
            scriptableBuff = buff;

            RefreshBuff(false, auraManager, scriptableBuff);
        }
        else if (stack)
        {
            this.auraManager = auraManager;
            scriptableBuff = buff;

            UpdateStack(false, auraManager, scriptableBuff);
        }
    }

    public virtual void ApplyBuffEffect(float buffAmount)
    {
        // To be overridden in children
        Debug.LogWarning("Buff Effect: " + gameObject.name + "has no buff effect being applied");
    }

    public virtual void OnBuffEnd()
    {
        if (icon)
        {
            auraManager.RemoveBuff(gameObject, this, icon);
        }
        else
        {
            auraManager.RemoveBuff(gameObject, this);
        }
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

    public bool IsBuffActive(AuraManager auraManager, ScriptableBuff scriptableBuff)
    {
        if (auraManager)
        {
            var buff = auraManager.FindBuffOfType(scriptableBuff);

            return buff;
        }
        else
        {
            Debug.LogError("Was unable to check buff type on " + gameObject.name + "aura manager was invalid");
            return false;
        }
    }

    public void RefreshBuff(bool useIcon, AuraManager auraManager, ScriptableBuff scriptableBuff)
    {
        if (auraManager)
        {
            var localBuff = auraManager.FindBuffOfType(scriptableBuff);

            localBuff.buffIsRuning = false;

            if (useIcon)
            {
                localBuff.icon.UpdatePause();
            }

            localBuff.ResetDuration();

            if (useIcon)
            {
                localBuff.icon.ResetFill();

                localBuff.buffIsRuning = true;

                localBuff.icon.UpdatePause();

                auraManager.RemoveBuff(gameObject, this, icon);
            }
            else
            {
                localBuff.buffIsRuning = true;

                auraManager.RemoveBuff(gameObject, this);
            }
        }
        else
        {
            Debug.LogError("Was unable to refresh buff on " + gameObject.name + "aura manager was invalid");
        }    
    }

    public void UpdateStack(bool useIcon, AuraManager auraManager, ScriptableBuff scriptableBuff)
    {
        if (auraManager)
        {
            var localBuff = auraManager.FindBuffOfType(scriptableBuff);

            localBuff.stackCount++;

            if (useIcon)
            {
                localBuff.icon.UpdateStackCount(localBuff.stackCount);

                auraManager.RemoveBuff(gameObject, this, icon);
            }
            else
            {
                auraManager.RemoveBuff(gameObject, this);
            }
        }
        else
        {
            Debug.LogError("Was unable to stack buff on " + gameObject.name + "aura manager was invalid");
        }
    }

    public GameObject GetTarget()
    {
        return target;
    }

    public void ResetDuration()
    {
        duration = defaultDuration;
    }

    public ScriptableBuff GetBuff()
    {
        return scriptableBuff;
    }

    public int GetStackCount()
    {
        return stackCount;
    }

    public bool GetIsActive()
    {
        return isActive;
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

    public HealthComponent GetTargetHealthComponent()
    {
        return targetHealth;
    }
}
