using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraManager : MonoBehaviour
{
    private List<BuffEffect> currentBuffs = new List<BuffEffect>();
    private List<DebuffEffect> currentDebuffs = new List<DebuffEffect>();
    private PlayerUIManager playerUIManager = null;

    private void Start()
    {
        playerUIManager = FindObjectOfType<PlayerUIManager>();
    }

    public void ApplyBuff(GameObject target, ScriptableBuff buffToApply, bool createIcon)
    {
        BuffEffect buff = Instantiate(buffToApply.buffEffect, transform.position, Quaternion.identity) as BuffEffect;

        if (createIcon)
        {
            var buffIcon = CreateBuffIcon(buffToApply);

            buff.StartBuff(buffToApply.buffAmount, buffToApply.duration, this, buffToApply, buffIcon, target, buffToApply.stack, buffToApply.refresh);
        }
        else
        {
            buff.StartBuff(buffToApply.buffAmount, buffToApply.duration, this, buffToApply, target, buffToApply.stack, buffToApply.refresh);
        }

        currentBuffs.Add(buff);
    }

    public BuffEffect FindBuffOfType(ScriptableBuff buff)
    {
        if (buff)
        {
            BuffEffect foundBuff = null;

            foreach (BuffEffect buffEffect in currentBuffs)
            {
                var type = buffEffect.GetBuff().buffEffect;

                if (type == buff.buffEffect)
                {
                    foundBuff = buffEffect;
                    break;
                }
                else
                {
                    foundBuff = null;
                    continue;
                }
            }

            return foundBuff;
        }
        else
        {
            Debug.LogError("Was unable to find buff type on " + gameObject.name + "buff was invalid");
            return null;
        }
    }

    public void RemoveBuff(GameObject buffEffectObject, BuffEffect effect, BuffIcon iconToRemove)
    {
        if (buffEffectObject)
        {
            Destroy(buffEffectObject);

            currentBuffs.Remove(effect);
        }

        if (iconToRemove)
        {
            playerUIManager.RemoveBuffIcon(iconToRemove);
        }
        else
        {
            Debug.LogError("Failed to remove "  + buffEffectObject.name + "buff Icon is invalid");
        }
    }

    public void RemoveBuff(GameObject buffEffectObject, BuffEffect effect)
    {
        if (buffEffectObject)
        {
            Destroy(buffEffectObject);

            currentBuffs.Remove(effect);
        }
    }

    public void ApplyDebuff(GameObject target, ScriptableDebuff debuffToApply, bool createIcon)
    {
        DebuffEffect debuff = Instantiate(debuffToApply.debuffEffect, transform.position, Quaternion.identity) as DebuffEffect;

        if (createIcon)
        {
            var debuffIcon = CreateDebuffIcon(debuffToApply, debuffToApply.useTicks);

            debuff.StartDebuff(debuffToApply.ticks, debuffToApply.occurrence, this, debuffToApply, debuffIcon, target,  debuffToApply.useTicks, debuffToApply.refresh, debuffToApply.stack);
        }
        else
        {
            debuff.StartDebuff(debuffToApply.ticks, debuffToApply.occurrence, this, debuffToApply, target, debuffToApply.useTicks, debuffToApply.refresh, debuffToApply.stack);
        }

        currentDebuffs.Add(debuff);
    }

    public void RemoveDebuff(GameObject debuffEffectObject, DebuffEffect effect, DebuffIcon iconToRemove)
    {
        if (debuffEffectObject)
        {
            Destroy(debuffEffectObject);

            currentDebuffs.Remove(effect);
        }

        if (iconToRemove)
        {
            playerUIManager.RemoveDebuffIcon(iconToRemove);
        }
        else
        {
            Debug.LogError("Failed to remove " + debuffEffectObject.name + "debuff Icon is invalid");
        }
    }

    public void RemoveDebuff(GameObject debuffEffectObject, DebuffEffect effect)
    {
        if (debuffEffectObject)
        {
            Destroy(debuffEffectObject);

            currentDebuffs.Remove(effect);
        }
    }

    public DebuffEffect FindDebuffOtype(ScriptableDebuff debuff)
    {
        if (debuff)
        {
            DebuffEffect foundDebuff = null;

            foreach (DebuffEffect debuffEffect in currentDebuffs)
            {
                var type = debuffEffect.GetDebuff().debuffType;

                if (type == debuff.debuffType)
                {
                    foundDebuff = debuffEffect;
                    break;
                }
                else
                {
                    foundDebuff = null;
                    continue;
                }
            }

            return foundDebuff;
        }
        else
        {
            Debug.LogError("Was unable to find debuff type on " + gameObject.name + "debuff was invalid");
            return null;
        }
    }


    private BuffIcon CreateBuffIcon(ScriptableBuff buff)
    {
        return playerUIManager.AddBuffIcon(buff);
    }

    private DebuffIcon CreateDebuffIcon(ScriptableDebuff debuff, bool hasFillAmount)
    {
        return playerUIManager.AddDebuffIcon(debuff, hasFillAmount, debuff.useTicks);
    }

    public List<BuffEffect> GetCurrentBuffs()
    {
        return currentBuffs;
    }

    public PlayerUIManager GetUIManager()
    {
        return playerUIManager;
    }    

    public List<DebuffEffect> GetCurrentDebuffs()
    {
        return currentDebuffs;
    }
}
