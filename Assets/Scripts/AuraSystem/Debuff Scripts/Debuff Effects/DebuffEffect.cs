using System.Collections;
using UnityEngine;
using PlayerUI.Icons;

namespace AuraSystem.Effects
{
    public class DebuffEffect : MonoBehaviour
    {
        private float maxTicks = 9999999f;
        private Animation fadeOutAnimation = null;
        protected DebuffIcon icon = null;

        private bool firstRun = false;
        private bool shouldTick = true;
        private bool isFading = false;

        /// <summary>
        /// Sets all needed values for the given debuff and starts debuff ticking then adds an icon to the player hud
        /// </summary>
        /// <returns>The active DebuffEffect</returns>
        public virtual DebuffEffect StartDebuff(float ticks, float occurrence, AuraManager auraManager, ScriptableDebuff debuff, DebuffIcon icon, GameObject target, GameObject effect, bool useTicks, bool refresh, bool stack)
        {
            DebuffEffect debuffEffect = null;

            IsCurrentlyActive = isDebuffTypeActive(auraManager, debuff, out debuffEffect);

            if (!IsCurrentlyActive)
            {
                Ticks = ticks;
                DefaultTickCount = ticks;
                Occurrence = occurrence;
                MyAuraManager = auraManager;
                Debuff = debuff;
                this.icon = icon;
                shouldTick = useTicks;
                Damage = debuff.damage;
                Target = target;

                firstRun = true;
                IsCurrentlyActive = true;

                debuff.UpdateToolTip(StackCount);

                MyID = GeneralFunctions.GenID();

                VisualEffect = SpawnVisualEffect(effect, target.transform);

                fadeOutAnimation = icon.GetComponent<Animation>();

                if (!shouldTick)
                {
                    Ticks = maxTicks;
                    DefaultTickCount = ticks;
                }

                StartCoroutine(DebuffTimer());

                return this;
            }
            else if (refresh)
            {
                MyAuraManager = auraManager;
                Debuff = debuff;
                this.icon = icon;

                if (Debuff.useTicks)
                {
                    DefaultTickCount = ticks;
                }
                else
                {
                    DefaultTickCount = maxTicks;
                }

                RefreshDebuff(true, auraManager, debuff);

                return debuffEffect;
            }
            else if (stack)
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
        public virtual DebuffEffect StartDebuff(float ticks, float occurrence, AuraManager auraManager, ScriptableDebuff debuff, GameObject target, GameObject effect, bool useTick, bool refresh, bool stack)
        {
            DebuffEffect debuffEffect = null;

            IsCurrentlyActive = isDebuffTypeActive(auraManager, debuff, out debuffEffect);

            if (!IsCurrentlyActive)
            {
                Ticks = ticks;
                Occurrence = occurrence;
                MyAuraManager = auraManager;
                Debuff = debuff;
                shouldTick = useTick;
                Damage = debuff.damage;
                Target = target;

                debuff.UpdateToolTip(StackCount);

                MyID = GeneralFunctions.GenID();

                VisualEffect = SpawnVisualEffect(effect, target.transform);

                if (!shouldTick)
                {
                    Ticks = maxTicks;
                }

                firstRun = true;
                IsCurrentlyActive = true;

                StartCoroutine(DebuffTimer());

                return this;
            }
            else if (refresh)
            {
                MyAuraManager = auraManager;
                Debuff = debuff;

                RefreshDebuff(false, auraManager, debuff);

                return debuffEffect;
            }
            else if (stack)
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
        }
        /// <summary>
        /// Called when ever the current debuff ticks
        /// </summary>
        public virtual void ApplyDebuffEffect()
        {
            // To be overridden in child
            Debug.LogWarning("Debuff Effect: " + gameObject.name + "has no debuff effect being applied");
        }
        /// <summary>
        /// Called when tick count is <= 0
        /// </summary>
        public virtual void OnDebuffEnd()
        {
            if (VisualEffect)
            {
                GeneralFunctions.DetachFromParent(VisualEffect);

                Destroy(VisualEffect);
            }

            if (icon)
            {
                MyAuraManager.RemoveDebuff(gameObject, this, icon);
            }
            else
            {
                MyAuraManager.RemoveDebuff(gameObject, this);
            }
        }
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
                    if (!debuffEffect.isFading)
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
                        debuffEffect.MyAuraManager.RemoveDebuff(debuffEffect.gameObject, debuffEffect, debuffEffect.icon);
                    }
                    else
                    {
                        debuffEffect.MyAuraManager.RemoveDebuff(debuffEffect.gameObject, debuffEffect);
                    }
                }
            }
            else
            {
                Debug.LogError("Failed to remove debuff debuffEffect was not valid");
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

                localDebuff.ResetTickCount(localDebuff.Debuff.useTicks);

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
        /// <summary>
        /// Plays debuff fade out animation if debuff had a Icon and then destroys the DebuffEffect Gameobject
        /// </summary>
        public void StartRemove()
        {
            if (icon)
            {
                if (fadeOutAnimation)
                {
                    IsCurrentlyActive = false;

                    isFading = true;

                    fadeOutAnimation.Play();

                    StartCoroutine(RemoveDebuff());
                }
                else
                {
                    MyAuraManager.MyUIManager.RemoveDebuffIcon(icon);

                    Destroy(gameObject);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
        /// <summary>
        /// After the fade out animation is done destroy DebuffEffect Gameobject
        /// </summary>
        private IEnumerator RemoveDebuff()
        {
            yield return new WaitForSeconds(fadeOutAnimation.GetClip("Debuff_Fade_Out").length);

            MyAuraManager.MyUIManager.RemoveDebuffIcon(icon);

            Destroy(gameObject);
        }
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
        public bool IsCurrentlyActive { get; private set; } = false;
        /// <summary>
        /// Get the damage this debuff applies to it's target
        /// </summary>
        public float Damage { get; private set; } = 0f;
        /// <summary>
        /// Gets the spawned visual effect
        /// </summary>
        public GameObject VisualEffect { get; private set; } = null;
        /// <summary>
        /// Gets this debuff id
        /// </summary>
        public int MyID { get; private set; } = 0;
    }
}