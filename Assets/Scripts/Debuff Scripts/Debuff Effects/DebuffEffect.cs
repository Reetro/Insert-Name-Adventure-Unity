﻿using System.Collections;
using UnityEngine;

public class DebuffEffect : MonoBehaviour
{
    private float ticks = 0;
    private float occurrence = 0;
    private AuraManager auraManager = null;
    private ScriptableDebuff scriptedDebuff = null;
    private GameObject target = null;
    private int stackCount = 1;
    private float defaultTickCount = 0f;
    private float maxTicks = 99999f;
    private float damage = 0f;
    private GameObject visualEffect = null;

    protected DebuffIcon icon = null;
    protected GameplayObjectID idObject;

    private bool firstRun = false;
    private bool shouldTick = true;
    private bool isActive = false;

    public virtual void StartDebuff(float ticks, float occurrence, AuraManager auraManager, ScriptableDebuff debuff, DebuffIcon icon, GameObject target, GameObject effect, bool useTicks, bool refresh, bool stack)
    {
        isActive = isDebuffTypeActive(auraManager, debuff);

        if (!isActive)
        {
            this.ticks = ticks;
            defaultTickCount = ticks;
            this.occurrence = occurrence;
            this.auraManager = auraManager;
            scriptedDebuff = debuff;
            this.icon = icon;
            shouldTick = useTicks;
            damage = debuff.damage;
            this.target = target;

            firstRun = true;

            idObject = gameObject.AddComponent<GameplayObjectID>();

            visualEffect = SpawnVisualEffect(effect, target.transform);

            idObject.ConstructID();

            if (!shouldTick)
            {
                this.ticks = maxTicks;
                defaultTickCount = ticks;
            }

            StartCoroutine(DebuffTimer());
        }
        else if (refresh)
        {
            this.auraManager = auraManager;
            scriptedDebuff = debuff;
            this.icon = icon;

            if (scriptedDebuff.useTicks)
            {
                defaultTickCount = ticks;
            }
            else
            {
                defaultTickCount = maxTicks;
            }

            RefreshDebuff(true, auraManager, debuff);
        }
        else if (stack)
        {
            this.auraManager = auraManager;
            scriptedDebuff = debuff;
            this.icon = icon;

            AddToStack(true, auraManager, scriptedDebuff);
        }
    }

    public virtual void StartDebuff(float ticks, float occurrence, AuraManager auraManager, ScriptableDebuff debuff, GameObject target, GameObject effect, bool useTick, bool refresh, bool stack)
    {
        isActive = isDebuffTypeActive(auraManager, debuff);

        if (!isActive)
        {
            this.ticks = ticks;
            this.occurrence = occurrence;
            this.auraManager = auraManager;
            scriptedDebuff = debuff;
            shouldTick = useTick;
            damage = debuff.damage;
            this.target = target;

            idObject = gameObject.AddComponent<GameplayObjectID>();

            visualEffect = SpawnVisualEffect(effect, target.transform);

            idObject.ConstructID();

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
            this.auraManager = auraManager;
            scriptedDebuff = debuff;
            
            RefreshDebuff(false, auraManager, debuff);
        }
        else if (stack)
        {
            this.auraManager = auraManager;
            scriptedDebuff = debuff;

            AddToStack(false, auraManager, scriptedDebuff);
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
        if (visualEffect)
        {
            GeneralFunctions.DetachFromParent(visualEffect);

            Destroy(visualEffect);
        }

        if (icon)
        {
            auraManager.RemoveDebuff(gameObject, this, icon);
        }
        else
        {
            auraManager.RemoveDebuff(gameObject, this);
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

    private void AddToStack(bool useIcon, AuraManager auraManager, ScriptableDebuff scriptableDebuff)
    {
        if (auraManager)
        {
            var localDebuff = auraManager.FindDebuffOtype(scriptableDebuff);

            localDebuff.stackCount++;

            if (useIcon)
            {
                localDebuff.icon.UpdateStackCount(localDebuff.stackCount);

                auraManager.RemoveDebuff(gameObject, this, icon);
            }
            else
            {
                auraManager.RemoveDebuff(gameObject, this);
            }
        }
        else
        {
            Debug.LogError("Was unable to add debuff stack on " + gameObject.name + "aura manager was invalid");
        }
    }

    public void RemoveFromStack(bool useIcon, AuraManager auraManager, ScriptableDebuff scriptableDebuff)
    {
        if (auraManager)
        {
            var localDebuff = auraManager.FindDebuffOtype(scriptableDebuff);

            localDebuff.stackCount--;

            if (useIcon)
            {
                localDebuff.icon.UpdateStackCount(localDebuff.stackCount);
            }

            if (localDebuff.stackCount <= 0)
            {
                if (useIcon)
                {
                    if (localDebuff)
                    {
                        auraManager.RemoveDebuff(localDebuff.gameObject, localDebuff, localDebuff.icon);
                    }
                    else
                    {
                        var iconToRemove = auraManager.GetPlayerUIManager().FindDebuffIconByType(scriptableDebuff);

                        auraManager.GetPlayerUIManager().RemoveDebuffIcon(iconToRemove);
                    }
                }
                else
                {
                    if (localDebuff)
                    {
                        auraManager.RemoveDebuff(localDebuff.gameObject, localDebuff);
                    }
                    else
                    {
                        var iconToRemove = auraManager.GetPlayerUIManager().FindDebuffIconByType(scriptableDebuff);

                        auraManager.GetPlayerUIManager().RemoveDebuffIcon(iconToRemove);
                    }
                }
            }
        }
        else
        {
            Debug.LogError("Was unable to remove debuff stack on " + gameObject.name + "aura manager was invalid");
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
                localDebuff.icon.ResetFill();

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

    private GameObject SpawnVisualEffect(GameObject effect, Transform transform)
    {
        if (effect)
        {
            var spawnedEffect = Instantiate(effect, transform);

            return spawnedEffect;
        }
        else
        {
            return null;
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
        return auraManager;
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

    public float GetDamage()
    {
        return damage;
    }

    public GameObject GetVisualEffect()
    {
        return visualEffect;
    }
}
