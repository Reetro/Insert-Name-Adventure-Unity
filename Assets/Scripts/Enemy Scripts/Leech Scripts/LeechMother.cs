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

            ProjectilePath = "Enemy Projectiles/PH Leech Goo";
            CurrentFireTransform = gameObject.GetComponentInChildren<Transform>();
            AutoStart = true;
        }

        protected override void Shoot()
        {
            if (!GeneralFunctions.IsObjectAbove(PlayerTransform.position, transform.position))
            {
                ProjectileMovement bulllet = Instantiate(ProjectileToShoot, FireTransform.position, Quaternion.identity);

                Vector2 launchDirection = GeneralFunctions.GetDistanceBetweenVectors(PlayerTransform.position, transform.position);

                bulllet.ConstructProjectile(ProjectileSpeed, ProjectileDamage, launchDirection);
            }
        }
    }
}