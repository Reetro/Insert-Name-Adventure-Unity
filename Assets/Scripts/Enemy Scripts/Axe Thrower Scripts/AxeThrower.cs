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

        [Header("Sight Settings")]
        [Tooltip("What layers this object can see")]
        public LayerMask sightLayers;
        [SerializeField] private float sightRange = 10f;

        [Space]

        [Header("Debug Settings")]
        [Tooltip("Whether or not to draw debug lines")]
        [SerializeField] private bool drawDebug = false;

        protected override void Awake()
        {
            base.Awake();

            CurrentFireTransform = gameObject.GetComponentInChildren<Transform>();
            AutoStart = false;
        }

        private void Update()
        {
            if (MyHealthComponent.IsCurrentlyDead)
            {
                StopShooting();
            }
            else if (MyMovementComp.IsTransformVisiable(sightLayers, CurrentFireTransform, PlayerTransform, "Player", sightRange, drawDebug))
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
            ProjectileMovement axe = Instantiate(_ProjectileMovement, FireTransform.position, Quaternion.identity);

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

        protected override void OnDeath()
        {
            base.OnDeath();

            Destroy(gameObject);
        }
    }
}