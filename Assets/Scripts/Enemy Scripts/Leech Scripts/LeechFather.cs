using UnityEngine;
using LevelObjects.MovingObjects;
using System;

namespace EnemyCharacter.AI
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
            ProjectileMovement bulllet = Instantiate(_ProjectileMovement, FireTransform.position, Quaternion.identity);

            Vector2 launchDirection = gameObject.transform.TransformDirection(FireTransform.position);

            bulllet.ConstructProjectile(ProjectileSpeed, ProjectileDamage, launchDirection);
        }
    }
}