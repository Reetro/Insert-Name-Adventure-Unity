﻿using System.Collections.Generic;
using UnityEngine;
using PlayerUI.Icons;
using PlayerUI;
using AuraSystem.Effects;
using System.Collections;

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
                if (!MyCurrentBuffs.Contains(createdBuff))
                {
                    MyCurrentBuffs.Add(createdBuff);
                }

                return createdBuff;
            }
            else
            {
                Debug.LogError("Failed to apply " + buffToApply.Name + " created buff failed to start");

                return null;
            }
        }
        /// <summary>
        /// Finds a buff of the same type as the given buff
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
        /// Removes then destroy the given buff from the the aura manager then removes the icon from the playerUI
        /// </summary>
        public void StartBuffRemoval(GameObject buffEffectObject, BuffEffect effect, BuffIcon iconToRemove)
        {
            if (buffEffectObject)
            {
                MyCurrentBuffs.Remove(effect);

                if (iconToRemove)
                {
                    var buffEffect = buffEffectObject.GetComponent<BuffEffect>();

                    if (buffEffect)
                    {
                        if (buffEffect.fadeOutAnimation)
                        {
                            buffEffect.IsCurrentlyActive = false;
                            buffEffect.IsFading = true;

                            buffEffect.fadeOutAnimation.Play();

                            StartCoroutine(DestroyBuff(buffEffect, iconToRemove));
                        }
                        else
                        {
                            MyUIManager.RemoveBuffIcon(iconToRemove);

                            Destroy(buffEffectObject.gameObject);
                        }
                    }
                    else
                    {
                        Debug.LogError("Failed to StartBuffRemoval on " + buffEffectObject.name.ToString() + " does not have a BuffEffect component");
                    }
                }
                else
                {
                    Debug.LogError("Failed to fade out " + buffEffectObject.name.ToString() + " icon was invalid");

                    Destroy(buffEffectObject.gameObject);
                }
            }
        }
        /// <summary>
        /// Removes then destroy the given debuff from the aura manager
        /// </summary>
        public void StartBuffRemoval(GameObject buffEffectObject, BuffEffect effect)
        {
            if (buffEffectObject)
            {
                Destroy(buffEffectObject);

                MyCurrentBuffs.Remove(effect);
            }
        }
        /// <summary>
        /// After the fade out animation is done destroy DebuffEffect Gameobject and removes the buff icon from screen
        /// </summary>
        private IEnumerator DestroyBuff(BuffEffect buff, BuffIcon iconToRemove)
        {
            yield return new WaitForSeconds(buff.fadeOutAnimation.GetClip("Buff_Fade_Out").length);

            MyUIManager.RemoveBuffIcon(iconToRemove);

            Destroy(buff.gameObject);
        }
        /// <summary>
        /// Will spawn the given debuff into the scene then start the debuff
        /// </summary>
        /// <returns>The applied debuff</returns>
        public DebuffEffect ApplyDebuff(GameObject target, ScriptableDebuff debuffToApply, bool createIcon)
        {
            DebuffEffect debuff = Instantiate(debuffToApply.CurrentDebuffEffect, Vector2.zero, Quaternion.identity);

            DebuffEffect staticEffect = null;

            if (!IsStaticDebuffActive(debuff, out staticEffect))
            {
                DebuffEffect createdDebuff = null;

                if (createIcon)
                {
                    var hasFillAmount = debuffToApply.GetTotalTime() > 0;

                    var debuffIcon = CreateDebuffIcon(debuffToApply, hasFillAmount);

                    createdDebuff = debuff.StartDebuff(this, debuffToApply, debuffIcon, target);
                }
                else
                {
                    createdDebuff = debuff.StartDebuff(this, debuffToApply, target);
                }

                if (createdDebuff)
                {
                    if (!MyCurrentDebuffs.Contains(createdDebuff))
                    {
                        MyCurrentDebuffs.Add(createdDebuff);
                    }

                    return createdDebuff;
                }
                else
                {
                    Debug.LogError("Failed to apply " + debuffToApply.Name + " created debuff failed to start");

                    return null;
                }
            }
            else
            {
                if (staticEffect)
                {
                    Destroy(debuff);

                    return staticEffect;
                }
                else
                {
                    Debug.LogError("Failed to apply " + debuffToApply.Name + " failed to get static debuff");

                    return null;
                }
            }
        }
        /// <summary>
        /// Check to see if a debuff of the same type is active and is a static debuff
        /// </summary>
        /// <param name="debuff"></param>
        private bool IsStaticDebuffActive(DebuffEffect debuff, out DebuffEffect activeDebuffEffect)
        {
            foreach (DebuffEffect debuffEffect in MyCurrentDebuffs)
            {
                if (debuffEffect)
                {
                    if (debuffEffect.IsStatic && debuffEffect.GetType() == debuff.GetType())
                    {
                        activeDebuffEffect = debuffEffect;
                        return true;
                    }
                }
            }
            activeDebuffEffect = null;
            return false;
        }
        /// <summary>
        /// Plays debuff fade out animation if debuff had a Icon and then destroys the DebuffEffect Gameobject
        /// </summary>
        public void StartDebuffRemoval(GameObject debuffEffectObject, DebuffIcon iconToRemove)
        {
            if (debuffEffectObject)
            {
                if (iconToRemove)
                {
                    var debuffEffect = debuffEffectObject.GetComponent<DebuffEffect>();

                    MyCurrentDebuffs.Remove(debuffEffect);

                    if (debuffEffect)
                    {
                        if (debuffEffect.fadeOutAnimation)
                        {
                            debuffEffect.IsCurrentlyActive = false;
                            debuffEffect.IsFading = true;

                            debuffEffect.fadeOutAnimation.Play();

                            StartCoroutine(DestroyDebuff(debuffEffect, iconToRemove));
                        }
                        else
                        {
                            MyUIManager.RemoveDebuffIcon(iconToRemove);

                            Destroy(debuffEffectObject.gameObject);
                        }
                    }
                    else
                    {
                        Debug.LogError("Failed to StartDebuffRemoval on " + debuffEffectObject.name.ToString() + " does not have a DebuffEffect component");
                    }
                }
                else
                {
                    Debug.LogError("Failed to fade out " + debuffEffectObject.name.ToString() + " icon was invalid");

                    Destroy(debuffEffectObject.gameObject);
                }
            }
        }
        /// <summary>
        /// Removes then destroys the given debuff from the aura manager
        /// </summary>
        public void StartDebuffRemoval(GameObject debuffEffectObject)
        {
            if (debuffEffectObject)
            {
                var effect = debuffEffectObject.GetComponent<DebuffEffect>();

                MyCurrentDebuffs.Remove(effect);

                Destroy(debuffEffectObject);
            }
        }
        /// <summary>
        /// After the fade out animation is done destroy DebuffEffect Gameobject and removes the debuff icon from screen
        /// </summary>
        private IEnumerator DestroyDebuff(DebuffEffect debuff, DebuffIcon iconToRemove)
        {
            yield return new WaitForSeconds(debuff.fadeOutAnimation.GetClip("Debuff_Fade_Out").length);

            if (iconToRemove)
            {
                MyUIManager.RemoveDebuffIcon(iconToRemove);
            }

            if (debuff)
            {
                Destroy(debuff.gameObject);
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
                    var type = debuffEffect.Debuff.GetType();

                    if (type == debuff.GetType())
                    {
                        foundDebuff = debuffEffect;
                        break;
                    }
                }

                return foundDebuff;
            }
            else
            {
                Debug.LogError("Was unable to find debuff type on " + gameObject.name + " debuff was invalid");
                return null;
            }
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
            return MyUIManager.AddDebuffIcon(debuff, hasFillAmount);
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