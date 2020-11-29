using UnityEngine;

namespace Spells
{
    public abstract class Spell : MonoBehaviour
    {
        #region Local Vars
        private float defaultTimer = 0f;
        /// <summary>
        /// Will start the spell cooldown timer
        /// </summary>
        protected bool startCooldownTimer = false;
        /// <summary>
        /// Checks to see if the spell is currently on cooldown
        /// </summary>
        protected bool onCooldown = false;
        #endregion

        #region Setup Functions
        /// <summary>
        /// Sets all spell values and casts the spell
        /// </summary>
        public void StartSpellCast(ScriptableSpell spellInfo, GameObject spawnedSpell)
        {
            var spell = spawnedSpell.GetComponent<Spell>();

            if (spell)
            {
                if (CanSpellBeCasted(spellInfo, spell))
                {
                    CastSpell();
                }
                else
                {
                    Destroy(spawnedSpell);
                }
            }
        }
        /// <summary>
        /// The actual event that casts the spell
        /// </summary>
        protected abstract void CastSpell();
        /// <summary>
        /// Set all local values
        /// </summary>
        private void UnpackSpell(bool useTwoValues, float value1, float value2, ScriptableSpell scriptableSpell, float cooldown, bool hasCooldown)
        {
            UsingTwoValues = useTwoValues;
            Value1 = value1;
            Value2 = value2;
            HasCoolDown = hasCooldown;
            SpellCoolDown = cooldown;
            defaultTimer = cooldown;

            SpellInfo = scriptableSpell;

            OnUpackSpellValuesDone();
        }
        /// <summary>
        /// Called after spell has unpacked all need values
        /// </summary>
        protected abstract void OnUpackSpellValuesDone();
        #endregion

        #region Spell Logic Functions
        /// <summary>
        /// Checks to see if the spell exists in the world or is on cooldown if not will setup all spell values
        /// </summary>
        private bool CanSpellBeCasted(ScriptableSpell spellToCast, Spell spell)
        {
            if (!DoesSpellOfTypeExist(spell))
            {
                if (!onCooldown)
                {
                    MyID = GeneralFunctions.GenID();

                    UnpackSpell(spellToCast.UsingTwoValues, spellToCast.Value1, spellToCast.Value2, spellToCast, spellToCast.SpellCoolDown, spellToCast.HasCoolDown);

                    return true;
                }
                else
                {
                    return false;
                };
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets all spells in the current scene and checks to see if the given spell exists in the world
        /// </summary>
        /// <param name="spell"></param>
        public Spell DoesSpellOfTypeExist(Spell spell)
        {
            if (spell)
            {
                var spells = FindObjectsOfType<Spell>();

                Spell foundSpell = null;

                foreach (Spell currentSpell in spells)
                {
                    var type = currentSpell.GetType();

                    if (type == spell.GetType() && currentSpell != spell)
                    {
                        foundSpell = currentSpell;
                        break;
                    }
                }

                return foundSpell;
            }
            else
            {
                Debug.LogError("Was unable to find spell type on " + gameObject.name + " spell was invalid");
                return null;
            }
        }
        #endregion

        #region Cooldown Functions
        /// <summary>
        /// Place the spell on cooldown
        /// </summary>
        protected void StartCoolDown()
        {
            // TODO Add cooldown image

            onCooldown = true;
            startCooldownTimer = true;
        }
        /// <summary>
        /// The actual Cooldown timer
        /// </summary>
        private void Update()
        {
            if (startCooldownTimer)
            {
                SpellCoolDown -= Time.deltaTime;
            }

            if (SpellCoolDown <= 0)
            {
                startCooldownTimer = false;

                onCooldown = false;

                if (SpellInfo)
                {
                    SpellInfo.OnSpellCooldownEnd.Invoke();
                }

                OnSpellCastEnded();

                SpellCoolDown = defaultTimer;
            }
        }
        /// <summary>
        /// Destroy Gameobject on cast end
        /// </summary>
        protected virtual void OnSpellCastEnded()
        {
            SpellInfo.OnSpellCastEnd.Invoke();

            Destroy(gameObject);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Check to see if this status effect is using two values
        /// </summary>
        public bool UsingTwoValues { get; private set; } = false;
        /// <summary>
        /// The first effect amount
        /// </summary>
        public float Value1 { get; private set; } = 0f;
        /// <summary>
        /// The second effect amount is only set if UsingTwoValues is true
        /// </summary>
        public float Value2 { get; private set; } = 0f;
        /// <summary>
        /// Gets this spell id
        /// </summary>
        public int MyID { get; private set; } = 0;
        /// <summary>
        /// How long the spell cooldown lasts
        /// </summary>
        public float SpellCoolDown { get; private set; } = 0f;
        /// <summary>
        /// The spell to spawn into the world
        /// </summary>
        public bool HasCoolDown { get; private set; } = false;
        /// <summary>
        /// Gets the actual spell that is being casted
        /// </summary>
        public ScriptableSpell SpellInfo { get; private set; }
        #endregion
    }
}