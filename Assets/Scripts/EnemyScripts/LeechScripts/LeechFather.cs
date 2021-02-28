using System;
using EnemyScripts.BasicEnemyScripts;
using UnityEngine;

namespace EnemyScripts.LeechScripts
{
    [Serializable]
    public class LeechFather : EnemyShooter
    {
        public override void OnSceneCreated()
        {
            base.OnSceneCreated();

            CurrentFireTransform = gameObject.GetComponentInChildren<Transform>();
            AutoStart = true;
        }

        protected override void Shoot()
        {
            var position = FireTransform.position;
            var bullet = Instantiate(_ProjectileMovement, position, Quaternion.identity);

            Vector2 launchDirection = gameObject.transform.TransformDirection(position);

            bullet.ConstructProjectile(ProjectileSpeed, ProjectileDamage, launchDirection);
        }
    }
}