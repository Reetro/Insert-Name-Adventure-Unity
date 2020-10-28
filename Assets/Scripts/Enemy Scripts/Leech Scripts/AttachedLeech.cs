using UnityEngine;
using StatusEffects.Effects;
using StatusEffects;

namespace EnemyCharacter
{
    public class AttachedLeech : MonoBehaviour
    {
        [SerializeField] ScriptableStatusEffect leechingDebuff = null;
        [SerializeField] HealthComponent leechHealthComp = null;

        private StatusEffect statusEffect = null;

        /// <summary>
        /// The old leech ID
        /// </summary>
        public int MyID { get; private set; }
        /// <summary>
        /// Called after a leech has been spawned
        /// </summary>
        /// <param name="health"></param>
        /// <param name="player"></param>
        /// <param name="id"></param>
        public void OnLeechSpawn(float health, GameObject player, int id)
        {
            MyID = id;

            GeneralFunctions.ConstructHPComponent(gameObject);

            leechHealthComp.SetHealth(health);

            leechHealthComp.OnDeath.AddListener(OnDeath);

            statusEffect = GeneralFunctions.ApplyStatusEffectToTarget(player, leechingDebuff);

            // Ignore trap projectile collision
            Physics2D.IgnoreLayerCollision(14, 20, true);
        }
        /// <summary>
        /// Called when an attached leech has been killed
        /// </summary>
        public void OnDeath()
        {
            statusEffect.RemoveFromStack(statusEffect);

            Destroy(gameObject);
        }
    }
}