using System;
using EnemyScripts.BasicEnemyScripts;
using GeneralScripts;
using UnityEngine;

namespace EnemyScripts.LeechScripts
{
    [Serializable]
    public class LeechMother : EnemyShooter
    {
        public override void OnSceneCreated()
        {
            base.OnSceneCreated();

            CurrentFireTransform = gameObject.GetComponentInChildren<Transform>();
            AutoStart = true;
        }

        protected override void Shoot()
        {
            if (GeneralFunctions.IsObjectAbove(PlayerTransform.position, transform.position)) return;
            var bullet = Instantiate(_ProjectileMovement, FireTransform.position, Quaternion.identity);

            var launchDirection = GeneralFunctions.GetDistanceBetweenVectors(PlayerTransform.position, transform.position);

            bullet.ConstructProjectile(ProjectileSpeed, ProjectileDamage, launchDirection);
        }
    }
}