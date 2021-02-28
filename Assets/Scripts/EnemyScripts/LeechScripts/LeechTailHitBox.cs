using GeneralScripts.GeneralComponents;
using UnityEngine;

namespace EnemyScripts.LeechScripts
{
    public class LeechTailHitBox : MonoBehaviour
    {
        private HealthComponent parentHealthComp = null;

        private void Awake()
        {
            parentHealthComp = GetComponentInParent<HealthComponent>();

            if (!parentHealthComp)
            {
                Debug.LogError(name + " failed to to get parent leech health component");
            }
        }
        // ReSharper disable Unity.PerformanceAnalysis
        /// <summary>
        /// Damage the parent leech object
        /// </summary>
        /// <param name="showText"></param>
        /// <param name="amount"></param>
        /// <param name="whatCanBeDamaged"></param>
        public void DamageParent(bool showText, float amount, LayerMask whatCanBeDamaged)
        {
            parentHealthComp.ProcessesDamage(amount, showText, whatCanBeDamaged);
        }
    }
}
