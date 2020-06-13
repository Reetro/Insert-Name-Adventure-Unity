using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraManager : MonoBehaviour
{
    public BuffIcon iconPrefab;

    private List<BuffEffect> currentBuffs = null;
    private GameObject currentTarget = null;
    private bool iconWasMade = false;

    public void ApplyBuff(GameObject target, ScriptableBuff buffToApply, bool createIcon)
    {
        currentTarget = target;

        BuffEffect buff = Instantiate(buffToApply.buffEffect, transform.position, Quaternion.identity) as BuffEffect;

        buff.StartBuff(buffToApply.buffAmount, buffToApply.duration, this, buffToApply);

        //currentBuffs.Add(buff);

        // TODO create a start buff function that actually apply buff effects to selected target

        //if (createIcon)
        //{
        //    CreateBuffIcon(buffToApply);
        //}
    }

    private void CreateBuffIcon(ScriptableBuff buff)
    {
        BuffIcon icon = Instantiate(iconPrefab, transform.parent);

        icon.Initialize(buff);

        GameAssets.instance.playerUIManager.AddBuffIcon(icon);

        iconWasMade = true;
    }

    public void RemoveBuff(BuffEffect buffEffect)
    {
        //currentBuffs.Remove(buffEffect);

        Destroy(buffEffect);
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
