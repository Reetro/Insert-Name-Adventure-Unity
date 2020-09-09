using UnityEngine;
using PlayerUI.ToolTipUI;
using AuraSystem.Effects;

namespace AuraSystem
{
    [CreateAssetMenu(fileName = "Buff", menuName = "Create New Buff")]
    public class ScriptableBuff : ScriptableItem
    {
        private bool isNotUsingTicks = false;

        [Tooltip("This is the actual code file that is called that applies the buff")]
        [SerializeField] private BuffEffect buffEffect = null;

        [Tooltip("Should this buff count down using ticks if false buff will use a static timer")]
        [SerializeField] private bool useTicks = true;

        [ShowIf(ShowConditions.ActionOnConditionFail.DontDraw, ShowConditions.ConditionOperator.And, nameof(useTicks))]
        [Tooltip("How many times the buff is fired before it's removed if zero or below buff will tick forever")]
        [SerializeField] private float ticks = 1;

        [ShowIf(ShowConditions.ActionOnConditionFail.DontDraw, ShowConditions.ConditionOperator.And, nameof(useTicks))]
        [Tooltip("How often the buff effect is fired off")]
        [SerializeField] private float occurrence = 1;

        [ShowIf(ShowConditions.ActionOnConditionFail.DontDraw, ShowConditions.ConditionOperator.And, nameof(isNotUsingTicks))]
        [Tooltip("How long the buff should last for if zero or below debuff will last forever")]
        [SerializeField] private float duration = 1;

        [Tooltip("If a another buff of this type is applied to the player should the buff restart")]
        [SerializeField] private bool refresh = false;

        [Tooltip("If a another buff of this type is applied should stack count be increased")]
        [SerializeField] private bool stack = false;

        [Tooltip("If true this buff will not stack or be refreshed it will remain static")]
        [SerializeField] private bool isStatic = false;

        [Tooltip("This kind of depends on the buff but an example of this might is the player's health")]
        [SerializeField] private float buffValue = 1;

        [Tooltip("Visual effect that is attached to target")]
        [SerializeField] private GameObject visualEffect = null;

        #region Properties
        /// <summary>
        /// This is the actual code file that is called that applies the buff
        /// </summary>
        public BuffEffect CurrentBuffEffect { get { return buffEffect; } }
        /// <summary>
        /// If a another buff of this type is applied to the player should the buff restart
        /// </summary>
        public bool Refreshing { get { return refresh; } }
        /// <summary>
        /// If a another buff of this type is applied should stack count be increased
        /// </summary>
        public bool Stacking { get { return stack; } }
        /// <summary>
        /// Should this buff count down using ticks if false buff will use a static timer
        /// </summary>
        public bool UseTicks { get { return useTicks; } }
        /// <summary>
        /// If true this buff will not stack or be refreshed it will remain static
        /// </summary>
        public bool IsStatic { get { return isStatic; } }
        /// <summary>
        /// This kind of depends on the buff but an example of this might is the player's health
        /// </summary>
        public float BuffValue { get { return buffValue; } }
        /// <summary>
        /// How many times the buff is fired before it's removed
        /// </summary>
        public float Ticks { get { return ticks; } }
        /// <summary>
        /// How often the buff effect is fired off
        /// </summary>
        public float Occurrence { get { return occurrence; } }
        /// <summary>
        /// Visual effect that is attached to target
        /// </summary>
        public GameObject VisualEffect { get { return visualEffect; } }
        #endregion

        #region Buff Functions
        /// <summary>
        /// Returns the the total amount of time this buff will last for
        /// </summary>
        public float GetTotalTime()
        {
            if (useTicks)
            {
                return ticks * occurrence;
            }
            else
            {
                return duration;
            }
        }

        private void OnValidate()
        {
            isNotUsingTicks = !useTicks;
        }
        #endregion
    }
}