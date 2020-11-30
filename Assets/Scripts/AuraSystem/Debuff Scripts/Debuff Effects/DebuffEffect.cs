using System.Collections;
using UnityEngine;
using PlayerUI.Icons;

namespace AuraSystem.Effects
{
    public class DebuffEffect : MonoBehaviour
    {
        private float maxTicks = 9999999f;
        private float maxTimerTime = 9999999f;
        private float staticTimer = 0;
        private bool usingStaticTimer = false;
        protected DebuffIcon icon = null;

        private bool firstRun = false;
        private GameObject tempEffect = null;

        #region Setup Functions
        /// <summary>
        /// Sets all needed values for the given debuff and starts debuff ticking then adds an icon to the player hud
        /// </summary>
        /// <returns>The active DebuffEffect</returns>
        public virtual DebuffEffect StartDebuff(AuraManager auraManager, ScriptableDebuff debuff, DebuffIcon icon, GameObject target)
        {
            DebuffEffect debuffEffect = null;

            IsCurrentlyActive = isDebuffTypeActive(auraManager, debuff, out debuffEffect);

            if (!IsCurrentlyActive)
            {
                UnPackDebuff(debuff.Ticks, debuff.Occurrence, auraManager, debuff, icon, target);

                debuff.UpdateToolTip(StackCount);

                MyID = GeneralFunctions.GenID();

                if (tempEffect)
                {
                    VisualEffect = SpawnVisualEffect(tempEffect, target.transform);
                }

                fadeOutAnimation = icon.GetComponent<Animation>();

                usingStaticTimer = false;

                if (!UseTicks)
                {
                    if (debuff.GetTotalTime() > 0)
                    {
                        staticTimer = debuff.GetTotalTime();
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
                        staticTimer = debuff.GetTotalTime();
                    }
                    else
                    {
                        Ticks = maxTicks;
                        DefaultTickCount = Ticks;
                    }
                }

                if (!usingStaticTimer)
                {
                    StartCoroutine(DebuffTimer());
                }

                return this;
            }
            else if (debuffEffect.Refreshing)
            {
                MyAuraManager = auraManager;
                Debuff = debuff;
                this.icon = icon;

                if (Debuff.UseTicks)
                {
                    DefaultTickCount = Ticks;
                }
                else
                {
                    DefaultTickCount = maxTicks;
                }

                RefreshDebuff(true, auraManager, debuff);

                return debuffEffect;
            }
            else if (debuffEffect.Stacking)
            {
                MyAuraManager = auraManager;
                Debuff = debuff;
                this.icon = icon;

                AddToStack(true, auraManager, debuff);

                return debuffEffect;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Sets all needed values for the given debuff and starts debuff ticking
        /// </summary>
        /// <returns>The active DebuffEffect</returns>
        public virtual DebuffEffect StartDebuff(AuraManager auraManager, ScriptableDebuff debuff, GameObject target)
        {
            DebuffEffect debuffEffect = null;

            IsCurrentlyActive = isDebuffTypeActive(auraManager, debuff, out debuffEffect);

            if (!IsCurrentlyActive)
            {
                UnPackDebuff(debuff.Ticks, debuff.Occurrence, auraManager, debuff, target);

                debuff.UpdateToolTip(StackCount);

                MyID = GeneralFunctions.GenID();

                if (tempEffect)
                {
                    VisualEffect = SpawnVisualEffect(tempEffect, target.transform);
                }

                usingStaticTimer = false;

                if (!UseTicks)
                {
                    if (debuff.GetTotalTime() > 0)
                    {
                        staticTimer = debuff.GetTotalTime();
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
                        staticTimer = debuff.GetTotalTime();
                    }
                    else
                    {
                        Ticks = maxTicks;
                        DefaultTickCount = Ticks;
                    }
                }

                if (!usingStaticTimer)
                {
                    StartCoroutine(DebuffTimer());
                }

                return this;
            }
            else if (debuffEffect.Refreshing)
            {
                MyAuraManager = auraManager;
                Debuff = debuff;

                RefreshDebuff(false, auraManager, debuff);

                return debuffEffect;
            }
            else if (debuffEffect.Stacking)
            {
                MyAuraManager = auraManager;
                Debuff = debuff;

                AddToStack(false, auraManager, Debuff);

                return debuffEffect;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Set all internal values inside the effect
        /// </summary>
        /// <param name="ticks"></param>
        /// <param name="occurrence"></param>
        /// <param name="auraManager"></param>
        /// <param name="debuff"></param>
        /// <param name="icon"></param>
        /// <param name="useTicks"></param>
        /// <param name="target"></param>
        private void UnPackDebuff(float ticks, float occurrence, AuraManager auraManager, ScriptableDebuff debuff, DebuffIcon icon, GameObject target)
        {
            Ticks = ticks;
            DefaultTickCount = ticks;
            Occurrence = occurrence;
            MyAuraManager = auraManager;
            Debuff = debuff;
            this.icon = icon;
            UseTicks = debuff.UseTicks;
            DebuffValue = debuff.DebuffValue;
            Target = target;
            Refreshing = debuff.Refreshing;
            Stacking = debuff.Stacking;
            IsStatic = debuff.IsStatic;
            tempEffect = debuff.VisualEffect;

            firstRun = true;
            IsCurrentlyActive = true;
        }
        /// <summary>
        /// Set all internal values inside the effect
        /// </summary>
        /// <param name="ticks"></param>
        /// <param name="occurrence"></param>
        /// <param name="auraManager"></param>
        /// <param name="debuff"></param>
        /// <param name="icon"></param>
        /// <param name="useTicks"></param>
        /// <param name="target"></param>
        private void UnPackDebuff(float ticks, float occurrence, AuraManager auraManager, ScriptableDebuff debuff, GameObject target)
        {
            Ticks = ticks;
            DefaultTickCount = ticks;
            Occurrence = occurrence;
            MyAuraManager = auraManager;
            Debuff = debuff;
            UseTicks = debuff.UseTicks;
            DebuffValue = debuff.DebuffValue;
            Target = target;
            Refreshing = debuff.Refreshing;
            Stacking = debuff.Stacking;
            IsStatic = debuff.IsStatic;
            tempEffect = debuff.VisualEffect;

            firstRun = true;
            IsCurrentlyActive = true;
        }
        #endregion

        #region Timer Functions
        /// <summary>
        /// The actual debuff timer that counts ticks
        /// </summary>
        private IEnumerator DebuffTimer()
        {
            if (firstRun)
            {
                ApplyDebuffEffect();

                firstRun = false;
            }

            while (Ticks > 0)
            {
                yield return new WaitForSecondsRealtime(Occurrence);

                Ticks--;

                ApplyDebuffEffect();
            }

            if (Ticks <= 0)
            {
                OnDebuffEnd();
            }
        }
        /// <summary>
        /// Update stack count on tooltip
        /// </summary>
        private void Update()
        {
            Debuff.UpdateToolTip(StackCount);

            if (usingStaticTimer)
            {
                if (IsCurrentlyActive)
                {
                    staticTimer -= Time.deltaTime;

                    ApplyDebuffEffect();

                    if (staticTimer <= 0)
                    {
                        OnDebuffEnd();
                    }
                }
            }
        }
        /// <summary>
        /// Called when ever the current debuff ticks or every frame if using a static timer
        /// </summary>
        protected virtual void ApplyDebuffEffect()
        {
            // To be overridden in child
            Debug.LogWarning("Debuff Effect: " + gameObject.name + " has no debuff effect being applied");
        }
        /// <summary>
        /// Called when tick count is <= 0
        /// </summary>
        protected virtual void OnDebuffEnd()
        {
            if (VisualEffect)
            {
                GeneralFunctions.DetachFromParent(VisualEffect);

                Destroy(VisualEffect);
            }

            if (icon)
            {
                MyAuraManager.StartDebuffRemoval(gameObject, icon);
            }
            else
            {
                MyAuraManager.StartDebuffRemoval(gameObject);
            }
        }
        #endregion

        #region Debuff Functions
        /// <summary>
        /// Checks to see if a debuff of the given type is currently active
        /// </summary>
        private bool isDebuffTypeActive(AuraManager auraManager, ScriptableDebuff scriptableDebuff, out DebuffEffect debuffEffect)
        {
            if (auraManager)
            {
                var debuff = auraManager.FindDebuffOtype(scriptableDebuff);

                debuffEffect = debuff;

                if (debuffEffect)
                {
                    if (!debuffEffect.IsFading)
                    {
                        return debuff;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                Debug.LogError("Was unable to check debuff type on " + gameObject.name + "aura manager was invalid");

                debuffEffect = null;

                return false;
            }
        }
        /// <summary>
        /// Will add a value of one to the current debuff stack count
        /// </summary>
        private void AddToStack(bool useIcon, AuraManager auraManager, ScriptableDebuff scriptableDebuff)
        {
            if (auraManager)
            {
                var localDebuff = auraManager.FindDebuffOtype(scriptableDebuff);

                localDebuff.StackCount++;

                if (useIcon)
                {
                    localDebuff.icon.UpdateStackCount(localDebuff.StackCount);

                    auraManager.StartDebuffRemoval(gameObject, icon);
                }
                else
                {
                    auraManager.StartDebuffRemoval(gameObject);
                }
            }
            else
            {
                Debug.LogError("Was unable to add debuff stack on " + gameObject.name + "aura manager was invalid");
            }
        }
        /// <summary>
        /// Will remove a value of 1 from given the debuff stack if stack is <= 0 debuff will be removed
        /// </summary>
        public void RemoveFromStack(bool useIcon, DebuffEffect debuffEffect)
        {
            if (debuffEffect)
            {
                debuffEffect.StackCount--;

                if (useIcon)
                {
                    debuffEffect.icon.UpdateStackCount(debuffEffect.StackCount);
                }

                if (debuffEffect.StackCount <= 0)
                {
                    if (useIcon)
                    {
                        debuffEffect.MyAuraManager.StartDebuffRemoval(debuffEffect.gameObject, debuffEffect.icon);
                    }
                    else
                    {
                        debuffEffect.MyAuraManager.StartDebuffRemoval(debuffEffect.gameObject);
                    }
                }
            }
        }
        /// <summary>
        /// Resets the debuff back to default values
        /// </summary>
        private void RefreshDebuff(bool useIcon, AuraManager auraManager, ScriptableDebuff scriptableDebuff)
        {
            if (auraManager)
            {
                var localDebuff = auraManager.FindDebuffOtype(scriptableDebuff);

                localDebuff.ResetTickCount(localDebuff.Debuff.UseTicks);

                if (useIcon)
                {
                    localDebuff.icon.ResetFill();

                    auraManager.StartDebuffRemoval(gameObject, icon);
                }
                else
                {
                    auraManager.StartDebuffRemoval(gameObject);
                }
            }
            else
            {
                Debug.LogError("Was unable to refresh debuff on " + gameObject.name + "aura manager was invalid");
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
        /// Resets the tick count back to it's default value 
        /// </summary>
        public void ResetTickCount(bool useTick)
        {
            if (useTick)
            {
                Ticks = DefaultTickCount;
            }
            else
            {
                Ticks = maxTicks;
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the current stack count
        /// </summary>
        public int StackCount { get; private set; } = 1;
        /// <summary>
        /// Gets the current debuff target
        /// </summary>
        public GameObject Target { get; private set; } = null;
        /// <summary>
        /// Gets the current tick count
        /// </summary>
        public float Ticks { get; private set; } = 0;
        /// <summary>
        /// Gets the interval between each tick count
        /// </summary>
        public float Occurrence { get; private set; } = 0;
        /// <summary>
        /// Get the aura manager on the given debuff
        /// </summary>
        public AuraManager MyAuraManager { get; private set; } = null;
        /// <summary>
        /// Gets the debuff data
        /// </summary>
        public ScriptableDebuff Debuff { get; private set; } = null;
        /// <summary>
        /// Gets the default tick count
        /// </summary>
        public float DefaultTickCount { get; private set; } = 0f;
        /// <summary>
        /// Checks to see if the current debuff is actual active
        /// </summary>
        public bool IsCurrentlyActive { get; set; } = false;
        /// <summary>
        /// Get the Debuff value amount
        /// </summary>
        public float DebuffValue { get; private set; } = 0f;
        /// <summary>
        /// Gets the spawned visual effect
        /// </summary>
        public GameObject VisualEffect { get; private set; } = null;
        /// <summary>
        /// Gets this debuff id
        /// </summary>
        public int MyID { get; private set; } = 0;
        /// <summary>
        /// Checks to see if the fade out animation is currently played
        /// </summary>
        public bool IsFading { get; set; } = false;
        /// <summary>
        /// Gets the icon's fade out animation
        /// </summary>
        public Animation fadeOutAnimation { get; private set;} = null;
        /// <summary>
        /// Checks to see if the debuff is stacking
        /// </summary>
        public bool Stacking { get; private set; }
        /// <summary>
        /// Checks to see if the debuff is refreshing
        /// </summary>
        public bool Refreshing { get; private set; }
        /// <summary>
        /// Checks to see if the debuff is static
        /// </summary>
        public bool IsStatic { get; private set; }
        /// <summary>
        /// Check to see if this effect is using ticks
        /// </summary>
        public bool UseTicks { get; private set; } = false;
        #endregion
    }
}