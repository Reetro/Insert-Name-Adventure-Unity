using UnityEngine;
using AuraSystem.Effects;

namespace EnemyCharacter
{
    public class AttachedLeech : MonoBehaviour
    {
        [SerializeField] ScriptableDebuff leechingDebuff = null;
        [SerializeField] HealthComponent leechHealthComp = null;

        private DebuffEffect debuffEffect = null;

        /// <summary>
        /// The old leech ID
        /// </summary>
        public int MyID { get; private set; }

        public void OnLeechSpawn(float health, GameObject player, int id)
        {
            MyID = id;

            GeneralFunctions.ConstructHPComponent(gameObject);

            leechHealthComp.SetHealth(health);

            leechHealthComp.OnDeath.AddListener(OnDeath);

            debuffEffect = GeneralFunctions.ApplyDebuffToTarget(player, leechingDebuff, true);
        }

        public void OnDeath()
        {
            debuffEffect.RemoveFromStack(true, debuffEffect);

            Destroy(gameObject);
        }
    }
}