using EnemyCharacter;
using PlayerUI;
using UnityEngine;
using GameplayManagement.Assets;

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
        /// Add listeners to Gameplay Manger Events
        /// </summary>
        public void SetupCallBacks()
        {
            GameAssets.GlobalManager.onSceneLoadingDone.AddListener(OnLevelFinishedLoading);
        }
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
        /// Sets all spell values and casts the spell
        /// </summary>
        public void StartSpellCast(ScriptableSpell spellInfo, GameObject spawnedSpell, SpellIcon spellIcon)
        {
            var spell = spawnedSpell.GetComponent<Spell>();

            if (spell)
            {
                if (CanSpellBeCasted(spellInfo, spell))
                {
                    MySpellIcon = spellIcon;

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
        private void UnpackSpell(bool useTwoValues, float value1, float value2, ScriptableSpell scriptableSpell, float cooldown, bool hasCooldown, bool usingThreeValues, float value3)
        {
            UsingTwoValues = useTwoValues;
            UsingThreeValues = usingThreeValues;
            Value1 = value1;
            Value2 = value2;
            Value3 = value3;
            HasCoolDown = hasCooldown;
            SpellCoolDown = cooldown;
            defaultTimer = cooldown;
            NeedsEnemyToCooldown = scriptableSpell.NeedsEnemyToCooldown;

            ParticleSystemToSpawn = scriptableSpell.ParticleSystemToSpawn;
            ParticleSystemUpTime = scriptableSpell.ParticleSystemUpTime;
            SpawnParticles = scriptableSpell.SpawnParticles;

            SpellInfo = scriptableSpell;

            DefaultSpellCoolDown = SpellCoolDown;

            OnUpackSpellValuesDone();
        }
        /// <summary>
        /// Called after spell has unpacked all need values
        /// </summary>
        protected virtual void OnUpackSpellValuesDone()
        {
            // for use in children
        }
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

                    UnpackSpell(spellToCast.UsingTwoValues, spellToCast.Value1, spellToCast.Value2, spellToCast, spellToCast.SpellCoolDown, spellToCast.HasCoolDown, spellToCast.UsingThreeValues, spellToCast.Value3);

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
        /// Counts all enemies in the current level if count is 0 cooldown will pauses 
        /// </summary>
        public bool CanCooldownTick()
        {
            if (NeedsEnemyToCooldown)
            {
                int enemyCount = 0;
                var enemyBaseCharacters = FindObjectsOfType<EnemyBase>();

                foreach (EnemyBase enemyBase in enemyBaseCharacters)
                {
                    if (enemyBase)
                    {
                        enemyCount++;
                    }
                }

                var attachedLeeches = FindObjectsOfType<AttachedLeech>();

                foreach (AttachedLeech attachedLeech in attachedLeeches)
                {
                    if (attachedLeech)
                    {
                        enemyCount++;
                    }
                }

                return enemyCount > 0;
            }
            else
            {
                return true;
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
            if (MySpellIcon)
            {
                MySpellIcon.DisplayCooldown(true);
            }

            onCooldown = true;
            startCooldownTimer = true;
        }
        /// <summary>
        /// The actual Cooldown timer
        /// </summary>
        protected virtual  void Update()
        {
           if (CanCooldownTick())
           {
                if (startCooldownTimer)
                {
                    SpellCoolDown -= Time.deltaTime;

                    if (MySpellIcon)
                    {
                        MySpellIcon.UpdateCooldownFillAmount(SpellInfo.SpellCoolDown, true);

                        MySpellIcon.UpdateCooldownText(SpellCoolDown);
                    }
                }

                if (SpellCoolDown <= 0)
                {
                    startCooldownTimer = false;

                    onCooldown = false;

                    SpellCoolDown = DefaultSpellCoolDown;

                    if (SpellInfo)
                    {
                        SpellInfo.OnSpellCooldownEnd.Invoke();
                    }

                    OnSpellCastEnded();

                    SpellCoolDown = defaultTimer;
                }
           }
           else
           {
                PauseCooldown();
           }
        }
        /// <summary>
        /// Destroy Gameobject on cast end
        /// </summary>
        protected virtual void OnSpellCastEnded()
        {
            if (MySpellIcon)
            {
                MySpellIcon.DisplayCooldown(false);

                MySpellIcon.ResetCooldownFilAmount();
            }

            SpellInfo.OnSpellCastEnd.Invoke();

            Destroy(gameObject);
        }
        /// <summary>
        /// Stops the cooldown countdown
        /// </summary>
        public void PauseCooldown()
        {
            WasOnCooldown = true;

            startCooldownTimer = false;

            RemainingCooldownTime = SpellCoolDown;

            if (MySpellIcon)
            {
                MySpellIcon.UpdateCooldownFillAmount(SpellInfo.SpellCoolDown, false);

                MySpellIcon.UpdateCooldownText(RemainingCooldownTime);
            }
        }
        /// <summary>
        /// Resumes the spell cooldown
        /// </summary>
        public void ResumeCooldown()
        {
            WasOnCooldown = false;

            SpellCoolDown = RemainingCooldownTime;

            startCooldownTimer = true;
        }
        /// <summary>
        /// Upon a level being loaded check to see if was on cooldown
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="mode"></param>
        void OnLevelFinishedLoading()
        {
            if (WasOnCooldown)
            {
                ResumeCooldown();
            }
        }
        /// <summary>
        /// Pause spell cooldown when player overlaps the level exit
        /// </summary>
        public void OnLevelExitOverlap()
        {
            PauseCooldown();
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
        /// Default value of SpellCoolDown
        /// </summary>
        public float DefaultSpellCoolDown { get; private set; } = 0f;
        /// <summary>
        /// The spell to spawn into the world
        /// </summary>
        public bool HasCoolDown { get; private set; } = false;
        /// <summary>
        /// Gets the actual spell that is being casted
        /// </summary>
        public ScriptableSpell SpellInfo { get; private set; }
        /// <summary>
        /// The third effect amount is only set if useThreeValues is true
        /// </summary>
        public float Value3 { get; private set; } = 0f;
        /// <summary>
        /// Check to see if this status effect is using three values
        /// </summary>
        public bool UsingThreeValues { get; private set; } = false;
        /// <summary>
        /// The Spell icon this spell is attached to
        /// </summary>
        public SpellIcon MySpellIcon { get; private set; }
        /// <summary>
        /// The particle system to spawn into the world
        /// </summary>
        public GameObject ParticleSystemToSpawn { get; private set; }
        /// <summary>
        /// Whether or not to spawn a particle effect on cast
        /// </summary>
        public bool SpawnParticles { get; private set; }
        /// <summary>
        /// How long the particle system is up for
        /// </summary>
        public float ParticleSystemUpTime { get; private set; }
        /// <summary>
        /// The remaining cooldown time this spell has
        /// </summary>
        public static float RemainingCooldownTime { get; private set; } = 0f;
        /// <summary>
        /// Checks to see if the spell was on cooldown
        /// </summary>
        public static bool WasOnCooldown { get; private set; } = false;
        /// <summary>
        /// Does this spell need a enemy active in the level to cooldown
        /// </summary>
        public bool NeedsEnemyToCooldown { get; private set; } = false;
        #endregion
    }
}