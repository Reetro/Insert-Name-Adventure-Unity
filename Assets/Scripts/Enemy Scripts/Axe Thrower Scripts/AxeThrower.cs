using LevelObjects.MovingObjects;
using System;
using System.Collections;
using UnityEngine;

namespace EnemyCharacter.AI
{
    [Serializable]
    public class AxeThrower : EnemyShooter
    {
        private bool isCoroutineRuning = false;
        private bool isPlayerVisiable = false;

        [Tooltip("What layers this object can see")]
        public LayerMask sightLayers;

        protected override void Awake()
        {
            base.Awake();

            ProjectilePath = "Enemy Projectiles/Axe";
            CurrentFireTransform = gameObject.GetComponentInChildren<Transform>();
            AutoStart = false;
        }

        private void Update()
        {
            if (MyHealthComponent.IsCurrentlyDead)
            {
                StopShooting();

            }
            else if (IsPlayerVisiable(sightLayers))
            {
                StartShooting();

            }
            else
            {
                StopShooting();
            }
        }

        protected override IEnumerator ShootInterval()
        {
            isCoroutineRuning = true;

            while (isPlayerVisiable)
            {

                Shoot();

                yield return new WaitForSeconds(shootIntervale);
            }
        }
        
        private void StartShooting()
        {
            if (!isCoroutineRuning)
            {
                isPlayerVisiable = true;

                StartCoroutine(ShootInterval());
            }
        }

        protected override void Shoot()
        {
            ProjectileMovement axe = Instantiate(ProjectileToShoot, FireTransform.position, Quaternion.identity);

            Vector2 launchDirection = GeneralFunctions.GetDistanceBetweenVectors(PlayerTransform.position, transform.position);

            axe.ConstructProjectile(ProjectileSpeed, ProjectileDamage, launchDirection);
        }

        private void StopShooting()
        {
            isPlayerVisiable = false;

            if (isCoroutineRuning)
            {
                StopCoroutine(ShootInterval());

                isCoroutineRuning = false;
            }
        }

        public override void OnDeath()
        {
            base.OnDeath();

            Destroy(gameObject);
        }
    }
}