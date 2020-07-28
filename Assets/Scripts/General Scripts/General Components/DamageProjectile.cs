using UnityEngine;

namespace LevelObjects.MovingObjects
{
    public class DamageProjectile : ProjectileMovement
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            GeneralFunctions.DamageTarget(collision.gameObject, damage, true);

            Destroy(gameObject);
        }
    }
}