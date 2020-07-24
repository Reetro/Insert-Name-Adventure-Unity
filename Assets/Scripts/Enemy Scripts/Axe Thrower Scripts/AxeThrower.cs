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
            print(MyHealthComponent.CurrentHealth);

            if (MyHealthComponent.IsCurrentlyDead)
            {
                isPlayerVisiable = false;

                if (isCoroutineRuning)
                {
                    StopCoroutine(ShootInterval());

                    isCoroutineRuning = false;
                }
            }
            else if (IsPlayerVisiable(sightLayers))
            {
                if (!isCoroutineRuning)
                {
                    isPlayerVisiable = true;

                    StartCoroutine(ShootInterval());
                }
            }
            else
            {
                isPlayerVisiable = false;

                if (isCoroutineRuning)
                {
                    StopCoroutine(ShootInterval());

                    isCoroutineRuning = false;
                }
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

        protected override void Shoot()
        {
            ProjectileMovement axe = Instantiate(ProjectileToShoot, FireTransform.position, Quaternion.identity);

            Vector2 launchDirection = GeneralFunctions.GetDistanceBetweenVectors(PlayerTransform.position, transform.position);

            axe.ConstructProjectile(ProjectileSpeed, ProjectileDamage, launchDirection);
        }

        public override void OnDeath()
        {
            base.OnDeath();

            Destroy(gameObject);
        }
    }
}