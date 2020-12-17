#pragma warning disable 0414
using PlayerUI.ToolTipUI;
using UnityEngine.Events;
using UnityEngine;

namespace Spells
{
    [CreateAssetMenu(fileName = "Spell", menuName = "Create New Spell")]
    public class ScriptableSpell : ScriptableItem
    {
        private bool showUseValue3 = false;
        private bool showUseValue2 = false;
        private bool showValue2 = false;
        private bool showValue3 = false;

        [Header("Spell Bools")]

        [ShowIf(ShowConditions.ActionOnConditionFail.DontDraw, ShowConditions.ConditionOperator.And, nameof(showUseValue2))]
        [Tooltip("Should this status effect have two values")]
        [SerializeField] private bool useTwoValues = false;

        [ShowIf(ShowConditions.ActionOnConditionFail.DontDraw, ShowConditions.ConditionOperator.And, nameof(showUseValue3))]
        [Tooltip("Should this status effect have two values")]
        [SerializeField] private bool useThreeValues = false;

        [Tooltip("Whether or not the spell has a cooldown")]
        [SerializeField] private bool hasCoolDown = true;

        [ShowIf(ShowConditions.ActionOnConditionFail.DontDraw, ShowConditions.ConditionOperator.And, nameof(hasCoolDown))]
        [Tooltip("Does this spell need a enemy active in the level to cooldown")]
        [SerializeField] private bool needsEnemyToCD = true;

        [Tooltip("Whether or not to spawn a particle effect on cast")]
        [SerializeField] private bool spawnParticleEffect = true;

        [Header("Effect Settings")]

        [ShowIf(ShowConditions.ActionOnConditionFail.DontDraw, ShowConditions.ConditionOperator.And, nameof(spawnParticleEffect))]
        [Tooltip("The particle system to spawn into the world")]
        [SerializeField] private GameObject particleSystemToSpawn = null;

        [ShowIf(ShowConditions.ActionOnConditionFail.DontDraw, ShowConditions.ConditionOperator.And, nameof(spawnParticleEffect))]
        [Tooltip("How long the particle system is up for")]
        [SerializeField] private float particleSystemUpTime = 1f;

        [Header("Spell Object Settings")]

        [Tooltip("The spell to spawn into the world")]
        [SerializeField] private GameObject spellToSpawn = null;

        [ShowIf(ShowConditions.ActionOnConditionFail.DontDraw, ShowConditions.ConditionOperator.And, nameof(hasCoolDown))]
        [Tooltip("How long the spell cooldown lasts")]
        [SerializeField] private float spellCoolDown = 1f;

        [Space]

        [Header("Spell Values")]

        [Tooltip("Name of the spell value")]
        [SerializeField] private string spellValue1Name = "";
        [Tooltip("This kind of depends on the spell but an example of this might be how much to damage to apply to the player")]
        [SerializeField] private float spellValue1 = 1f;

        [Space]

        [ShowIf(ShowConditions.ActionOnConditionFail.DontDraw, ShowConditions.ConditionOperator.And, nameof(showValue2))]
        [Tooltip("Name of the spell value")]
        [SerializeField] private string spellValue2Name = "";

        [ShowIf(ShowConditions.ActionOnConditionFail.DontDraw, ShowConditions.ConditionOperator.And, nameof(showValue2))]
        [Tooltip("This kind of depends on the spell but an example of this might be how much to damage to apply to the player")]
        [SerializeField] private float spellValue2 = 1f;

        [Space]

        [ShowIf(ShowConditions.ActionOnConditionFail.DontDraw, ShowConditions.ConditionOperator.And, nameof(showValue3))]
        [Tooltip("Name of the spell value")]
        [SerializeField] private string spellValue3Name = "";

        [ShowIf(ShowConditions.ActionOnConditionFail.DontDraw, ShowConditions.ConditionOperator.And, nameof(showValue3))]
        [Tooltip("This kind of depends on the spell but an example of this might be how much to damage to apply to the player")]
        [SerializeField] private float spellValue3 = 1f;

        [Header("Upgrades")]

        [Tooltip("The first spell Upgrade")]
        [SerializeField] private ScriptableSpellUpgrade spellUpgrade1 = null;

        [Tooltip("The second spell Upgrade")]
        [SerializeField] private ScriptableSpellUpgrade spellUpgrade2 = null;

        #region Properties
        /// <summary>
        /// This kind of depends on the spell but an example of this might is the player's health
        /// </summary>
        public float Value1 { get { return spellValue1; } }
        /// <summary>
        /// This kind of depends on the spell but an example of this might is the player's health
        /// </summary>
        public float Value2 { get { return spellValue2; } }
        /// <summary>
        /// his kind of depends on the spell but an example of this might is the player's health
        /// </summary>
        public float Value3 { get { return spellValue3; } }
        /// <summary>
        /// Name of the spell value
        /// </summary>
        public string Value1Name { get { return spellValue1Name; } }
        /// <summary>
        /// Name of the spell value
        /// </summary>
        public string Value2Name { get { return spellValue2Name; } }
        /// <summary>
        /// Name of the spell value
        /// </summary>
        public string Value3Name { get { return spellValue3Name; } }
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
        /// <summary>
        ///  The particle system to spawn into the world
        /// </summary>
        public GameObject ParticleSystemToSpawn { get { return particleSystemToSpawn; } }
        /// <summary>
        /// Whether or not to spawn a particle effect on cast
        /// </summary>
        public bool SpawnParticles { get { return spawnParticleEffect; } }
        /// <summary>
        /// How long the particle system is up for
        /// </summary>
        public float ParticleSystemUpTime { get { return particleSystemUpTime; } }
        /// <summary>
        /// Does this spell need a enemy active in the level to cooldown
        /// </summary>
        public bool NeedsEnemyToCooldown { get { return needsEnemyToCD; } }
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
            
            if (useTwoValues)
            {
                showValue2 = true;
            }
            else if (useThreeValues)
            {
                showValue2 = true;
            }
            else
            {
                showValue2 = false;
            }

            showValue3 = useThreeValues;
        }
        #endregion
    }
}