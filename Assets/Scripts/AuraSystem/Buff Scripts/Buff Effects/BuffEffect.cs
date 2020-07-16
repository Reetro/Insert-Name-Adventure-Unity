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
    private GameObject visualEffect = null;
    private int ID = 0;

    protected BuffIcon icon = null;
    
    private bool buffIsRuning = false;
    private bool isActive = true;

    /// <summary>
    /// Sets all needed values and starts buff timer then adds an icon to the player hud
    /// </summary>
    public virtual void StartBuff(float buffAmount, float duration, AuraManager auraManager, ScriptableBuff buff, BuffIcon icon, GameObject target, GameObject effect, bool stack, bool refresh)
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

            visualEffect = SpawnVisualEffect(effect, target.transform);

            ID = GeneralFunctions.GenID();

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

            AddToStack(true, auraManager, scriptableBuff);
        }
    }
    /// <summary>
    /// Sets all needed values and starts buff timer
    /// </summary>
    public virtual void StartBuff(float buffAmount, float duration, AuraManager auraManager, ScriptableBuff buff, GameObject target, GameObject effect, bool stack, bool refresh)
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

            visualEffect = SpawnVisualEffect(effect, target.transform);

            ID = GeneralFunctions.GenID();

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

            AddToStack(false, auraManager, scriptableBuff);
        }
    }
    /// <summary>
    /// The effect that is called every second while the buff is active
    /// </summary>
    public virtual void ApplyBuffEffect(float buffAmount)
    {
        // To be overridden in children
        Debug.LogWarning("Buff Effect: " + gameObject.name + "has no buff effect being applied");
    }
    /// <summary>
    /// Called when the buff timer reaches it's end
    /// </summary>
    public virtual void OnBuffEnd()
    {
        if (visualEffect)
        {
            GeneralFunctions.DetachFromParent(visualEffect);

            Destroy(visualEffect);
        }

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
                ApplyBuffEffect(buffAmount * Time.deltaTime);
            }
            else
            {
                buffIsRuning = false;
                duration = 0;
                OnBuffEnd();
            }
        }
    }
    /// <summary>
    /// Checks to see if the current buff is active
    /// </summary>
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
    /// <summary>
    /// Completely reset this buff back to it's default values
    /// </summary>
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
    /// <summary>
    /// Adds a value of one to the current buff stack
    /// </summary>
    public void AddToStack(bool useIcon, AuraManager auraManager, ScriptableBuff scriptableBuff)
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
    /// <summary>
    /// Removes a value of one from the current buff stack
    /// </summary>
    public void RemoveFromStack(bool useIcon, AuraManager auraManager, ScriptableBuff scriptableBuff)
    {
        if (auraManager)
        {
            var localBuff = auraManager.FindBuffByID(GetID());

            localBuff.stackCount--;

            if (useIcon)
            {
                localBuff.icon.UpdateStackCount(localBuff.stackCount);
            }

            if (localBuff.stackCount <= 0)
            {
                if (useIcon)
                {
                    if (localBuff)
                    {
                        auraManager.RemoveBuff(localBuff.gameObject, localBuff, localBuff.icon);
                    }
                    else
                    {
                        var iconToRemove = auraManager.MyUIManager.FindBuffIconByType(scriptableBuff);

                        auraManager.MyUIManager.RemoveBuffIcon(iconToRemove);
                    }
                }
                else
                {
                    if (localBuff)
                    {
                        auraManager.RemoveBuff(localBuff.gameObject, localBuff);
                    }
                }
            }
        }
        else
        {
            Debug.LogError("Was unable to remove buff stack on " + gameObject.name + "aura manager was invalid");
        }
    }
    /// <summary>
    /// Spawn and attach the given visual effect
    /// </summary>
    /// <returns>The spawned visual effect</returns>
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
    /// <summary>
    /// Gets the current buff target
    /// </summary>
    public GameObject GetTarget()
    {
        return target;
    }
    /// <summary>
    /// Resets the buff duration back it's default value
    /// </summary>
    public void ResetDuration()
    {
        duration = defaultDuration;
    }
    /// <summary>
    /// Gets the buff data
    /// </summary>
    public ScriptableBuff GetBuff()
    {
        return scriptableBuff;
    }
    /// <summary>
    /// Gets the buffs stack count
    /// </summary>
    public int GetStackCount()
    {
        return stackCount;
    }
    /// <summary>
    /// Looks to see if the buff is currently active
    /// </summary>
    public bool GetIsActive()
    {
        return isActive;
    }
    /// <summary>
    /// Checks to see if the buff is running
    /// </summary>
    public bool GetBuffRunning()
    {
        return buffIsRuning;
    }
    /// <summary>
    /// Get the current duration of the buff
    /// </summary>
    public float GetDuration()
    {
        return duration;
    }
    /// <summary>
    /// Gets the actual buff amount
    /// </summary>
    public float GetBuffAmount()
    {
        return buffAmount;
    }
    /// <summary>
    /// Get the aura manager on the given buff
    /// </summary>
    public AuraManager GetAuraManager()
    {
        return auraManager;
    }
    /// <summary>
    /// Get the spawn visual effect
    /// </summary>
    public GameObject GetVisualEffect()
    {
        return visualEffect;
    }
    /// <summary>
    /// Gets this buff id
    /// </summary>
    public int GetID()
    {
        return ID;
    }
}
