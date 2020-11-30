using UnityEngine;
using PlayerUI.Icons;
using System.Collections;

namespace AuraSystem.Effects
{
    public class BuffEffect : MonoBehaviour
    {
        private float defaultDuration = 0f;
        private GameObject tempEffect = null;
        protected BuffIcon icon = null;

        private float maxTicks = 9999999f;
        private float maxTimerTime = 9999999f;
        private float staticTimer = 0;
        private bool usingStaticTimer = false;

        private bool firstRun = false;

        #region Setup Functions
        /// <summary>
        /// Sets all needed values and starts buff timer then adds an icon to the player hud
        /// </summary>
        /// <returns>The active BuffEffect</returns>
        public virtual BuffEffect StartBuff(AuraManager auraManager, ScriptableBuff buff, BuffIcon icon, GameObject target)
        {
            BuffEffect buffEffect = null;

            IsCurrentlyActive = IsBuffActive(auraManager, buff, out buffEffect);

            if (!IsCurrentlyActive)
            {
                UnpackBuff(auraManager, buff, icon, target);

                MyID = GeneralFunctions.GenID();

                IsCurrentlyActive = true;

                fadeOutAnimation = icon.GetComponent<Animation>();

                if (tempEffect)
                {
                    VisualEffect = SpawnVisualEffect(tempEffect, target.transform);
                }

                fadeOutAnimation = icon.GetComponent<Animation>();

                usingStaticTimer = false;

                if (!UseTicks)
                {
                    if (buff.GetTotalTime() > 0)
                    {
                        staticTimer = buff.GetTotalTime();
                        usingStaticTimer = true;
                    }
                    else
                    {
                        staticTimer = maxTimerTime;
                        usingStaticTimer = true;
                    }
                }
                else
                {
                    if (Ticks > 0)
                    {
                        staticTimer = buff.GetTotalTime();
                    }
                    else
                    {
                        Ticks = maxTicks;
                        DefaultTickCount = Ticks;
                    }
                }

                if (!usingStaticTimer)
                {
                    StartCoroutine(BuffTimer());
                }

                return this;
            }
            else if (buffEffect.Refreshing)
            {
                Duration = buffEffect.Duration;
                defaultDuration = Duration;
                MyAuraManager = auraManager;
                Buff = buff;
                this.icon = icon;

                RefreshBuff(true, auraManager, Buff);

                return buffEffect;
            }
            else if (buffEffect.Stacking)
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
        /// <returns>The active BuffEffect</returns>
        public virtual BuffEffect StartBuff(AuraManager auraManager, ScriptableBuff buff, GameObject target)
        {
            BuffEffect buffEffect = null;

            IsCurrentlyActive = IsBuffActive(auraManager, buff, out buffEffect);

            if (!IsCurrentlyActive)
            {
                UnpackBuff(auraManager, buff, target);

                VisualEffect = SpawnVisualEffect(tempEffect, target.transform);

                MyID = GeneralFunctions.GenID();

                IsCurrentlyActive = true;

                usingStaticTimer = false;

                if (!UseTicks)
                {
                    if (buff.GetTotalTime() > 0)
                    {
                        staticTimer = buff.GetTotalTime();
                        usingStaticTimer = true;
                    }
                    else
                    {
                        staticTimer = maxTimerTime;
                        usingStaticTimer = true;
                    }
                }
                else
                {
                    if (Ticks > 0)
                    {
                        staticTimer = buff.GetTotalTime();
                    }
                    else
                    {
                        Ticks = maxTicks;
                        DefaultTickCount = Ticks;
                    }
                }

                if (!usingStaticTimer)
                {
                    StartCoroutine(BuffTimer());
                }

                return this;
            }
            else if (buffEffect.Refreshing)
            {
                Duration = buffEffect.Duration;
                defaultDuration = Duration;
                MyAuraManager = auraManager;
                Buff = buff;

                RefreshBuff(false, auraManager, Buff);

                return buffEffect;
            }
            else if (buffEffect.Stacking)
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
        /// Set all internal values inside the effect
        /// </summary>
        /// <param name="auraManager"></param>
        /// <param name="buff"></param>
        /// <param name="icon"></param>
        /// <param name="target"></param>
        private void UnpackBuff(AuraManager auraManager, ScriptableBuff buff, BuffIcon icon, GameObject target)
        {
            Duration = buff.GetTotalTime();
            Target = target;
            UseTicks = buff.UseTicks;
            defaultDuration = Duration;
            BuffValue = buff.BuffValue;
            MyAuraManager = auraManager;
            Buff = buff;
            this.icon = icon;
            Stacking = buff.Stacking;
            Refreshing = buff.Refreshing;
            IsStatic = buff.IsStatic;
            Ticks = buff.Ticks;
            Occurrence = buff.Occurrence;

            firstRun = true;
        }
        /// <summary>
        /// Set all internal values inside the effect
        /// </summary>
        /// <param name="auraManager"></param>
        /// <param name="buff"></param>
        /// <param name="icon"></param>
        /// <param name="target"></param>
        private void UnpackBuff(AuraManager auraManager, ScriptableBuff buff, GameObject target)
        {
            Duration = buff.GetTotalTime();
            Target = target;
            UseTicks = buff.UseTicks;
            defaultDuration = Duration;
            BuffValue = buff.BuffValue;
            MyAuraManager = auraManager;
            Buff = buff;
            Target = target;
            tempEffect = buff.VisualEffect;
            Stacking = buff.Stacking;
            Refreshing = buff.Refreshing;
            IsStatic = buff.IsStatic;
            Ticks = buff.Ticks;
            Occurrence = buff.Occurrence;

            firstRun = true;
        }
        #endregion

        #region Buff Timer Functions
        /// <summary>
        /// Called when ever the current buff ticks or every frame if using a static timer
        /// </summary>
        public virtual void ApplyBuffEffect()
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
                MyAuraManager.StartBuffRemoval(gameObject, this, icon);
            }
            else
            {
                MyAuraManager.StartBuffRemoval(gameObject, this);
            }
        }
        /// <summary>
        /// Update buff duration every frame
        /// </summary>
        private void Update()
        {
            Buff.UpdateToolTip(StackCount);

            if (usingStaticTimer)
            {
                if (IsCurrentlyActive)
                {
                    if (Duration > 0)
                    {
                        Duration -= Time.deltaTime;
                        ApplyBuffEffect();
                    }
                    else
                    {
                        IsCurrentlyActive = false;
                        Duration = 0;
                        OnBuffEnd();
                    }
                }
            }
        }
        /// <summary>
        /// The actual buff timer that counts ticks
        /// </summary>
        private IEnumerator BuffTimer()
        {
            if (firstRun)
            {
                ApplyBuffEffect();

                firstRun = false;
            }

            while (Ticks > 0)
            {
                yield return new WaitForSecondsRealtime(Occurrence);

                Ticks--;

                ApplyBuffEffect();
            }

            if (Ticks <= 0)
            {
                OnBuffEnd();
            }
        }
        #endregion

