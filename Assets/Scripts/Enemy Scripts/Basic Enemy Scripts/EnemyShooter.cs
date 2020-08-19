using System.Collections;
using UnityEngine;
using LevelObjects.MovingObjects;

namespace EnemyCharacter.AI
{
    public class EnemyShooter : EnemyBase
    {
        [Header("Shooter settings")]
        [Tooltip("How often a projectile is spawned")]
        [SerializeField] protected float shootIntervale = 2f;
        [Tooltip("How fast the spawned projectile goes")]
        [SerializeField] private float projectileSpeed = 400f;
        [Tooltip("How much damage the spawned projectile does")]
        [SerializeField] private float projectileDamage = 1f;
        [Tooltip("The projectile the shooter will spawn")]
        [SerializeField] private GameObject projectileToSpawn = null;

        /// <summary>
        /// The transform to fire the spawned projectile from
        /// </summary>
        protected virtual Transform CurrentFireTransform { get; set; } = null;
        /// <summary>
        /// Whether or not to instantly start the ShootInterval Coroutine when game starts
        /// </summary>
        protected virtual bool AutoStart { get; set; } = true;

        private void Start()
        {
            ConstructShooter();

            if (AutoStart)
            {
                StartCoroutine(ShootInterval());
            }
        }
        /// <summary>
        /// Called every X seconds (determined by shootIntervale) will call the shoot method
        /// </summary>
        protected virtual IEnumerator ShootInterval()
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
            if (projectileToSpawn)
            {
                _ProjectileMovement = projectileToSpawn.GetComponent<ProjectileMovement>();

                if (_ProjectileMovement)
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
                    Debug.LogError(gameObject.ToString() + " failed to set ProjectileToShoot projectile to spawn does not have a projectile movement component");
                }
            }
            else
            {
                Debug.LogError(gameObject.ToString() + " failed to set ProjectileToShoot projectile to spawn was invalid");
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
        /// Gets the current projectile prefab projectile movement component
        /// </summary>
        public ProjectileMovement _ProjectileMovement { get; private set; } = null;
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