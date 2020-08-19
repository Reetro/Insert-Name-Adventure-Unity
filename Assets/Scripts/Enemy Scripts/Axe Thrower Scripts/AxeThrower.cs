using LevelObjects.MovingObjects;
using System;
using System.Collections;
using UnityEngine;

namespace EnemyCharacter.AI
{
    [Serializable]
    public class AxeThrower : EnemyShooter
    {
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
        /// Start player visibility check
        /// </summary>
        protected override void Start()
        {
            base.Start();

            StartCoroutine(CheckPlayerVisablity());
        }
        /// <summary>
        /// Every 0.001 second check to see if player is visible
        /// </summary>
        /// <returns></returns>
        private IEnumerator CheckPlayerVisablity()
        {
            while (!isPlayerVisiable)
            {
                var visible = MyMovementComp.IsTransformVisiable(sightLayers, CurrentFireTransform, PlayerTransform, "Player", sightRange, drawDebug);

                if (visible)
                {
                    isPlayerVisiable = true;
                    StartCoroutine(ShootInterval());
                    yield break;
                }
                else
                {
                    yield return new WaitForSeconds(0.001f);
                }
            }
        }
        /// <summary>
        /// Check to see if Axe thrower is dead if so stop shooting
        /// </summary>
        private void Update()
        {
            if (MyHealthComponent.IsCurrentlyDead)
            {
                StopShooting();
            }
        }
        /// <summary>
        /// Every X Seconds throw an axe
        /// </summary>
        protected override IEnumerator ShootInterval()
        {
            while (isPlayerVisiable)
            {
                var visible = MyMovementComp.IsTransformVisiable(sightLayers, CurrentFireTransform, PlayerTransform, "Player", sightRange, drawDebug);

                if (visible)
                {
                    Shoot();

                    yield return new WaitForSeconds(shootIntervale);
                }
                else
                {
                    StopShooting();
                }
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

            StopCoroutine(ShootInterval());

            StartCoroutine(CheckPlayerVisablity());
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