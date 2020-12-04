﻿using UnityEngine;
using UnityEngine.Events;
using PlayerUI.ToolTipUI;

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

        [Tooltip("Should this status effect have two values")]
        [SerializeField] private bool useTwoValues = false;

        [ShowIf(ShowConditions.ActionOnConditionFail.DontDraw, ShowConditions.ConditionOperator.And, nameof(useTicks))]
        [Tooltip("How many times the status effect is fired before it's removed if zero or below status effect will tick forever")]
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

        [Space]

        [Tooltip("Name of the status effect value")]
        [SerializeField] private string value1Name = "";
        [Tooltip("This kind of depends on the status effect but an example of this might be how much to damage to apply to the player")]
        [SerializeField] private float value1 = 1;

        [Space]

        [ShowIf(ShowConditions.ActionOnConditionFail.DontDraw, ShowConditions.ConditionOperator.And, nameof(useTwoValues))]
        [Tooltip("Name of the status effect value")]
        [SerializeField] private string value2Name = "";

        [ShowIf(ShowConditions.ActionOnConditionFail.DontDraw, ShowConditions.ConditionOperator.And, nameof(useTwoValues))]
        [Tooltip("This kind of depends on the status effect but an example of this might be how much to damage to apply to the player")]
        [SerializeField] private float value2 = 1;

        [HideInInspector]
        [Tooltip("Called when the Status Effect Ends")]
        public UnityEvent OnEffectEnd;

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
        /// Check to see if this status effect is using two values
        /// </summary>
        public bool UsingTwoValues { get { return useTwoValues; } }
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
        public float Value1 { get { return value1; } }
        /// <summary>
        /// This kind of depends on the status effect but an example of this might is the player's health
        /// </summary>
        public float Value2 { get { return value2; } }
        /// <summary>
        /// Name of the status effect value
        /// </summary>
        public string Value1Name { get { return value1Name; } }
        /// <summary>
        /// Name of the status effect value
        /// </summary>
        public string Value2Name { get { return value2Name; } }
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
