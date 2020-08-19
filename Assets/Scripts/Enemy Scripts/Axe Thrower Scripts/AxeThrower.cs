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

        /// <summary>
        /// Get the transform to spawn axes at and disable auto start
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            CurrentFireTransform = gameObject.GetComponentInChildren<Transform>();
            AutoStart = false;
        }
        /// <summary>
        /// Check to see if player is visitable if so start throwing axes 
        /// </summary>
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
        /// <summary>
        /// Starts throwing axes at a set interval
        /// </summary>
        private void StartShooting()
        {
            if (!isCoroutineRuning)
            {
                isPlayerVisiable = true;

                StartCoroutine(ShootInterval());
            }
        }
        /// <summary>
        /// Every X Seconds throw an axe
        /// </summary>
        protected override IEnumerator ShootInterval()
        {
            while (isPlayerVisiable)
            {
                isCoroutineRuning = true;

                Shoot();

                yield return new WaitForSeconds(shootIntervale);
            }
        }
        /// <summary>
        /// Spawn an axe and set up damage, speed, and direction
        /// </summary>
        protected override void Shoot()
        {
            ProjectileMovement axe = Instantiate(_ProjectileMovement, FireTransform.position, Quaternion.identity);

            Vector2 launchDirection = GeneralFunctions.GetDistanceBetweenVectors(PlayerTransform.position, transform.position);

            axe.ConstructProjectile(ProjectileSpeed, ProjectileDamage, launchDirection);
        }
        /// <summary>
        /// Stop throwing axes
        /// </summary>
        private void StopShooting()
        {
            isPlayerVisiable = false;

            if (isCoroutineRuning)
            {
                StopCoroutine(ShootInterval());

                isCoroutineRuning = false;
            }
        }
        /// <summary>
        /// Destroy Axe thrower Gameobject on death
        /// </summary>
        protected override void OnDeath()
        {
            base.OnDeath();

            Destroy(gameObject);
        }
    }
}