using System.Collections;
using UnityEngine;
using LevelObjects.MovingObjects;

namespace EnemyCharacter.AI
{
    public class EnemyShooter : EnemyBase
    {
        [SerializeField] private float shootIntervale = 2f;
        [SerializeField] private float projectileSpeed = 400f;
        [SerializeField] private float projectileDamage = 1f;

        /// <summary>
        /// Where to find the shooter projectile prefab in resources folder
        /// </summary>
        protected virtual string PrefabPath { get; set; } = "";
        /// <summary>
        /// The transform to fire the spawned projectile from
        /// </summary>
        protected virtual Transform CurrentFireTransform { get; set; } = null;

        private void Start()
        {
            ConstructShooter();

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
        /// Collects all virtual data and sets local values
        /// </summary>
        private void ConstructShooter()
        {
            var projectileObject = Resources.Load(PrefabPath) as GameObject;

            if (projectileObject)
            {
                ProjectileToShoot = projectileObject.GetComponent<ProjectileMovement>();

                if (ProjectileToShoot)
                {
                    if (CurrentFireTransform)
                    {
                        FireTransform = CurrentFireTransform;
                    }
                    else
                    {
                        Debug.LogError(gameObject.name + " failed to set FireTransform CurrentFireTransform is not valid");
                    }
                }
                else
                {
                    Debug.LogError(gameObject.name + " failed to get projectile movement");
                }
            }
            else
            {
                Debug.LogError(gameObject.name + " failed to get projectile prefab invalid path");
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
        public ProjectileMovement ProjectileToShoot { get; private set; } = null;
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
        public Transform FireTransform { get; private set; } = null;
    }
}