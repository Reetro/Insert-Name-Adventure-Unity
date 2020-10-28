using System.Collections;
using UnityEngine;

namespace StatusEffects.Effects
{
    public class StatusEffect : MonoBehaviour
    {
        private float defaultDuration = 0f;
        private float maxTicks = 9999999f;
        private float maxTimerTime = 9999999f;
        private float staticTimer = 0;
        private bool usingStaticTimer = false;

        private bool firstRun = false;

        #region Setup Functions
        /// <summary>
        /// Sets all needed values and starts effect timer
        /// </summary>
        /// <returns>The active Status Effect</returns>
        public virtual StatusEffect StartStatusEffect(AuraManager auraManager, ScriptableStatusEffect effect, GameObject target)
        {
            StatusEffect statusEffect = null;

            IsCurrentlyActive = IsStatusEffectgActive(auraManager, effect, out statusEffect);

            if (!IsCurrentlyActive)
            {
                UnpackEffect(auraManager, effect, target);

                MyID = GeneralFunctions.GenID();

                IsCurrentlyActive = true;

                usingStaticTimer = false;

                if (!UseTicks)
                {
                    if (effect.GetTotalTime() > 0)
                    {
                        staticTimer = effect.GetTotalTime();
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
                        staticTimer = effect.GetTotalTime();
                    }
                    else
                    {
                        Ticks = maxTicks;
                        DefaultTickCount = Ticks;
                    }
                }

                if (!usingStaticTimer)
                {
                    StartCoroutine(StatusEffectTimer());
                }

                return this;
            }
            else if (statusEffect.Refreshing)
            {
                Duration = statusEffect.Duration;
                defaultDuration = Duration;
                MyAuraManager = auraManager;
                CurrentStatusEffect = effect;

                RefreshStatusEffect(auraManager, CurrentStatusEffect);

                return statusEffect;
            }
            else if (statusEffect.Stacking)
            {
                MyAuraManager = auraManager;
                CurrentStatusEffect = effect;

                AddToStack(auraManager, CurrentStatusEffect);

                return statusEffect;
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
        /// <param name="effect"></param>
        /// <param name="icon"></param>
        /// <param name="target"></param>
        private void UnpackEffect(AuraManager auraManager, ScriptableStatusEffect statusEffect, GameObject target)
        {
            Duration = statusEffect.GetTotalTime();
            Target = target;
            UseTicks = statusEffect.UseTicks;
            defaultDuration = Duration;
            Value1 = statusEffect.Value1;
            MyAuraManager = auraManager;
            CurrentStatusEffect = statusEffect;
            Target = target;
            Stacking = statusEffect.Stacking;
            Refreshing = statusEffect.Refreshing;
            IsStatic = statusEffect.IsStatic;
            Ticks = statusEffect.Ticks;
            Occurrence = statusEffect.Occurrence;

            UsingTwoValues = statusEffect.UsingTwoValues;

            if (UsingTwoValues)
            {
                Value2 = statusEffect.Value2;
            }

            firstRun = true;
        }
        #endregion

        #region Status Effect Timer Functions
        /// <summary>
        /// Called when ever the current effect ticks or every frame if using a static timer
        /// </summary>
        protected virtual void ApplyStatusEffect()
        {
            // To be overridden in children
            Debug.LogWarning("Status Effect: " + gameObject.name + " has no status effect being applied");
        }
        /// <summary>
        /// Called when the effect timer reaches it's end
        /// </summary>
        protected virtual void OnStatusEffectEnd()
        {
            MyAuraManager.StartStatusEffectRemoval(gameObject, this);
        }
        /// <summary>
        /// Update effect duration every frame
        /// </summary>
        private void Update()
        {
            CurrentStatusEffect.UpdateToolTip(StackCount);

            if (usingStaticTimer)
            {
                if (IsCurrentlyActive)
                {
                    if (Duration > 0)
                    {
                        Duration -= Time.deltaTime;
                        ApplyStatusEffect();
                    }
                    else
                    {
                        IsCurrentlyActive = false;
                        Duration = 0;
                        OnStatusEffectEnd();
                    }
                }
            }
        }
        /// <summary>
        /// The actual effect timer that counts ticks
        /// </summary>
        private IEnumerator StatusEffectTimer()
        {
            if (firstRun)
            {
                ApplyStatusEffect();

                firstRun = false;
            }

            while (Ticks > 0)
            {
                yield return new WaitForSecondsRealtime(Occurrence);

                Ticks--;

                ApplyStatusEffect();
            }

            if (Ticks <= 0)
            {
                OnStatusEffectEnd();
            }
        }
        #endregion

        #region Status Effect Functions
        /// <summary>
        /// Checks to see if the current effect is active
        /// </summary>
        public bool IsStatusEffectgActive(AuraManager auraManager, ScriptableStatusEffect scriptableStatus, out StatusEffect statusEffect)
        {
            if (auraManager)
            {
                var effectBase = auraManager.FindStatusEffectOfType(scriptableStatus);

                statusEffect = effectBase;

                return effectBase;
            }
            else
            {
                Debug.LogError("Was unable to check status effect type on " + gameObject.name + " aura manager was invalid");

                statusEffect = null;

                return false;
            }
        }
        /// <summary>
        /// Completely reset this effect back to it's default values
        /// </summary>
        public void RefreshStatusEffect(AuraManager auraManager, ScriptableStatusEffect scriptableEffect)
        {
            if (auraManager)
            {
                var localEffect = auraManager.FindStatusEffectOfType(scriptableEffect);

                localEffect.IsCurrentlyActive = false;

                localEffect.ResetDuration();

                localEffect.IsCurrentlyActive = true;

                auraManager.StartStatusEffectRemoval(gameObject, this);
            }
            else
            {
                Debug.LogError("Was unable to refresh effect on " + gameObject.name + "aura manager was invalid");
            }
        }
        /// <summary>
        /// Adds a value of one to the current effect stack
        /// </summary>
        public void AddToStack(AuraManager auraManager, ScriptableStatusEffect scriptableEffect)
        {
            if (auraManager)
            {
                var statusEffect = auraManager.FindStatusEffectOfType(scriptableEffect);

                statusEffect.StackCount++;

                auraManager.StartStatusEffectRemoval(gameObject, this);
            }
            else
            {
                Debug.LogError("Was unable to stack effect on " + gameObject.name + " aura manager was invalid");
            }
        }
        /// <summary>
        /// Will remove a value of 1 from given the effect stack if stack is <= 0 effect will be removed
        /// </summary>
        /// <param name="useIcon"></param>
        /// <param name="scriptableDeeffect"></param>
        public void RemoveFromStack(StatusEffect effect)
        {
            if (effect)
            {
                effect.StackCount--;

                if (effect.StackCount <= 0)
                {
                    effect.MyAuraManager.StartStatusEffectRemoval(effect.gameObject, effect);
                }
            }
            else
            {
                Debug.LogError("Failed to remove effect effectEffect was not valid");
            }
        }
        /// <summary>
        /// Resets the effect duration back it's default value
        /// </summary>
        public void ResetDuration()
        {
            Duration = defaultDuration;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the current effect target
        /// </summary>
        public GameObject Target { get; private set; } = null;
        /// <summary>
        /// Gets the effect data
        /// </summary>
        public ScriptableStatusEffect CurrentStatusEffect { get; private set; } = null;
        /// <summary>
        /// Gets the effects stack count
        /// </summary>
        public int StackCount { get; private set; } = 1;
        /// <summary>
        /// Looks to see if the effect is currently active
        /// </summary>
        public bool IsCurrentlyActive { get; set; } = true;
        /// <summary>
        /// Get the current duration of the effect
        /// </summary>
        public float Duration { get; private set; } = 0f;
        /// <summary>
        /// The first effect amount
        /// </summary>
        public float Value1 { get; private set; } = 0f;
        /// <summary>
        /// The second effect amount is only set if UsingTwoValues is true
        /// </summary>
        public float Value2 { get; private set; } = 0f;
        /// <summary>
        /// Check to see if this status effect is using two values
        /// </summary>
        public bool UsingTwoValues { get; private set; } = false;
        /// <summary>
        /// Get the aura manager on the given effect
        /// </summary>
        public AuraManager MyAuraManager { get; private set; } = null;
        /// <summary>
        /// Gets this effect id
        /// </summary>
        public int MyID { get; private set; } = 0;
        /// <summary>
        /// Checks to see if the effect is stacking
        /// </summary>
        public bool Stacking { get; private set; } = false;
        /// <summary>
        /// Checks to see if the effect is refreshing
        /// </summary>
        public bool Refreshing { get; private set; } = false;
        /// <summary>
        /// Checks to see if the effect is static
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