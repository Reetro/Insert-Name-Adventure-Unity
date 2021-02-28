using GameplayScripts.GeneralMovementScripts;
using UnityEngine;

namespace GeneralScripts.GeneralComponents
{
    public class DamageProjectile : ProjectileMovement
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            GeneralFunctions.ApplyDamageToTarget(collision.gameObject, damage, true, gameObject);

            Destroy(gameObject);
        }
    }
}