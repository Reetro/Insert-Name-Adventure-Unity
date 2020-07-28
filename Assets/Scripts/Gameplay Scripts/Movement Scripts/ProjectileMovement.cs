using UnityEngine;
using UnityEngine.Events;

namespace LevelObjects.MovingObjects
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ProjectileMovement : MonoBehaviour
    {
        protected Vector2 CurrentVelocity { get; set; }

        protected Rigidbody2D MyRigidBody2D { get; private set; } = null;

        private Vector2 launchDirection;
        protected bool canFire = false;
        protected bool useNoise = false;
        protected float damage = 1f;

        private Vector2 lastDirection = Vector2.zero;

        protected float MinNoise { get; private set; } = 0;
        protected float MaxNoise { get; private set; } = 0;
        public virtual bool UseFixedUpdate { get; set; } = true;
        protected float MoveSpeed { get; private set; } = 0;
        public Vector2 LastDirection { get { return lastDirection; } }
        public Vector2 LaunchDirection { get { return launchDirection; } }

        [Header("Events")]
        [Space]
        public UnityEvent OnImpact;

        protected virtual void Awake()
        {
            MyRigidBody2D = GetComponent<Rigidbody2D>();

            OnImpact.AddListener(OnProjectileImpact);
        }

        public virtual void ConstructProjectile(float moveSpeed, float damage, Vector2 launchDirection)
        {
            this.damage = damage;
            MoveSpeed = moveSpeed;
            this.launchDirection = launchDirection;

            canFire = true;
            useNoise = false;
        }

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

        public void UpdateDirection(Vector2 newDirection)
        {
            lastDirection = launchDirection;
            launchDirection = newDirection;
        }

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

        public virtual void OnProjectileImpact()
        {
            Destroy(gameObject);
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (GeneralFunctions.IsObjectPlayer(collision.gameObject))
            {
                GeneralFunctions.DamageTarget(collision.gameObject, damage, true);

                OnImpact.Invoke();
            }
            else
            {
                OnImpact.Invoke();
            }
        }
    }
}