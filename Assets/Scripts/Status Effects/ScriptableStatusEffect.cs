using UnityEngine;
using PlayerUI.ToolTipUI;
using StatusEffects.Effects;

namespace StatusEffects
{
    [CreateAssetMenu(fileName = "Status Effect", menuName = "Create New Status Effect")]
    [System.Serializable]
    public class ScriptableStatusEffect : ScriptableItem
    {
        private bool isNotUsingTicks = false;

        [Tooltip("This is the actual code file that is called that applies the status effect")]
        [SerializeField] private GameObject Effect = null;

        [Tooltip("Should this status effect count down using ticks if false status effect will use a static timer")]
        [SerializeField] private bool useTicks = true;

        [ShowIf(ShowConditions.ActionOnConditionFail.DontDraw, ShowConditions.ConditionOperator.And, nameof(useTicks))]
        [Tooltip("How many times the status effect is fired before it's removed if zero or below BuffEffect will tick forever")]
        [SerializeField] private float ticks = 1;

        [ShowIf(ShowConditions.ActionOnConditionFail.DontDraw, ShowConditions.ConditionOperator.And, nameof(useTicks))]
        [Tooltip("How often the status effect effect is fired off")]
        [SerializeField] private float occurrence = 1;

        [ShowIf(ShowConditions.ActionOnConditionFail.DontDraw, ShowConditions.ConditionOperator.And, nameof(isNotUsingTicks))]
        [Tooltip("How long the status effect should last for if zero or below the status effect will last forever")]
        [SerializeField] private float duration = 1;

        [Tooltip("If a another status effect of this type is applied to the player should the status effect restart")]
        [SerializeField] private bool refresh = false;

        [Tooltip("If a another status effect of this type is applied should stack count be increased")]
        [SerializeField] private bool stack = true;

        [Tooltip("If true this status effect will not stack or be refreshed it will remain static")]
        [SerializeField] private bool isStatic = false;

        [Tooltip("This kind of depends on the status effect but an example of this might is the player's health")]
        [SerializeField] private float value = 1;

        #region Properties
        /// <summary>
        /// This is the actual code file that is called that applies the status effect
        /// </summary>
        public GameObject CurrentStatusEffect { get { return Effect; } }
        /// <summary>
        /// If a another status effect of this type is applied to the player should the status effect restart
        /// </summary>
        public bool Refreshing { get { return refresh; } }
        /// <summary>
        /// If a another status effect of this type is applied should stack count be increased
        /// </summary>
        public bool Stacking { get { return stack; } }
        /// <summary>
        /// Should this status effect count down using ticks if false status effect will use a static timer
        /// </summary>
        public bool UseTicks { get { return useTicks; } }
        /// <summary>
        /// If true this status effect will not stack or be refreshed it will remain static
        /// </summary>
        public bool IsStatic { get { return isStatic; } }
        /// <summary>
        /// This kind of depends on the status effect but an example of this might is the player's health
        /// </summary>
        public float Value { get { return value; } }
        /// <summary>
        /// How many times the status effect is fired before it's removed
        /// </summary>
        public float Ticks { get { return ticks; } }
        /// <summary>
        /// How often the status effect effect is fired off
        /// </summary>
        public float Occurrence { get { return occurrence; } }
        #endregion

        #region Status Effect Functions
        /// <summary>
        /// Returns the the total amount of time this status effect will last for
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
            hasArtwork = false;

            isNotUsingTicks = !useTicks;
        }
        #endregion
    }
}
