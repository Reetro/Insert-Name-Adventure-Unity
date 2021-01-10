using UnityEngine;

namespace LevelObjects.MovingObjects
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