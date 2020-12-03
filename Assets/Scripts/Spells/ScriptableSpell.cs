using PlayerUI.ToolTipUI;
using UnityEngine.Events;
using UnityEngine;
using System;

namespace Spells
{
    [CreateAssetMenu(fileName = "Spell", menuName = "Create New Spell")]
    public class ScriptableSpell : ScriptableItem
    {
        private bool showUseValue3 = false;
        private bool showUseValue2 = false;

        [ShowIf(ShowConditions.ActionOnConditionFail.DontDraw, ShowConditions.ConditionOperator.And, nameof(showUseValue2))]
        [Tooltip("Should this status effect have two values")]
        [SerializeField] private bool useTwoValues = false;

        [ShowIf(ShowConditions.ActionOnConditionFail.DontDraw, ShowConditions.ConditionOperator.And, nameof(showUseValue3))]
        [Tooltip("Should this status effect have two values")]
        [SerializeField] private bool useThreeValues = false;

        [ShowIf(ShowConditions.ActionOnConditionFail.DontDraw, ShowConditions.ConditionOperator.And, nameof(hasCoolDown))]
        [Tooltip("How long the spell cooldown lasts")]
        [SerializeField] private float spellCoolDown = 1f;

        [Tooltip("Whether or not the spell has a cooldown")]
        [SerializeField] private bool hasCoolDown = true;

        [Tooltip("The spell to spawn into the world")]
        [SerializeField] private GameObject spellToSpawn = null;

        [Tooltip("Container for editable spell values")]
        [SerializeField] private SpellValues spellValues1 = null;

        [Tooltip("Container for editable spell values")]
        [SerializeField] private SpellValues spellValues2 = null;

        [Tooltip("Container for editable spell values")]
        [SerializeField] private SpellValues spellValues3 = null;

        #region Properties
        /// <summary>
        /// This kind of depends on the spell but an example of this might is the player's health
        /// </summary>
        public float Value1 { get { return spellValues1.SpellValue; } }
        /// <summary>
        /// This kind of depends on the spell but an example of this might is the player's health
        /// </summary>
        public float Value2 { get { return spellValues2.SpellValue; } }
        /// <summary>
        /// his kind of depends on the spell but an example of this might is the player's health
        /// </summary>
        public float Value3 { get { return spellValues3.SpellValue; } }

        /// <summary>
        /// Check to see if this spell is using two values
        /// </summary>
        public bool UsingTwoValues { get { return useTwoValues; } }
        /// <summary>
        /// The spell to spawn into the world
        /// </summary>
        public GameObject SpellToSpawn { get { return spellToSpawn; } }
        /// <summary>
        /// How long the spell cooldown lasts
        /// </summary>
        public float SpellCoolDown { get { return spellCoolDown; } }
        /// <summary>
        /// The spell to spawn into the world
        /// </summary>
        public bool HasCoolDown { get { return hasCoolDown; } }
        /// <summary>
        /// Check to see if this spell is using three values
        /// </summary>
        public bool UsingThreeValues { get { return useThreeValues; } }
        #endregion

        #region Spell Events
        [System.Serializable]
        public class OnSpellWasCast : UnityEvent<GameObject> { }

        [HideInInspector]
        [Tooltip("Called when a spell is casted")]
        public OnSpellWasCast OnSpellCast;

        [HideInInspector]
        [Tooltip("Called when a spell cast has ended")]
        public UnityEvent OnSpellCastEnd;

        [HideInInspector]
        [Tooltip("Called when a spell cooldown has ended")]
        public UnityEvent OnSpellCooldownEnd;
        #endregion

        #region Spell Functions
        private void OnValidate()
        {
            hasArtwork = true;

            if (useThreeValues)
            {
                useTwoValues = false;
            }

            showUseValue3 = !useTwoValues;
            showUseValue2 = !useThreeValues;
        }
        #endregion
    }

    [Serializable]
    public class SpellValues
    {
        [Tooltip("Name of the spell value")]
        public string SpellValueName = "";
        [Tooltip("This kind of depends on the spell but an example of this might be how much to damage to apply to the player")]
        public float SpellValue = 1f;
    }
}