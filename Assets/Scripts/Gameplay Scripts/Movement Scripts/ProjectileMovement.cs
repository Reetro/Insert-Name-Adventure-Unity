using UnityEngine;

namespace LevelObjects.MovingObjects
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
        protected Rigidbody2D MyRigidBody2D { get; private set; } = null;
        /// <summary>
        /// The current direction of the projectile
        /// </summary>
        private Vector2 launchDirection;
        /// <summary>
        /// Checks to see if the projectile can currently move
        /// </summary>
        protected bool canFire = false;
        /// <summary>
        /// Whether or not to add a random vector at the start of movement
        /// </summary>
        protected bool useNoise = false;
        /// <summary>
        /// The amount of damage to apply to a hit object
        /// </summary>
        protected float damage = 1f;
        /// <summary>
        /// Minimum value for a random vector
        /// </summary>
        protected float MinNoise { get; private set; } = 0;
        /// <summary>
        /// Maximum value for a random vector
        /// </summary>
        protected float MaxNoise { get; private set; } = 0;
        /// <summary>
        /// Whether or not to move this projectile with the fixed update method
        /// </summary>
        public virtual bool UseFixedUpdate { get; set; } = true;
        /// <summary>
        /// How fast the projectile travels
        /// </summary>
        protected float MoveSpeed { get; private set; } = 0;
        /// <summary>
        /// Gets the current launchDirection
        /// </summary>
        public Vector2 LaunchDirection { get { return launchDirection; } }

        protected virtual void Awake()
        {
            MyRigidBody2D = GetComponent<Rigidbody2D>();
        }
        /// <summary>
        /// Sets all default values and starts projectile movement
        /// </summary>
        /// <param name="moveSpeed"></param>
        /// <param name="damage"></param>
        /// <param name="launchDirection"></param>
        public virtual void ConstructProjectile(float moveSpeed, float damage, Vector2 launchDirection)
        {
            this.damage = damage;
            MoveSpeed = moveSpeed;
            this.launchDirection = launchDirection;

            canFire = true;
            useNoise = false;
        }
        /// <summary>
        /// Sets all default values and starts projectile movement also adds a random vector to launch direction
        /// </summary>
        /// <param name="moveSpeed"></param>
        /// <param name="damage"></param>
        /// <param name="launchDirection"></param>
        /// <param name="noiseMin"></param>
        /// <param name="noiseMax"></param>
        public virtual void ConstructProjectileWithNoise(float moveSpeed, float damage, Vector2 launchDirection, float noiseMin, float noiseMax)
        {
            this.damage = damage;
            MoveSpeed = moveSpeed;
            this.launchDirection = launchDirection + GeneralFunctions.CreateRandomVector2(noiseMin, noiseMax, noiseMin, noiseMax);

            MinNoise = noiseMin;
            MaxNoise = noiseMax;

            canFire = true;
            useNoise = true;
        }
        /// <summary>
        /// Changes the current direction of the projectile
        /// </summary>
        /// <param name="newDirection"></param>
        public void UpdateDirection(Vector2 newDirection)
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
            if (UseFixedUpdate)
            {
                CurrentVelocity = MyRigidBody2D.velocity;

                if (canFire && useNoise)
                {
                    var newDirection = launchDirection.normalized + GeneralFunctions.CreateRandomVector2(MinNoise, MaxNoise, MinNoise, MaxNoise);

                    MyRigidBody2D.velocity = newDirection * MoveSpeed * Time.fixedDeltaTime;
                }

                if (canFire)
                {
                    MyRigidBody2D.velocity = launchDirection.normalized * MoveSpeed * Time.fixedDeltaTime;
                }
            }
        }
    }
}