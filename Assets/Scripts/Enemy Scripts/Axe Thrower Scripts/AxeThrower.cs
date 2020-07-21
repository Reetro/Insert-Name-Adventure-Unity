using LevelObjects.MovingObjects;
using System;
using UnityEngine;

namespace EnemyCharacter.AI
{
    [Serializable]
    public class AxeThrower : EnemyShooter
    {
        protected override void Shoot()
        {
            ProjectileMovement axe = Instantiate(ProjectileToShoot, FireTransform.position, Quaternion.identity);


        }
    }
}
