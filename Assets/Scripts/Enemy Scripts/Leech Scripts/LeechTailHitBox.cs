using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyCharacter.Collision
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
        /// <summary>
        /// Damage the parent leech object
        /// </summary>
        /// <param name="showText"></param>
        /// <param name="amount"></param>
        /// <param name="whatCanBeDamaged"></param>
        public void DamageParent(bool showText, float amount, LayerMask whatCanBeDamaged)
        {
            parentHealthComp.ProccessDamage(amount, showText, whatCanBeDamaged);
        }
    }
}
