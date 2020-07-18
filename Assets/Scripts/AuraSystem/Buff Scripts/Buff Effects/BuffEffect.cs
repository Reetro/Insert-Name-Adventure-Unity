using UnityEngine;
using PlayerUI.Icons;
using System.Collections;

namespace AuraSystem.Effects
{
    public class BuffEffect : MonoBehaviour
    {
        private float defaultDuration = 0f;
        protected BuffIcon icon = null;

        /// <summary>
        /// Sets all needed values and starts buff timer then adds an icon to the player hud
        /// </summary>
        public virtual BuffEffect StartBuff(float buffAmount, float duration, AuraManager auraManager, ScriptableBuff buff, BuffIcon icon, GameObject target, GameObject effect, bool stack, bool refresh)
        {
            BuffEffect buffEffect = null;

            IsCurrentlyActive = IsBuffActive(auraManager, buff, out buffEffect);

            if (!IsCurrentlyActive)
            {
                Duration = duration;
                defaultDuration = Duration;
                BuffAmount = buffAmount;
                MyAuraManager = auraManager;
                Buff = buff;
                this.icon = icon;
                Target = target;

                VisualEffect = SpawnVisualEffect(effect, target.transform);

                MyID = GeneralFunctions.GenID();

                IsBuffRunning = true;

                return this;
            }
            else if (refresh)
            {
                Duration = duration;
                defaultDuration = Duration;
                MyAuraManager = auraManager;
                Buff = buff;
                this.icon = icon;

                RefreshBuff(true, auraManager, Buff);

                return buffEffect;
            }
            else if (stack)
            {
                MyAuraManager = auraManager;
                Buff = buff;
                this.icon = icon;

                AddToStack(true, auraManager, Buff);

                return buffEffect;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Sets all needed values and starts buff timer
        /// </summary>
        public virtual BuffEffect StartBuff(float buffAmount, float duration, AuraManager auraManager, ScriptableBuff buff, GameObject target, GameObject effect, bool stack, bool refresh)
        {
            BuffEffect buffEffect = null;

            IsCurrentlyActive = IsBuffActive(auraManager, buff, out buffEffect);

            if (!IsCurrentlyActive)
            {
                Duration = duration;
                defaultDuration = Duration;
                BuffAmount = buffAmount;
                MyAuraManager = auraManager;
                Buff = buff;
                Target = target;

                VisualEffect = SpawnVisualEffect(effect, target.transform);

                MyID = GeneralFunctions.GenID();

                IsBuffRunning = true;

                return this;
            }
            else if (refresh)
            {
                Duration = duration;
                defaultDuration = Duration;
                MyAuraManager = auraManager;
                Buff = buff;

                RefreshBuff(false, auraManager, Buff);

                return buffEffect;
            }
            else if (stack)
            {
                MyAuraManager = auraManager;
                Buff = buff;

                AddToStack(false, auraManager, Buff);

                return buffEffect;
            }
            else
            {
                return null;
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
            if (VisualEffect)
            {
                GeneralFunctions.DetachFromParent(VisualEffect);

                Destroy(VisualEffect);
            }

            if (icon)
            {
                MyAuraManager.RemoveBuff(gameObject, this, icon);
            }
            else
            {
                MyAuraManager.RemoveBuff(gameObject, this);
            }
        }

        private void Update()
        {
            if (IsBuffRunning)
            {
                Buff.UpdateToolTip(StackCount);

                if (Duration > 0)
                {
                    Duration -= Time.deltaTime;
                    ApplyBuffEffect(BuffAmount * Time.deltaTime);
                }
                else
                {
                    IsBuffRunning = false;
                    Duration = 0;
                    OnBuffEnd();
                }
            }
        }
        /// <summary>
        /// Checks to see if the current buff is active
        /// </summary>
        public bool IsBuffActive(AuraManager auraManager, ScriptableBuff scriptableBuff, out BuffEffect buffEffect)
        {
            if (auraManager)
            {
                var buff = auraManager.FindBuffOfType(scriptableBuff);

                buffEffect = buff;

                return buff;
            }
            else
            {
                Debug.LogError("Was unable to check buff type on " + gameObject.name + "aura manager was invalid");

                buffEffect = null;

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

                localBuff.IsBuffRunning = false;

                if (useIcon)
                {
                    localBuff.icon.UpdatePause();
                }

                localBuff.ResetDuration();

                if (useIcon)
                {
                    localBuff.icon.ResetFill();

                    localBuff.IsBuffRunning = true;

                    localBuff.icon.UpdatePause();

                    auraManager.RemoveBuff(gameObject, this, icon);
                }
                else
                {
                    localBuff.IsBuffRunning = true;

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

                localBuff.StackCount++;

                if (useIcon)
                {
                    localBuff.icon.UpdateStackCount(localBuff.StackCount);

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
        /// Will remove a value of 1 from given the buff stack if stack is <= 0 buff will be removed
        /// </summary>
        /// <param name="useIcon"></param>
        /// <param name="scriptableDebuff"></param>
        public void RemoveFromStack(bool useIcon, BuffEffect buffEffect)
        {
            if (buffEffect)
            {
                buffEffect.StackCount--;

                if (buffEffect.StackCount <= 0)
                {
                    if (useIcon)
                    {
                        buffEffect.MyAuraManager.RemoveBuff(buffEffect.gameObject, buffEffect, buffEffect.icon);
                    }
                    else
                    {
                        buffEffect.MyAuraManager.RemoveBuff(buffEffect.gameObject, buffEffect);
                    }
                }
            }
            else
            {
                Debug.LogError("Failed to remove buff buffEffect was not valid");
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
        public GameObject Target { get; private set; } = null;
        /// <summary>
        /// Resets the buff duration back it's default value
        /// </summary>
        public void ResetDuration()
        {
            Duration = defaultDuration;
        }
        /// <summary>
        /// Gets the buff data
        /// </summary>
        public ScriptableBuff Buff { get; private set; } = null;
        /// <summary>
        /// Gets the buffs stack count
        /// </summary>
        public int StackCount { get; private set; } = 1;
        /// <summary>
        /// Looks to see if the buff is currently active
        /// </summary>
        public bool IsCurrentlyActive { get; private set; } = true;
        /// <summary>
        /// Checks to see if the buff is running
        /// </summary>
        public bool IsBuffRunning { get; private set; } = false;
        /// <summary>
        /// Get the current duration of the buff
        /// </summary>
        public float Duration { get; private set; } = 0f;
        /// <summary>
        /// Gets the actual buff amount
        /// </summary>
        public float BuffAmount { get; private set; } = 0f;
        /// <summary>
        /// Get the aura manager on the given buff
        /// </summary>
        public AuraManager MyAuraManager { get; private set; } = null;
        /// <summary>
        /// Get the spawn visual effect
        /// </summary>
        public GameObject VisualEffect { get; private set; } = null;
        /// <summary>
        /// Gets this buff id
        /// </summary>
        public int MyID { get; private set; } = 0;
    }
}