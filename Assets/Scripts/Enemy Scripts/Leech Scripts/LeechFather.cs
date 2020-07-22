﻿using UnityEngine;
using LevelObjects.MovingObjects;
using System;

namespace EnemyCharacter.AI
{
    [Serializable]
    public class LeechFather : EnemyShooter
    {
        protected override void Awake()
        {
            base.Awake();

            PrefabPath = "Enemy Projectiles/Shootable Egg";
            CurrentFireTransform = gameObject.GetComponentInChildren<Transform>();
        }

        protected override void Shoot()
        {
            ProjectileMovement bulllet = Instantiate(ProjectileToShoot, FireTransform.position, Quaternion.identity);

            Vector2 launchDirection = gameObject.transform.TransformDirection(FireTransform.position);

            bulllet.ConstructProjectile(ProjectileSpeed, ProjectileDamage, launchDirection);
        }
    }
}