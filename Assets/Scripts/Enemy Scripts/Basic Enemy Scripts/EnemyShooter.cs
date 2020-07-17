using System.Collections;
using UnityEngine;
using LevelObjects.MovingObjects;

namespace EnemyCharacter.AI
{
    public class EnemyShooter : EnemyBase
    {
        [SerializeField] private ProjectileMovement projectilePrefab = null;
        [SerializeField] private float shootIntervale = 2f;
        [SerializeField] private float projectileSpeed = 400f;
        [SerializeField] private float projectileDamage = 1f;
        [SerializeField] private Transform firePoint = null;

        private void Start()
        {
            StartCoroutine(ShootInterval());
        }
        /// <summary>
        /// Called every X seconds (determined by shootIntervale) will call the shoot method
        /// </summary>
        private IEnumerator ShootInterval()
        {
            while (!MyHealthComponent.IsCurrentlyDead)
            {
                yield return new WaitForSecondsRealtime(shootIntervale);

                Shoot();
            }
        }
        /// <summary>
        /// Called every X seconds (determined by shootIntervale)
        /// </summary>
        protected virtual void Shoot()
        {
            // for use in children
            Debug.LogWarning(gameObject.name + "shoot script has no implementation");
        }
        /// <summary>
        /// Gets the current projectile prefab
        /// </summary>
        public ProjectileMovement ProjectileToShoot => projectilePrefab;
        /// <summary>
        /// Gets current shoot intervals
        /// </summary>
        public float ShootIntervale { get { return shootIntervale; } }
        /// <summary>
        /// Get the speed of the current projectile
        /// </summary>
        public float ProjectileSpeed { get { return projectileSpeed; } }
        /// <summary>
        /// Gets the amount of damage the projectile can deal
        /// </summary>
        public float ProjectileDamage { get { return projectileDamage; } }
        /// <summary>
        /// Gets the transform of the projectile fire point
        /// </summary>
        public Transform FireTransform { get { return firePoint; } }
    }
}