﻿using UnityEngine;
using LevelObjects.MovingObjects;

namespace EnemyCharacter.AI
{
    public class LeechFather : EnemyShooter
    {
        protected override void Shoot()
        {
            ProjectileMovement bulllet = Instantiate(ProjectileToShoot, FireTransform.position, Quaternion.identity);

            Vector2 launchDirection = gameObject.transform.TransformDirection(FireTransform.position);

            bulllet.ConstructProjectile(ProjectileSpeed, ProjectileDamage, launchDirection);
        }
    }
}