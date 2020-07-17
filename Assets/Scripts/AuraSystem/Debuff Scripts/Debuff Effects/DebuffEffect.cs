using System.Collections;
using UnityEngine;
using PlayerUI.Icons;

namespace AuraSystem.Effects
{
    public class DebuffEffect : MonoBehaviour
    {
        private float maxTicks = 9999999f;
        protected DebuffIcon icon = null;

        private bool firstRun = false;
        private bool shouldTick = true;

        /// <summary>
        /// Sets all needed values for the given debuff and starts debuff ticking then adds an icon to the player hud
        /// </summary>
        public virtual void StartDebuff(float ticks, float occurrence, AuraManager auraManager, ScriptableDebuff debuff, DebuffIcon icon, GameObject target, GameObject effect, bool useTicks, bool refresh, bool stack)
        {
            IsCurrentlyActive = isDebuffTypeActive(auraManager, debuff);

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

                debuff.UpdateStackCount(StackCount);

                MyID = GeneralFunctions.GenID();

                VisualEffect = SpawnVisualEffect(effect, target.transform);

                if (!shouldTick)
                {
                    Ticks = maxTicks;
                    DefaultTickCount = ticks;
                }

                StartCoroutine(DebuffTimer());
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
            }
            else if (stack)
            {
                MyAuraManager = auraManager;
                Debuff = debuff;
                this.icon = icon;

                AddToStack(true, auraManager, Debuff);
            }
        }
        /// <summary>
        /// Sets all needed values for the given debuff and starts debuff ticking
        /// </summary>
        public virtual void StartDebuff(float ticks, float occurrence, AuraManager auraManager, ScriptableDebuff debuff, GameObject target, GameObject effect, bool useTick, bool refresh, bool stack)
        {
            IsCurrentlyActive = isDebuffTypeActive(auraManager, debuff);

            if (!IsCurrentlyActive)
            {
                Ticks = ticks;
                Occurrence = occurrence;
                MyAuraManager = auraManager;
                Debuff = debuff;
                shouldTick = useTick;
                Damage = debuff.damage;
                Target = target;

                debuff.UpdateStackCount(StackCount);

                MyID = GeneralFunctions.GenID();

                VisualEffect = SpawnVisualEffect(effect, target.transform);

                if (!shouldTick)
                {
                    Ticks = maxTicks;
                }

                firstRun = true;
                IsCurrentlyActive = true;

                StartCoroutine(DebuffTimer());
            }
            else if (refresh)
            {
                MyAuraManager = auraManager;
                Debuff = debuff;

                RefreshDebuff(false, auraManager, debuff);
            }
            else if (stack)
            {
                MyAuraManager = auraManager;
                Debuff = debuff;

                AddToStack(false, auraManager, Debuff);
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
            Debuff.UpdateStackCount(StackCount);
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
        private bool isDebuffTypeActive(AuraManager auraManager, ScriptableDebuff scriptableDebuff)
        {
            if (auraManager)
            {
                var debuff = auraManager.FindDebuffOtype(scriptableDebuff);

                return debuff;
            }
            else
            {
                Debug.LogError("Was unable to check debuff type on " + gameObject.name + "aura manager was invalid");
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
        /// Removes a value of one from the current debuff stack count
        /// </summary>
        public void RemoveFromStack(bool useIcon, AuraManager auraManager, ScriptableDebuff scriptableDebuff)
        {
            if (auraManager)
            {
                var localDebuff = auraManager.FindDebuffByID(MyID);

                localDebuff.StackCount--;

                if (useIcon)
                {
                    localDebuff.icon.UpdateStackCount(localDebuff.StackCount);
                }

                if (localDebuff.StackCount <= 0)
                {
                    if (useIcon)
                    {
                        if (localDebuff)
                        {
                            auraManager.RemoveDebuff(localDebuff.gameObject, localDebuff, localDebuff.icon);
                        }
                        else
                        {
                            var iconToRemove = auraManager.MyUIManager.FindDebuffIconByType(scriptableDebuff);

                            auraManager.MyUIManager.RemoveDebuffIcon(iconToRemove);
                        }
                    }
                    else
                    {
                        if (localDebuff)
                        {
                            auraManager.RemoveDebuff(localDebuff.gameObject, localDebuff);
                        }
                    }
                }
            }
            else
            {
                Debug.LogError("Was unable to remove debuff stack on " + gameObject.name + "aura manager was invalid");
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