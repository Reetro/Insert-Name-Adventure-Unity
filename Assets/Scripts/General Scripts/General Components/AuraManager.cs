using System.Collections.Generic;
using UnityEngine;
using PlayerUI.Icons;
using PlayerUI;
using AuraSystem.Effects;

namespace AuraSystem
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
        /// <summary>
        /// Will spawn the given buff into the scene then start the buff
        /// </summary>
        /// <returns>The applied buff</returns>
        public BuffEffect ApplyBuff(GameObject target, ScriptableBuff buffToApply, bool createIcon)
        {
            BuffEffect buff = Instantiate(buffToApply.buffEffect, Vector2.zero, Quaternion.identity);

            BuffEffect createdBuff = null;

            if (createIcon)
            {
                var buffIcon = CreateBuffIcon(buffToApply);

                createdBuff = buff.StartBuff(buffToApply.buffAmount, buffToApply.duration, this, buffToApply, buffIcon, target, buffToApply.visualEffect, buffToApply.stack, buffToApply.refresh);
            }
            else
            {
                createdBuff = buff.StartBuff(buffToApply.buffAmount, buffToApply.duration, this, buffToApply, target, buffToApply.visualEffect, buffToApply.stack, buffToApply.refresh);
            }

            if (createdBuff)
            {
                MyCurrentBuffs.Add(createdBuff);

                return createdBuff;
            }
            else
            {
                Debug.LogError("Failed to apply " + buffToApply.Name + " created buff failed to start");

                return null;
            }
        }
        /// <summary>
        /// Finds a debuff of the same type as the given debuff
        /// </summary>
        /// <returns>A BuffEffect</returns>
        public BuffEffect FindBuffOfType(ScriptableBuff buff)
        {
            if (buff)
            {
                BuffEffect foundBuff = null;

                foreach (BuffEffect buffEffect in MyCurrentBuffs)
                {
                    var type = buffEffect.Buff.buffEffect;

                    if (type == buff.buffEffect)
                    {
                        foundBuff = buffEffect;
                        break;
                    }
                    else
                    {
                        foundBuff = null;
                        continue;
                    }
                }

                return foundBuff;
            }
            else
            {
                Debug.LogError("Was unable to find buff type on " + gameObject.name + "buff was invalid");
                return null;
            }
        }
        /// <summary>
        /// Finds a specific buff with the given ID 
        /// </summary>
        /// <returns>A BuffEffect</returns>
        public BuffEffect FindBuffByID(int ID)
        {
            BuffEffect foundBuff = null;

            foreach (BuffEffect buffEffect in MyCurrentBuffs)
            {
                var localID = buffEffect.MyID;

                if (localID == ID)
                {
                    foundBuff = buffEffect;
                    break;
                }
                else
                {
                    foundBuff = null;
                    continue;
                }
            }

            return foundBuff;
        }
        /// <summary>
        /// Removes then destroy the given buff from the the aura manager then removes the icon from the playerUI
        /// </summary>
        public void RemoveBuff(GameObject buffEffectObject, BuffEffect effect, BuffIcon iconToRemove)
        {
            if (buffEffectObject)
            {
                Destroy(buffEffectObject);

                MyCurrentBuffs.Remove(effect);
            }

            if (iconToRemove)
            {
                MyUIManager.RemoveBuffIcon(iconToRemove);
            }
            else
            {
                Debug.LogError("Failed to remove " + buffEffectObject.name + "buff Icon is invalid");
            }
        }
        /// <summary>
        /// Removes then destroy the given debuff from the aura manager
        /// </summary>
        public void RemoveBuff(GameObject buffEffectObject, BuffEffect effect)
        {
            if (buffEffectObject)
            {
                Destroy(buffEffectObject);

                MyCurrentBuffs.Remove(effect);
            }
        }
        /// <summary>
        /// Will spawn the given debuff into the scene then start the debuff
        /// </summary>
        /// <returns>The applied debuff</returns>
        public DebuffEffect ApplyDebuff(GameObject target, ScriptableDebuff debuffToApply, bool createIcon)
        {
            DebuffEffect debuff = Instantiate(debuffToApply.debuffEffect, Vector2.zero, Quaternion.identity);

            DebuffEffect createdDebuff = null;

            if (createIcon)
            {
                var debuffIcon = CreateDebuffIcon(debuffToApply, debuffToApply.useTicks);

                createdDebuff = debuff.StartDebuff(debuffToApply.ticks, debuffToApply.occurrence, this, debuffToApply, debuffIcon, target, debuffToApply.visualEffect, debuffToApply.useTicks, debuffToApply.refresh, debuffToApply.stack);
            }
            else
            {
                createdDebuff = debuff.StartDebuff(debuffToApply.ticks, debuffToApply.occurrence, this, debuffToApply, target, debuffToApply.visualEffect, debuffToApply.useTicks, debuffToApply.refresh, debuffToApply.stack);
            }

            if (createdDebuff)
            {
                MyCurrentDebuffs.Add(createdDebuff);

                return createdDebuff;
            }
            else
            {
                Debug.LogError("Failed to apply " + debuffToApply.Name + " created debuff failed to start");

                return null;
            }
        }
        /// <summary>
        /// Removes then destroy the given debuff from the aura manager Gameobject then removes the icon from the playerUI
        /// </summary>
        public void RemoveDebuff(GameObject debuffEffectObject, DebuffEffect effect, DebuffIcon iconToRemove)
        {
            if (debuffEffectObject)
            {
                Destroy(debuffEffectObject);

                MyCurrentDebuffs.Remove(effect);
            }

            if (iconToRemove)
            {
                MyUIManager.RemoveDebuffIcon(iconToRemove);
            }
            else
            {
                Debug.LogError("Failed to remove " + debuffEffectObject.name + "debuff Icon is invalid");
            }
        }
        /// <summary>
        /// Removes then destroy the given debuff from the aura manager
        /// </summary>
        public void RemoveDebuff(GameObject debuffEffectObject, DebuffEffect effect)
        {
            if (debuffEffectObject)
            {
                Destroy(debuffEffectObject);

                MyCurrentDebuffs.Remove(effect);
            }

            if (debuffEffectObject)
            {
                Destroy(debuffEffectObject);

                MyCurrentDebuffs.Remove(effect);
            }
        }
        /// <summary>
        /// Finds a debuff of the same type as the given debuff
        /// </summary>
        /// <returns>A DebuffEffect</returns>
        public DebuffEffect FindDebuffOtype(ScriptableDebuff debuff)
        {
            if (debuff)
            {
                DebuffEffect foundDebuff = null;

                foreach (DebuffEffect debuffEffect in MyCurrentDebuffs)
                {
                    var type = debuffEffect.Debuff.debuffType;

                    if (type == debuff.debuffType)
                    {
                        foundDebuff = debuffEffect;
                        break;
                    }
                    else
                    {
                        foundDebuff = null;
                        continue;
                    }
                }

                return foundDebuff;
            }
            else
            {
                Debug.LogError("Was unable to find debuff type on " + gameObject.name + "debuff was invalid");
                return null;
            }
        }
        /// <summary>
        /// Finds a specific debuff with the given ID 
        /// </summary>
        /// <returns>A DebuffEffect</returns>
        public DebuffEffect FindDebuffByID(int ID)
        {
            DebuffEffect foundDebuff = null;

            foreach (DebuffEffect debuffEffect in MyCurrentDebuffs)
            {
                var localID = debuffEffect.MyID;

                if (localID == ID)
                {
                    foundDebuff = debuffEffect;
                    break;
                }
                else
                {
                    foundDebuff = null;
                    continue;
                }
            }

            return foundDebuff;
        }
        /// <summary>
        /// Will add a buff icon to the player UI
        /// </summary>
        /// <returns>The created icon</returns>
        private BuffIcon CreateBuffIcon(ScriptableBuff buff)
        {
            return MyUIManager.AddBuffIcon(buff);
        }
        /// <summary>
        /// Will add a debuff icon to the player UI
        /// </summary>
        /// <returns>The created icon</returns>
        private DebuffIcon CreateDebuffIcon(ScriptableDebuff debuff, bool hasFillAmount)
        {
            return MyUIManager.AddDebuffIcon(debuff, hasFillAmount, debuff.useTicks);
        }
        /// <summary>
        /// Gets all current buffs on this Gameobject
        /// </summary>
        public List<BuffEffect> MyCurrentBuffs { get; } = new List<BuffEffect>();
        /// <summary>
        /// Gets all current debuffs on this Gameobject
        /// </summary>
        public List<DebuffEffect> MyCurrentDebuffs { get; } = new List<DebuffEffect>();
        /// <summary>
        /// Gets the player UI Manager
        /// </summary>
        public PlayerUIManager MyUIManager { get; private set; } = null;
    }
}