        #region Buff Functions
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

                localBuff.IsCurrentlyActive = false;

                if (useIcon)
                {
                    localBuff.icon.UpdatePause();
                }

                localBuff.ResetDuration();

                if (useIcon)
                {
                    localBuff.icon.ResetFill();

                    localBuff.IsCurrentlyActive = true;

                    localBuff.icon.UpdatePause();

                    auraManager.StartBuffRemoval(gameObject, this, icon);
                }
                else
                {
                    localBuff.IsCurrentlyActive = true;

                    auraManager.StartBuffRemoval(gameObject, this);
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

                    auraManager.StartBuffRemoval(gameObject, this, icon);
                }
                else
                {
                    auraManager.StartBuffRemoval(gameObject, this);
                }
            }
            else
            {
                Debug.LogError("Was unable to stack buff on " + gameObject.name + " aura manager was invalid");
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

                if (useIcon)
                {
                    buffEffect.icon.UpdateStackCount(buffEffect.StackCount);
                }

                if (buffEffect.StackCount <= 0)
                {
                    if (useIcon)
                    {
                        buffEffect.MyAuraManager.StartBuffRemoval(buffEffect.gameObject, buffEffect, buffEffect.icon);
                    }
                    else
                    {
                        buffEffect.MyAuraManager.StartBuffRemoval(buffEffect.gameObject, buffEffect);
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
        /// Resets the buff duration back it's default value
        /// </summary>
        public void ResetDuration()
        {
            Duration = defaultDuration;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the current buff target
        /// </summary>
        public GameObject Target { get; private set; } = null;
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
        public bool IsCurrentlyActive { get; set; } = true;
        /// <summary>
        /// Get the current duration of the buff
        /// </summary>
        public float Duration { get; private set; } = 0f;
        /// <summary>
        /// Gets the actual buff amount
        /// </summary>
        public float BuffValue { get; private set; } = 0f;
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
        /// <summary>
        /// Checks to see if the fade out animation is currently played
        /// </summary>
        public bool IsFading { get; set; } = false;
        /// <summary>
        /// Gets the icon's fade out animation
        /// </summary>
        public Animation fadeOutAnimation { get; private set; } = null;
        /// <summary>
        /// Checks to see if the buff is stacking
        /// </summary>
        public bool Stacking { get; private set; } = false;
        /// <summary>
        /// Checks to see if the buff is refreshing
        /// </summary>
        public bool Refreshing { get; private set; } = false;
        /// <summary>
        /// Checks to see if the buff is static
        /// </summary>
        public bool IsStatic { get; private set; }
        /// <summary>
        /// Gets the current tick count
        /// </summary>
        public float Ticks { get; private set; } = 0;
        /// <summary>
        /// Gets the interval between each tick count
        /// </summary>
        public float Occurrence { get; private set; } = 0;
        /// <summary>
        /// Gets the default tick count
        /// </summary>
        public float DefaultTickCount { get; private set; } = 0f;
        /// <summary>
        /// Check to see if this effect is using ticks
        /// </summary>
        public bool UseTicks { get; private set; } = false;
        #endregion
    }
}