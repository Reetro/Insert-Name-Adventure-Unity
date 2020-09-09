using UnityEngine;
using PlayerUI.ToolTipUI;
using AuraSystem.Effects;

namespace AuraSystem
{
    [CreateAssetMenu(fileName = "Debuff", menuName = "Create New Debuff")]
    public class ScriptableDebuff : ScriptableItem
    {
        private bool isNotUsingTicks = false;

        [Tooltip("This is the actual code file that is called that applies the debuff")]
        [SerializeField] private DebuffEffect debuffEffect = null;

        [Tooltip("Should this debuff count down using ticks if false debuff will use a static timer")]
        [SerializeField] private bool useTicks = true;

        [ShowIf(ShowConditions.ActionOnConditionFail.DontDraw, ShowConditions.ConditionOperator.And, nameof(useTicks))]
        [Tooltip("How many times the debuff is fired before it's removed")]
        [SerializeField] private float ticks = 1;

        [ShowIf(ShowConditions.ActionOnConditionFail.DontDraw, ShowConditions.ConditionOperator.And, nameof(useTicks))]
        [Tooltip("How often the debuff effect is fired off")]
        [SerializeField] private float occurrence = 1;

        [ShowIf(ShowConditions.ActionOnConditionFail.DontDraw, ShowConditions.ConditionOperator.And, nameof(isNotUsingTicks))]
        [Tooltip("How long the debuff should last for if 0 or below debuff will last forever")]
        [SerializeField] private float debuffTime = 0f;

        [Tooltip("If a another debuff of this type is applied to the player should the debuff restart")]
        [SerializeField] private bool refresh = false;

        [Tooltip("If a another debuff of this type is applied should stack count be increased")]
        [SerializeField] private bool stack = true;

        [Tooltip("If true this debuff will not stack or be refreshed it will remain static")]
        [SerializeField] private bool isStatic = false;

        [Tooltip("Value assigned to the debuff used for calculate debuff amount (such as damage or slow amount for example)")]
        [SerializeField] private float debuffValue = 1;

        [Tooltip("Visual effect that is spawned and attached to target")]
        [SerializeField] private GameObject visualEffect = null;

        #region Properties
        /// <summary>
        /// Check to see if the debuff can stack
        /// </summary>
        public bool Stacking { get { return stack; } }
        /// <summary>
        /// Check to see if the debuff can be refreshed
        /// </summary>
        public bool Refreshing { get { return refresh; } }
        /// <summary>
        /// Check to see if the debuff is static
        /// </summary>
        public bool IsStatic { get { return isStatic; } }
        /// <summary>
        /// How often the debuff effect is fired off
        /// </summary>
        public float Occurrence { get { return occurrence; } }
        /// <summary>
        /// Should this debuff count down using ticks if false debuff will use a static timer
        /// </summary>
        public bool UseTicks { get { return useTicks; } }
        /// <summary>
        /// How long the debuff should last for if 0 or below debuff will last forever
        /// </summary>
        public float DebuffTime { get { return debuffTime; } }
        /// <summary>
        /// How many times the debuff is fired before it's removed
        /// </summary>
        public float Ticks { get { return ticks; } }
        /// <summary>
        /// Value assigned to the debuff used for calculate debuff amount (such as damage or slow amount for example)
        /// </summary>
        public float DebuffValue { get { return debuffValue; } }
        /// <summary>
        /// This is the actual code file that is called that applies the debuff
        /// </summary>
        public DebuffEffect CurrentDebuffEffect { get { return debuffEffect; } }
        /// <summary>
        /// Visual effect that is spawned and attached to target
        /// </summary>
        public GameObject VisualEffect { get { return visualEffect; } }
        #endregion

        /// <summary>
        /// Returns the the total amount of time this debuff will last for
        /// </summary>
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