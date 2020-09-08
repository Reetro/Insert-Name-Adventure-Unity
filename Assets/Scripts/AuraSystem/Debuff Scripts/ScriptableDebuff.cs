using UnityEngine;
using PlayerUI.ToolTipUI;
using AuraSystem.Effects;

namespace AuraSystem
{
    [CreateAssetMenu(fileName = "Debuff", menuName = "Create New Debuff")]
    public class ScriptableDebuff : ScriptableItem
    {
        [HideInInspector]
        public bool isNotUsingTicks = false;

        [Tooltip("This is the actual code that is called that applies the debuff")]
        public DebuffEffect debuffEffect;

        [Tooltip("Should this debuff count down using ticks")]
        public bool useTicks = true;

        [ShowIf(ShowConditions.ActionOnConditionFail.DontDraw, ShowConditions.ConditionOperator.And, nameof(useTicks))]
        [Tooltip("How many times the debuff is fired before it's removed")]
        public float ticks;

        [ShowIf(ShowConditions.ActionOnConditionFail.DontDraw, ShowConditions.ConditionOperator.And, nameof(useTicks))]
        [Tooltip("How often the debuff effect is fired off")]
        public float occurrence;

        [ShowIf(ShowConditions.ActionOnConditionFail.DontDraw, ShowConditions.ConditionOperator.And, nameof(isNotUsingTicks))]
        [Tooltip("How long the debuff should last for if 0 or below debuff will last forever")]
        public float debuffTime = 0f;

        [Tooltip("If a another debuff of this type is applied to the player should the debuff restart")]
        public bool refresh = false;

        [Tooltip("If a another debuff of this type is applied should stack count be increased")]
        public bool stack = false;

        [Tooltip("Value assigned to the debuff used for calculated debuff amount (such as damage or slow amount for example)")]
        public float debuffValue;

        [Tooltip("Visual effect that is attached to target")]
        public GameObject visualEffect;

        /// <summary>
        /// Returns the the total amount of time this debuff will last for
        /// </summary>
        /// <returns></returns>
        public float GetTotalTime()
        {
            if (useTicks)
            {
                return ticks * occurrence;
            }
            else
            {
                return debuffTime;
            }
        }
        /// <summary>
        /// Hide / Show debuffTime in editor 
        /// </summary>
        private void OnValidate()
        {
            isNotUsingTicks = !useTicks;
        }
    }
}