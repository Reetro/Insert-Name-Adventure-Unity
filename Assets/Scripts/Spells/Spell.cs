using EnemyScripts.BasicEnemyScripts;
using EnemyScripts.LeechScripts;
using GameplayScripts;
using GeneralScripts;
using GeneralScripts.UI;
using UnityEngine;

namespace Spells
{
    public abstract class Spell : MonoBehaviour
    {
        #region Local Vars
        private float defaultTimer;
        /// <summary>
        /// Will start the spell cooldown timer
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        protected bool startCooldownTimer;
        /// <summary>
        /// Checks to see if the spell is currently on cooldown
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        protected bool onCooldown;
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

            OnUnpacksSpellValuesDone();
        }
        /// <summary>
        /// Called after spell has unpacked all need values
        /// </summary>
        protected virtual void OnUnpacksSpellValuesDone()
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
                }
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Counts all enemies in the current level if count is 0 cooldown will pauses 
        /// </summary>
        private bool CanCooldownTick()
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
        private Spell DoesSpellOfTypeExist(Spell spell)
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
                        SpellInfo.onSpellCooldownEnd.Invoke();
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

            SpellInfo.onSpellCastEnd.Invoke();

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
        private void ResumeCooldown()
        {
            WasOnCooldown = false;

            SpellCoolDown = RemainingCooldownTime;

            startCooldownTimer = true;
        }
        /// <summary>
        /// Upon a level being loaded check to see if was on cooldown
        /// </summary>
        private void OnLevelFinishedLoading()
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
        // ReSharper disable once MemberCanBeProtected.Global
        public bool UsingTwoValues { get; private set; }
        /// <summary>
        /// The first effect amount
        /// </summary>
        // ReSharper disable once MemberCanBeProtected.Global
        public float Value1 { get; private set; }
        /// <summary>
        /// The second effect amount is only set if UsingTwoValues is true
        /// </summary>
        // ReSharper disable once MemberCanBeProtected.Global
        public float Value2 { get; private set; }
        /// <summary>
        /// Gets this spell id
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public int MyID { get; private set; }
        /// <summary>
        /// How long the spell cooldown lasts
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        public float SpellCoolDown { get; private set; }
        /// <summary>
        /// Default value of SpellCoolDown
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        public float DefaultSpellCoolDown { get; private set; }
        /// <summary>
        /// The spell to spawn into the world
        /// </summary>
        // ReSharper disable once MemberCanBeProtected.Global
        public bool HasCoolDown { get; private set; }
        /// <summary>
        /// Gets the actual spell that is being casted
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        public ScriptableSpell SpellInfo { get; private set; }
        /// <summary>
        /// The third effect amount is only set if useThreeValues is true
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public float Value3 { get; private set; }
        /// <summary>
        /// Check to see if this status effect is using three values
        /// </summary>
        // ReSharper disable once MemberCanBeProtected.Global
        public bool UsingThreeValues { get; private set; }
        /// <summary>
        /// The Spell icon this spell is attached to
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        public SpellIcon MySpellIcon { get; private set; }
        /// <summary>
        /// The particle system to spawn into the world
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public GameObject ParticleSystemToSpawn { get; private set; }
        /// <summary>
        /// Whether or not to spawn a particle effect on cast
        /// </summary>
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // ReSharper disable once MemberCanBePrivate.Global
        public bool SpawnParticles { get; private set; }
        /// <summary>
        /// How long the particle system is up for
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public float ParticleSystemUpTime { get; private set; }
        /// <summary>
        /// The remaining cooldown time this spell has
        /// </summary>
        private static float RemainingCooldownTime { get; set; }
        /// <summary>
        /// Checks to see if the spell was on cooldown
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        public static bool WasOnCooldown { get; private set; }
        /// <summary>
        /// Does this spell need a enemy active in the level to cooldown
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        public bool NeedsEnemyToCooldown { get; private set; }
        #endregion
    }
}