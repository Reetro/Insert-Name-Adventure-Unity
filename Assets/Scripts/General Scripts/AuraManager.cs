using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraManager : MonoBehaviour
{
    public BuffIcon iconPrefab;

    private List<BuffEffect> currentBuffs = new List<BuffEffect>();
    private GameObject currentTarget = null;
    private PlayerUIManager playerUIManager = null;

    private void Start()
    {
        playerUIManager = FindObjectOfType<PlayerUIManager>();
    }

    public void ApplyBuff(GameObject target, ScriptableBuff buffToApply, bool createIcon)
    {
        currentTarget = target;

        BuffEffect buff = Instantiate(buffToApply.buffEffect, transform.position, Quaternion.identity) as BuffEffect;

        if (createIcon)
        {
            var buffIcon = CreateBuffIcon(buffToApply);

            buff.StartBuff(buffToApply.buffAmount, buffToApply.duration, this, buffToApply, buffIcon);
        }
        else
        {
            buff.StartBuff(buffToApply.buffAmount, buffToApply.duration, this, buffToApply);
        }

        currentBuffs.Add(buff);
    }

    private BuffIcon CreateBuffIcon(ScriptableBuff buff)
    {
        return playerUIManager.AddBuffIcon(buff);
    }

    public void RemoveBuff(GameObject buffEffectObject, BuffEffect effect, BuffIcon iconToRemove)
    {
        currentBuffs.Remove(effect);

        if (iconToRemove)
        {
            playerUIManager.RemoveIcon(iconToRemove);
        }
        else
        {
            Debug.LogError("Failed to remove "  + buffEffectObject.name + "buff Icon is invalid");
        }

        Destroy(buffEffectObject);
    }

    public void RemoveBuff(GameObject buffEffectObject, BuffEffect effect)
    {
        currentBuffs.Remove(effect);

        Destroy(buffEffectObject);
    }

    public List<BuffEffect> GetCurrentBuffs()
    {
        return currentBuffs;
    }

    public GameObject GetCurrentTarget()
    {
        return currentTarget;
    }
}
