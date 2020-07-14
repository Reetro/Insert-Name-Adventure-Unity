using System.Collections.Generic;
using UnityEngine;

public class AuraManager : MonoBehaviour
{
    private List<BuffEffect> currentBuffs = new List<BuffEffect>();
    private List<DebuffEffect> currentDebuffs = new List<DebuffEffect>();
    private PlayerUIManager playerUIManager = null;

    public void SetUIManager(PlayerUIManager playerUIManager)
    {
        this.playerUIManager = playerUIManager;
    }
    /// <summary>
    /// Will spawn the given buff into the scene then start the buff
    /// </summary>
    public void ApplyBuff(GameObject target, ScriptableBuff buffToApply, bool createIcon)
    {
        BuffEffect buff = Instantiate(buffToApply.buffEffect, transform.position, Quaternion.identity);

        if (createIcon)
        {
            var buffIcon = CreateBuffIcon(buffToApply);

            buff.StartBuff(buffToApply.buffAmount, buffToApply.duration, this, buffToApply, buffIcon, target, buffToApply.visualEffect, buffToApply.stack, buffToApply.refresh);
        }
        else
        {
            buff.StartBuff(buffToApply.buffAmount, buffToApply.duration, this, buffToApply, target, buffToApply.visualEffect, buffToApply.stack, buffToApply.refresh);
        }

        currentBuffs.Add(buff);
    }
    /// <summary>
    /// Finds a debuff of the same type as the given debuff
    /// </summary>
    /// <returns>A BuffEffect</returns>
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
    /// <summary>
    /// Finds a specific buff with the given ID 
    /// </summary>
    /// <returns>A BuffEffect</returns>
    public BuffEffect FindBuffByID(int ID)
    {
        BuffEffect foundBuff = null;

        foreach (BuffEffect buffEffect in currentBuffs)
        {
            var localID = buffEffect.GetID();

            if (localID == ID)
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
    /// <summary>
    /// Removes then destroy the given buff from the the aura manager then removes the icon from the playerUI
    /// </summary>
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
    /// <summary>
    /// Removes then destroy the given debuff from the aura manager
    /// </summary>
    public void RemoveBuff(GameObject buffEffectObject, BuffEffect effect)
    {
        if (buffEffectObject)
        {
            Destroy(buffEffectObject);

            currentBuffs.Remove(effect);
        }
    }
    /// <summary>
    /// Will spawn the given debuff into the scene then start the debuff
    /// </summary>
    public void ApplyDebuff(GameObject target, ScriptableDebuff debuffToApply, bool createIcon)
    {
        DebuffEffect debuff = Instantiate(debuffToApply.debuffEffect, transform.position, Quaternion.identity);

        if (createIcon)
        {
            var debuffIcon = CreateDebuffIcon(debuffToApply, debuffToApply.useTicks);

            debuff.StartDebuff(debuffToApply.ticks, debuffToApply.occurrence, this, debuffToApply, debuffIcon, target, debuffToApply.visualEffect, debuffToApply.useTicks, debuffToApply.refresh, debuffToApply.stack);
        }
        else
        {
            debuff.StartDebuff(debuffToApply.ticks, debuffToApply.occurrence, this, debuffToApply, target, debuffToApply.visualEffect, debuffToApply.useTicks, debuffToApply.refresh, debuffToApply.stack);
        }

        currentDebuffs.Add(debuff);
    }
    /// <summary>
    /// Removes then destroy the given debuff from the aura manager Gameobject then removes the icon from the playerUI
    /// </summary>
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
    /// <summary>
    /// Removes then destroy the given debuff from the aura manager
    /// </summary>
    public void RemoveDebuff(GameObject debuffEffectObject, DebuffEffect effect)
    {
        if (debuffEffectObject)
        {
            Destroy(debuffEffectObject);

            currentDebuffs.Remove(effect);
        }

        if (debuffEffectObject)
        {
            Destroy(debuffEffectObject);

            currentDebuffs.Remove(effect);
        }
    }
    /// <summary>
    /// Finds a debuff of the same type as the given debuff
    /// </summary>
    /// <returns>A DebuffEffect</returns>
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
    /// <summary>
    /// Finds a specific debuff with the given ID 
    /// </summary>
    /// <returns>A DebuffEffect</returns>
    public DebuffEffect FindDebuffByID(int ID)
    {
        DebuffEffect foundDebuff = null;

        foreach (DebuffEffect debuffEffect in currentDebuffs)
        {
            var localID = debuffEffect.GetID();

            if (localID == ID)
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
    /// <summary>
    /// Will add a buff icon to the player UI
    /// </summary>
    /// <returns>The created icon</returns>
    private BuffIcon CreateBuffIcon(ScriptableBuff buff)
    {
        return playerUIManager.AddBuffIcon(buff);
    }
    /// <summary>
    /// Will add a debuff icon to the player UI
    /// </summary>
    /// <returns>The created icon</returns>
    private DebuffIcon CreateDebuffIcon(ScriptableDebuff debuff, bool hasFillAmount)
    {
        return playerUIManager.AddDebuffIcon(debuff, hasFillAmount, debuff.useTicks);
    }
    /// <summary>
    /// Gets all current buffs on this Gameobject
    /// </summary>
    /// <returns>An array of BuffEffects</returns>
    public List<BuffEffect> GetCurrentBuffs()
    {
        return currentBuffs;
    }
    /// <summary>
    /// Gets the PlayerUIManager
    /// </summary>
    public PlayerUIManager GetPlayerUIManager()
    {
        return playerUIManager;
    }
    /// <summary>
    /// Gets all current debuffs on this Gameobject
    /// </summary>
    /// <returns>An array of DebuffEffects</returns>
    public List<DebuffEffect> GetCurrentDebuffs()
    {
        return currentDebuffs;
    }
}