using UnityEngine;
using LevelObjects.MovingObjects;
using System;

namespace EnemyCharacter.AI
{
    [Serializable]
    public class LeechMother : EnemyShooter
    {
        protected override void Awake()
        {
            base.Awake();

            PrefabPath = "Enemy Projectiles/PH Leech Goo";
            CurrentFireTransform = gameObject.GetComponentInChildren<Transform>();
        }

        protected override void Shoot()
        {
            if (!GeneralFunctions.IsObjectAbove(PlayerTransform.position, transform.position))
            {
                ProjectileMovement bulllet = Instantiate(ProjectileToShoot, FireTransform.position, Quaternion.identity);

                Vector2 launchDirection = PlayerTransform.position - transform.position;

                bulllet.ConstructProjectile(ProjectileSpeed, ProjectileDamage, launchDirection);
            }
        }
    }
}