using PlayerUI.ToolTipUI;
using UnityEngine.Events;
using UnityEngine;

namespace Spells
{
    [CreateAssetMenu(fileName = "Spell", menuName = "Create New Spell")]
    public class ScriptableSpell : ScriptableItem
    {
        [Tooltip("Should this status effect have two values")]
        [SerializeField] private bool useTwoValues = false;

        [Tooltip("This kind of depends on the spell but an example of this might be how much to damage to apply to the player")]
        [SerializeField] private float value = 1;

        [ShowIf(ShowConditions.ActionOnConditionFail.DontDraw, ShowConditions.ConditionOperator.And, nameof(useTwoValues))]
        [Tooltip("This kind of depends on the spell but an example of this might be how much to damage to apply to the player")]
        [SerializeField] private float value2 = 1;

        [ShowIf(ShowConditions.ActionOnConditionFail.DontDraw, ShowConditions.ConditionOperator.And, nameof(hasCoolDown))]
        [Tooltip("How long the spell cooldown lasts")]
        [SerializeField] private float spellCoolDown = 1f;

        [Tooltip("Whether or not the spell has a cooldown")]
        [SerializeField] private bool hasCoolDown = true;

        [Tooltip("The spell to spawn into the world")]
        [SerializeField] private GameObject spellToSpawn = null;

        #region Properties
        /// <summary>
        /// This kind of depends on the status effect but an example of this might is the player's health
        /// </summary>
        public float Value1 { get { return value; } }
        /// <summary>
        /// This kind of depends on the status effect but an example of this might is the player's health
        /// </summary>
        public float Value2 { get { return value2; } }
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
        }
        #endregion
    }
}