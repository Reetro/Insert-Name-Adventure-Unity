using UnityEngine;
using System.Collections;
using EnemyCharacter.AI;

namespace LevelObjects.MovingObjects
{
    public class Boomerang : ProjectileMovement
    {
        #region Properties
        /// <summary>
        /// The shaman that spawned this boomerang
        /// </summary>
        private Shaman currentShaman = null;
        /// <summary>
        /// How many hits it takes to teleport a shaman
        /// </summary>
        private int maxHitsBeforeTeleport = 0;
        /// <summary>
        /// How much to offset the shaman teleport location by
        /// </summary>
        private float offSet = 0.5f;
        /// <summary>
        /// Checks to see if this boomerang just spawned
        /// </summary>
        private bool justSpawned = true;
        /// <summary>
        /// The current amount of times this boomerang has hit a surface
        /// </summary>
        public int CurrentHitCount { get; set; }
        /// <summary>
        /// Once the boomerang has damaged the player this delay is used to determine when damage can be applied again
        /// </summary>
        public float DamageDelay { get; set; }
        /// <summary>
        /// The maximum speed this projectile can go
        /// </summary>
        private float MaxSpeedMagnitude = 8;
        /// <summary>
        /// Determines if the boomerang can damage the player
        /// </summary>
        private bool canDamage = true;
        /// <summary>
        /// Checks to see if the shaman can actually teleport to the given location
        /// </summary>
        private bool canShamanTeleprot = true;
        #endregion

        #region Movement Functions
        /// <summary>
        /// Reflect the projectile when ever it hits a surface
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            Vector2 _wallNormal = collision.GetContact(0).normal;

            Vector2 newDirection = Vector2.Reflect(CurrentVelocity, _wallNormal);

            UpdateDirection(newDirection);

            if (GeneralFunctions.IsObjectPlayer(collision.gameObject))
            {
                if (canDamage)
                {
                    GeneralFunctions.ApplyDamageToTarget(collision.gameObject, damage, true, gameObject);

                    canDamage = false;

                    StartCoroutine(StartDamageDelay());
                }
            }

            if (!justSpawned && !GeneralFunctions.IsObjectPlayer(collision.gameObject))
            {
                CurrentHitCount++;
            }

            if (CurrentHitCount >= maxHitsBeforeTeleport)
            {
                TeleportShaman(collision.GetContact(0).point);

                CurrentHitCount = 0;
            }
        }
        /// <summary>
        /// Keep tracks of the current velocity the projectile has
        /// </summary>
        protected override void FixedUpdate()
        {
            if (MyRigidBody2D.velocity.magnitude >= MaxSpeedMagnitude || MyRigidBody2D.velocity.magnitude <= MaxSpeedMagnitude)
            {
                MyRigidBody2D.velocity = MyRigidBody2D.velocity.normalized * MaxSpeedMagnitude;
            }

            CurrentVelocity = MyRigidBody2D.velocity;
        }
        /// <summary>
        /// If the circle collider overlaps with ground destroy the current boomerang and spawn in a new one
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (GeneralFunctions.IsObjectOnLayer("Ground", collision.gameObject) || collision.gameObject.CompareTag("Shaman"))
            {
                if (!justSpawned)
                {
                    canShamanTeleprot = false;

                    DestroyBoomerang(true);
                }
            }
        }
        /// <summary>
        /// The damage delay timer
        /// </summary>
        private IEnumerator StartDamageDelay()
        {
            yield return new WaitForSeconds(DamageDelay);

            canDamage = true;
        }
        #endregion

        #region Shaman Teleport Functions
        /// <summary>
        /// Teleports the shaman to the given location
        /// </summary>
        /// <param name="teleportLocation"></param>
        private void TeleportShaman(Vector2 teleportLocation)
        {
            if (canShamanTeleprot)
            {
                var newLocation = GetAdjustedTeleportLocation(teleportLocation, offSet);

                currentShaman.transform.position = newLocation;

                var lookAt = GeneralFunctions.LookAt2D(currentShaman.transform.position, transform.position);

                transform.rotation = lookAt;

                currentShaman.MyRigidBody2D.velocity = Vector2.zero;

                DestroyBoomerang(true);
            }
        }
        /// <summary>
        /// Destroys this boomerang and can tell the shaman to throw a new one
        /// </summary>
        /// <param name="throwBoomerang"></param>
        /// <param name="randomThrow"></param>
        public void DestroyBoomerang(bool throwBoomerang)
        {
            if (currentShaman)
            {
                if (throwBoomerang)
                {
                    currentShaman.ThrowBoomerang();
                }

                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("Unable to destroy boomerang " + name.ToString() + " currentShaman is invalid");
            }
        }
        /// <summary>
        /// Fire a ray2D at the given location
        /// </summary>
        /// <param name="teleportLocation"></param>
        /// <param name="offSet"></param>
        /// <returns>The end point of the ray</returns>
        public Vector2 GetAdjustedTeleportLocation(Vector2 teleportLocation, float offSet)
        {
            Ray2D ray = new Ray2D(teleportLocation, teleportLocation.normalized);
            ray.direction = -ray.direction;

            var point = ray.GetPoint(offSet);

            return point;
        }
        /// <summary>
        /// Sets all local values and adds force to the rigidbody
        /// </summary>
        /// <param name="shamanToSet"></param>
        /// <param name="maxHitsBeforeTeleport"></param>
        /// <param name="offSet"></param>
        public void SetupBoomerang(Shaman shamanToSet, int maxHitsBeforeTeleport, float offSet, float MaxSpeed, float DamageDelay)
        {
            justSpawned = true;

            currentShaman = shamanToSet;

            MaxSpeedMagnitude = MaxSpeed;

            this.maxHitsBeforeTeleport = maxHitsBeforeTeleport;

            this.offSet = offSet;

            MyRigidBody2D.AddForce(LaunchDirection * MoveSpeed);

            this.DamageDelay = DamageDelay;

            canDamage = true;

            canShamanTeleprot = true;

            StartCoroutine(SpawnTimer());
        }
        /// <summary>
        /// A small .1 second timer that adds a delay to hit counting
        /// </summary>
        private IEnumerator SpawnTimer()
        {
            yield return new WaitForSeconds(0.1f);

            justSpawned = false;
        }
        #endregion
    }
}