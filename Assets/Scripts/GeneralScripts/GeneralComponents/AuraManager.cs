using System.Collections.Generic;
using System.Linq;
using GeneralScripts.UI;
using StatusEffects;
using StatusEffects.Effects;
using UnityEngine;

namespace GeneralScripts.GeneralComponents
{
    public class AuraManager : MonoBehaviour
    {
        /// <summary>
        /// Sets this aura manager internal UI manager to the given object
        /// </summary>
        /// <param name="playerUIManager"></param>
        public void SetUIManager(PlayerUIManager playerUIManager)
        {
            MyUIManager = playerUIManager;
        }

        #region Status Effect Functions
        /// <summary>
        /// Will spawn the given status effect into the scene then start the status effect
        /// </summary>
        /// <returns>The applied status effect</returns>
        public StatusEffect ApplyEffect(GameObject target, ScriptableStatusEffect effectToApply)
        {
            GameObject spawnedEffect = Instantiate(effectToApply.CurrentStatusEffect, Vector2.zero, Quaternion.identity);

            StatusEffect effect = spawnedEffect.GetComponent<StatusEffect>();

            if (effect)
            {
                if (!IsStatusEffectActive(out var staticEffect))
                {
                    StatusEffect createdEffect = effect.StartStatusEffect(this, effectToApply, target);

                    if (createdEffect)
                    {
                        if (!MyCurrentStatusEffects.Contains(createdEffect))
                        {
                            MyCurrentStatusEffects.Add(createdEffect);
                        }

                        return createdEffect;
                    }
                    else
                    {
                        Debug.LogError("Failed to apply " + effectToApply.Name + " created status effect failed to start");

                        return null;
                    }
                }
                else
                {
                    if (staticEffect)
                    {
                        Destroy(staticEffect);

                        return staticEffect;
                    }
                    else
                    {
                        Debug.LogError("Failed to apply " + effectToApply.Name + " failed to get static status effect");

                        return null;
                    }
                }
            }
            else
            {
                Debug.LogError("Failed to spawn status effect " + spawnedEffect.name + " does not have a status effect component");

                Destroy(spawnedEffect);

                return null;
            }
        }
        /// <summary>
        /// Check to see if a status of the same type is active and is a static status effect
        /// </summary>
        private bool IsStatusEffectActive(out StatusEffect activeEffect)
        {
            // ReSharper disable once EqualExpressionComparison
            foreach (var currentStatusEffect in MyCurrentStatusEffects.Where(currentStatusEffect => currentStatusEffect).Where(currentStatusEffect => currentStatusEffect.IsStatic && currentStatusEffect.GetType() == currentStatusEffect.GetType()))
            {
                activeEffect = currentStatusEffect;
                return true;
            }
            activeEffect = null;
            return false;
        }
        /// <summary>
        /// Finds a status effect of the same type as the given status effect
        /// </summary>
        /// <returns>A StatusEffectBase</returns>
        public StatusEffect FindStatusEffectOfType(ScriptableStatusEffect scriptableStatusEffect)
        {
            if (scriptableStatusEffect)
            {
                return (from statusEffect in MyCurrentStatusEffects let type = statusEffect.CurrentStatusEffect.GetType() where type == scriptableStatusEffect.GetType() select statusEffect).FirstOrDefault();
            }
            else
            {
                Debug.LogError("Was unable to find buff type on " + gameObject.name + "buff was invalid");
                return null;
            }
        } 
        /// <summary>
        /// Removes then destroy the given status effect from the aura manager
        /// </summary>
        /// <param name="currentEffectObject"></param>
        /// <param name="effect"></param>
        public void StartStatusEffectRemoval(GameObject currentEffectObject, StatusEffect effect)
        {
            if (currentEffectObject)
            {
                MyCurrentStatusEffects.Remove(effect);

                Destroy(currentEffectObject);
            }
            else
            {
                Debug.LogError("Failed to remove effect " + effect.name + " Effect Object was not valid");
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the player UI Manager
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public PlayerUIManager MyUIManager { get; private set; }
        /// <summary>
        /// Gets all current status effects on this Gameobject
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        public List<StatusEffect> MyCurrentStatusEffects { get; } = new List<StatusEffect>();
        #endregion
    }
}