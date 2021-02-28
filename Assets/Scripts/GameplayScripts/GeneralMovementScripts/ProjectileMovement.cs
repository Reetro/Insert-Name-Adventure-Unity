using GeneralScripts;
using UnityEngine;

namespace GameplayScripts.GeneralMovementScripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ProjectileMovement : MonoBehaviour
    {
        /// <summary>
        /// Gets the rigidbody current velocity
        /// </summary>
        protected Vector2 CurrentVelocity { get; set; }
        /// <summary>
        /// Gets the Rigidbody on the projectile
        /// </summary>
        protected Rigidbody2D MyRigidBody2D { get; private set; }
        /// <summary>
        /// The current direction of the projectile
        /// </summary>
        private Vector2 launchDirection;
        /// <summary>
        /// Checks to see if the projectile can currently move
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        protected bool canFire;
        /// <summary>
        /// Whether or not to add a random vector at the start of movement
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        protected bool useNoise;
        /// <summary>
        /// The amount of damage to apply to a hit object
        /// </summary>
        protected float damage = 1f;
        /// <summary>
        /// Minimum value for a random vector
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        protected float MinNoise { get; private set; }
        /// <summary>
        /// Maximum value for a random vector
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        protected float MaxNoise { get; private set; }
        /// <summary>
        /// Whether or not to move this projectile with the fixed update method
        /// </summary>
        protected virtual bool UseFixedUpdate { get; set; } = true;
        /// <summary>
        /// How fast the projectile travels
        /// </summary>
        protected float MoveSpeed { get; private set; }
        /// <summary>
        /// Gets the current launchDirection
        /// </summary>
        protected Vector2 LaunchDirection => launchDirection;

        protected virtual void Awake()
        {
            MyRigidBody2D = GetComponent<Rigidbody2D>();
        }
        /// <summary>
        /// Sets all default values and starts projectile movement
        /// </summary>
        /// <param name="moveSpeed"></param>
        /// <param name="mewDamage"></param>
        /// <param name="newLaunchDirection"></param>
        public virtual void ConstructProjectile(float moveSpeed, float mewDamage, Vector2 newLaunchDirection)
        {
            damage = mewDamage;
            MoveSpeed = moveSpeed;
            launchDirection = newLaunchDirection;

            canFire = true;
            useNoise = false;
        }
        /// <summary>
        /// Sets all default values and starts projectile movement also adds a random vector to launch direction
        /// </summary>
        /// <param name="moveSpeed"></param>
        /// <param name="newDamage"></param>
        /// <param name="newLaunchDirection"></param>
        /// <param name="noiseMin"></param>
        /// <param name="noiseMax"></param>
        public virtual void ConstructProjectileWithNoise(float moveSpeed, float newDamage, Vector2 newLaunchDirection, float noiseMin, float noiseMax)
        {
            damage = newDamage;
            MoveSpeed = moveSpeed;
            launchDirection = newLaunchDirection + GeneralFunctions.CreateRandomVector2(noiseMin, noiseMax, noiseMin, noiseMax);

            MinNoise = noiseMin;
            MaxNoise = noiseMax;

            canFire = true;
            useNoise = true;
        }
        /// <summary>
        /// Changes the current direction of the projectile
        /// </summary>
        /// <param name="newDirection"></param>
        protected virtual void UpdateDirection(Vector2 newDirection)
        {
            launchDirection = newDirection;
        }
        /// <summary>
        /// Changes the current velocity of the projectile
        /// </summary>
        /// <param name="newVelocity"></param>
        public void UpdateVelocity(Vector2 newVelocity)
        {
            MyRigidBody2D.velocity = newVelocity;
        }

        protected virtual void FixedUpdate()
        {
            if (!UseFixedUpdate) return;
            CurrentVelocity = MyRigidBody2D.velocity;

            if (canFire && useNoise)
            {
                var newDirection = launchDirection.normalized + GeneralFunctions.CreateRandomVector2(MinNoise, MaxNoise, MinNoise, MaxNoise);

                MyRigidBody2D.velocity = newDirection * (MoveSpeed * Time.fixedDeltaTime);
            }

            if (canFire)
            {
                MyRigidBody2D.velocity = launchDirection.normalized * (MoveSpeed * Time.fixedDeltaTime);
            }
        }
    }
}