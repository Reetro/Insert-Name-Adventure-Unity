﻿using System.Collections;
using UnityEngine;

public class DebuffEffect : MonoBehaviour
{
    public float ticks = 0;
    private float occurrence = 0;
    private AuraManager auraManger = null;
    private ScriptableDebuff scriptedDebuff = null;
    private GameObject target = null;
    public int stackCount = 1;
    private float defaultTickCount = 0f;
    private float maxTicks = 99999f;
    private HealthComponent targetHealth = null;

    protected DebuffIcon icon = null;

    private bool firstRun = false;
    private bool shouldTick = true;
    private bool isActive = false;

    public virtual void StartDebuff(float ticks, float occurrence, AuraManager auraManger, ScriptableDebuff debuff, DebuffIcon icon, GameObject target, bool useTicks, bool refresh, bool stack)
    {
        isActive = isDebuffTypeActive(auraManger, debuff);

        if (!isActive)
        {
            this.ticks = ticks;
            defaultTickCount = ticks;
            this.occurrence = occurrence;
            this.auraManger = auraManger;
            scriptedDebuff = debuff;
            this.icon = icon;
            shouldTick = useTicks;

            firstRun = true;

            targetHealth = target.GetComponent<HealthComponent>();

            if (!shouldTick)
            {
                this.ticks = maxTicks;
                defaultTickCount = ticks;
            }

            StartCoroutine(DebuffTimer());
        }
        else if (refresh)
        {
            this.auraManger = auraManger;
            scriptedDebuff = debuff;
            this.icon = icon;

            if (scriptedDebuff.useTicks)
            {
                defaultTickCount = ticks;
            }
            else
            {
                defaultTickCount = maxTicks;

                Debug.Log(defaultTickCount);
            }

            RefreshDebuff(true, auraManger, debuff);
        }
        else if (stack)
        {
            this.auraManger = auraManger;
            scriptedDebuff = debuff;
            this.icon = icon;

            UpdateStack(true, auraManger, scriptedDebuff);
        }
    }

    public virtual void StartDebuff(float ticks, float occurrence, AuraManager auraManger, ScriptableDebuff debuff, GameObject target, bool useTick, bool refresh, bool stack)
    {
        isActive = isDebuffTypeActive(auraManger, debuff);

        if (!isActive)
        {
            this.ticks = ticks;
            this.occurrence = occurrence;
            this.auraManger = auraManger;
            scriptedDebuff = debuff;
            shouldTick = useTick;

            targetHealth = target.GetComponent<HealthComponent>();

            if (!shouldTick)
            {
                this.ticks = maxTicks;
            }

            firstRun = true;
            isActive = true;

            StartCoroutine(DebuffTimer());
        }
        else if (refresh)
        {
            this.auraManger = auraManger;
            scriptedDebuff = debuff;
            
            RefreshDebuff(false, auraManger, debuff);
        }
        else if (stack)
        {
            this.auraManger = auraManger;
            scriptedDebuff = debuff;

            UpdateStack(false, auraManger, scriptedDebuff);
        }
    }

    private IEnumerator DebuffTimer()
    {
        if (firstRun)
        {
            ApplyDebuffEffect();

            firstRun = false;
        }

        while (ticks > 0)
        {
            yield return new WaitForSecondsRealtime(occurrence);
            ticks--;

            ApplyDebuffEffect();
        }

        if (ticks <= 0)
        {
            OnDebuffEnd();
        }
    }

    public virtual void ApplyDebuffEffect()
    {
        // To be overridden in child
        Debug.LogWarning("Debuff Effect: " + gameObject.name + "has no debuff effect being applied");
    }

    public virtual void OnDebuffEnd()
    {
        if (icon)
        {
            auraManger.RemoveDebuff(gameObject, this, icon);
        }
        else
        {
            auraManger.RemoveDebuff(gameObject, this);
        }
    }

    private bool isDebuffTypeActive(AuraManager auraManager, ScriptableDebuff scriptableDebuff)
    {
        if (auraManager)
        {
            var debuff = auraManager.FindDebuffOtype(scriptableDebuff);

            return debuff;
        }
        else
        {
            Debug.LogError("Was unable to check debuff type on " + gameObject.name + "aura manager was invalid");
            return false;
        }
    }

    private void UpdateStack(bool useIcon, AuraManager auraManager, ScriptableDebuff scriptableDebuff)
    {
        if (auraManager)
        {
            var localDebuff = auraManager.FindDebuffOtype(scriptableDebuff);

            localDebuff.stackCount++;

            if (useIcon)
            {
                localDebuff.icon.UpdateStackCount(localDebuff.stackCount);

                auraManger.RemoveDebuff(gameObject, this, icon);
            }
            else
            {
                auraManger.RemoveDebuff(gameObject, this);
            }
        }
        else
        {
            Debug.LogError("Was unable to update debuff stack on " + gameObject.name + "aura manager was invalid");
        }
    }

    private void RefreshDebuff(bool useIcon, AuraManager auraManager, ScriptableDebuff scriptableDebuff)
    {
        if (auraManager)
        {
            var localDebuff = auraManager.FindDebuffOtype(scriptableDebuff);

            localDebuff.ResetTickCount(localDebuff.GetDebuff().useTicks);

            if (useIcon)
            {
                auraManager.RemoveDebuff(gameObject, this, icon);
            }
            else
            {
                auraManager.RemoveDebuff(gameObject, this);
            }
        }
        else
        {
            Debug.LogError("Was unable to refresh debuff on " + gameObject.name + "aura manager was invalid");
        }
    }

    public void ResetTickCount(bool useTick)
    {
        if (useTick)
        {
            ticks = defaultTickCount;
        }
        else
        {
            ticks = maxTicks;
        }
    }

    public int GetStackCount()
    {
        return stackCount;
    }

    public GameObject GetTarget()
    {
        return target;
    }

    public float GetTicks()
    {
        return ticks;
    }

    public float GetOccurrence()
    {
        return occurrence;
    }

    public AuraManager GetAuraManager()
    {
        return auraManger;
    }

    public ScriptableDebuff GetDebuff()
    {
        return scriptedDebuff;
    }

    public float GetDefaultTickCount()
    {
        return defaultTickCount;
    }

    public bool GetIsActive()
    {
        return isActive;
    }

    public HealthComponent GetTargetHealthComponent()
    {
        return targetHealth;
    }
}
