#pragma warning disable 0414
using GameplayScripts;
using GeneralScripts.CustomEditors;
using UnityEngine;

namespace Spells
{
    [CreateAssetMenu(fileName = "Spell Upgrade", menuName = "Create New Spell Upgrade")]
    public class ScriptableSpellUpgrade : ScriptableItem
    {
        private bool showFloatValue2;
        private bool showFloatValue3;
        private bool showValue2;
        private bool showValue3;

        [Header("Upgrade Booleans")]

        [Tooltip("Should this upgrade use float modifiers")]
        [SerializeField] private bool useFloatModifiers = true;

        [ShowIf(ShowConditions.ActionOnConditionFail.DontDraw, ShowConditions.ConditionOperator.And, nameof(showValue3))]
        [Tooltip("Should this upgrade use two float modifiers")]
        [SerializeField] private bool useFloatTwoModifiers = true;

        [ShowIf(ShowConditions.ActionOnConditionFail.DontDraw, ShowConditions.ConditionOperator.And, nameof(showValue2))]
        [Tooltip("Should this upgrade use three float modifiers")]
        [SerializeField] private bool useFloatThreeModifiers = true;

        [Space]

        [ShowIf(ShowConditions.ActionOnConditionFail.DontDraw, ShowConditions.ConditionOperator.And, nameof(useFloatModifiers))]
        [Tooltip("Name of the first float modifier")]
        // ReSharper disable once NotAccessedField.Local
        [SerializeField] private string modifier1Name = "";

        [ShowIf(ShowConditions.ActionOnConditionFail.DontDraw, ShowConditions.ConditionOperator.And, nameof(useFloatModifiers))]
        [Tooltip("The first float spell modifier")]
        // ReSharper disable once RedundantDefaultMemberInitializer
        [SerializeField] private float modifier1 = 0;

        [Space]

        [ShowIf(ShowConditions.ActionOnConditionFail.DontDraw, ShowConditions.ConditionOperator.And, nameof(showFloatValue2))]
        [Tooltip("Name of the second float modifier")]
        // ReSharper disable once NotAccessedField.Local
        [SerializeField] private string modifier2Name = "";

        [ShowIf(ShowConditions.ActionOnConditionFail.DontDraw, ShowConditions.ConditionOperator.And, nameof(showFloatValue2))]
        [Tooltip("The second spell float modifier")]
        // ReSharper disable once RedundantDefaultMemberInitializer
        [SerializeField] private float modifier2 = 0;

        [Space]

        [ShowIf(ShowConditions.ActionOnConditionFail.DontDraw, ShowConditions.ConditionOperator.And, nameof(showFloatValue3))]
        [Tooltip("Name of the third float modifier")]
        // ReSharper disable once NotAccessedField.Local
        [SerializeField] private string modifier3Name = "";

        [ShowIf(ShowConditions.ActionOnConditionFail.DontDraw, ShowConditions.ConditionOperator.And, nameof(showFloatValue3))]
        [Tooltip("The third spell float modifier")]
        // ReSharper disable once RedundantDefaultMemberInitializer
        [SerializeField] private float modifier3 = 0f;

        #region Properties
        /// <summary>
        /// The value of the first float modifier
        /// </summary>
        public float SpellFloatModifier1 => modifier1;

        /// <summary>
        /// The value of the second float modifier
        /// </summary>
        public float SpellFloatModifier2 => modifier2;

        /// <summary>
        /// The value of the third float modifier
        /// </summary>
        public float SpellFloatModifier3 => modifier3;

        /// <summary>
        /// Checks to see if this upgrade is using float modifiers
        /// </summary>
        public bool UseFloatModifiers => useFloatModifiers;

        /// <summary>
        /// Checks to see if this upgrade is using two float modifiers
        /// </summary>
        public bool UseFloatTwoModifiers => useFloatTwoModifiers;

        /// <summary>
        /// Checks to see if this upgrade is using three float modifiers
        /// </summary>
        public bool UseFloatThreeModifiers => useFloatThreeModifiers;

        #endregion

        #region Spell Upgrade Functions
        private void OnValidate()
        {
            hasArtwork = false;

            if (useFloatThreeModifiers)
            {
                useFloatTwoModifiers = false;
            }

            showValue2 = !useFloatTwoModifiers;
            showValue3 = !useFloatThreeModifiers;

            if (useFloatTwoModifiers)
            {
                showFloatValue2 = true;
            }
            else if (useFloatThreeModifiers)
            {
                showFloatValue2 = true;
            }
            else if (!useFloatModifiers)
            {
                showFloatValue2 = false;
            }
            else
            {
                showFloatValue2 = false;
            }

            if (!useFloatModifiers)
            {
                showValue2 = false;
                showValue3 = false;

                useFloatThreeModifiers = false;
                useFloatTwoModifiers = false;
            }

            showFloatValue3 = useFloatThreeModifiers;
        }
        #endregion
    }
}