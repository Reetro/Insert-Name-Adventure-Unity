using PlayerUI.ToolTipUI;
using AuraSystem.Effects;
using UnityEngine;

namespace AuraSystem
{
    [CreateAssetMenu(fileName = "Aura", menuName = "Create New Aura")]
    public class ScriptableAura : ScriptableItem
    {
        private bool isNotUsingTicks = false;

        [Tooltip("This is the actual code file that is called that applies the aura")]
        [SerializeField] private BuffEffect auraEffect = null;

        [Tooltip("Should this aura count down using ticks if false aura will use a static timer")]
        [SerializeField] private bool useTicks = true;

        [ShowIf(ShowConditions.ActionOnConditionFail.DontDraw, ShowConditions.ConditionOperator.And, nameof(useTicks))]
        [Tooltip("How many times the aura is fired before it's removed if zero or below aura will tick forever")]
        [SerializeField] private float ticks = 1;

        [ShowIf(ShowConditions.ActionOnConditionFail.DontDraw, ShowConditions.ConditionOperator.And, nameof(useTicks))]
        [Tooltip("How often the aura effect is fired off")]
        [SerializeField] private float occurrence = 1;

        [ShowIf(ShowConditions.ActionOnConditionFail.DontDraw, ShowConditions.ConditionOperator.And, nameof(isNotUsingTicks))]
        [Tooltip("How long the aura should last for if zero or below aura will last forever")]
        [SerializeField] private float duration = 1;

        [Tooltip("If a another aura of this type is applied to the player should the aura restart")]
        [SerializeField] private bool refresh = false;

        [Tooltip("If a another aura of this type is applied should stack count be increased")]
        [SerializeField] private bool stack = false;

        [Tooltip("If true this aura will not stack or be refreshed it will remain static")]
        [SerializeField] private bool isStatic = false;

        [Tooltip("This kind of depends on the aura but an example of this might is the player's health")]
        [SerializeField] private float auraValue = 1;

        [Tooltip("Visual effect that is attached to target")]
        [SerializeField] private GameObject visualEffect = null;

        #region Properties
        /// <summary>
        /// This is the actual code file that is called that applies the aura
        /// </summary>
        public BuffEffect CurrentBuffEffect { get { return auraEffect; } }
        /// <summary>
        /// If a another aura of this type is applied to the player should the aura restart
        /// </summary>
        public bool Refreshing { get { return refresh; } }
        /// <summary>
        /// If a another aura of this type is applied should stack count be increased
        /// </summary>
        public bool Stacking { get { return stack; } }
        /// <summary>
        /// Should this aura count down using ticks if false aura will use a static timer
        /// </summary>
        public bool UseTicks { get { return useTicks; } }
        /// <summary>
        /// If true this aura will not stack or be refreshed it will remain static
        /// </summary>
        public bool IsStatic { get { return isStatic; } }
        /// <summary>
        /// This kind of depends on the aura but an example of this might is the player's health
        /// </summary>
        public float BuffValue { get { return auraValue; } }
        /// <summary>
        /// How many times the aura is fired before it's removed
        /// </summary>
        public float Ticks { get { return ticks; } }
        /// <summary>
        /// How often the aura effect is fired off
        /// </summary>
        public float Occurrence { get { return occurrence; } }
        /// <summary>
        /// Visual effect that is attached to target
        /// </summary>
        public GameObject VisualEffect { get { return visualEffect; } }
        #endregion

        #region Aura Functions
        /// <summary>
        /// Returns the the total amount of time this aura will last for
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