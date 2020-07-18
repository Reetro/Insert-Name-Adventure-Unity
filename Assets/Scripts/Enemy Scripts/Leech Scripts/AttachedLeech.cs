using UnityEngine;
using AuraSystem;
using AuraSystem.Effects;

namespace EnemyCharacter
{
    public class AttachedLeech : MonoBehaviour
    {
        [SerializeField] ScriptableDebuff leechingDebuff = null;
        [SerializeField] HealthComponent leechHealthComp = null;

        private DebuffEffect debuffEffect = null;

        public void OnLeechSpawn(float health, GameObject player)
        {
            GeneralFunctions.ConstructHPComponent(gameObject);

            leechHealthComp.SetHealth(health);

            debuffEffect = GeneralFunctions.ApplyDebuffToTarget(player, leechingDebuff, true);
        }

        public void OnDeath()
        {
            debuffEffect.RemoveFromStack(true, debuffEffect);

            Destroy(gameObject);
        }
    }
}