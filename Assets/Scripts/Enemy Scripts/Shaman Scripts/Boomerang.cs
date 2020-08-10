using UnityEngine;
using System.Collections;
using EnemyCharacter.AI;

namespace LevelObjects.MovingObjects
{
    public class Boomerang : ProjectileMovement
    {
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
        /// Reflect the projectile when ever it hits a surface
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            Vector2 _wallNormal = collision.GetContact(0).normal;

            Vector2 newDirection = Vector2.Reflect(CurrentVelocity, _wallNormal);

            UpdateDirection(newDirection);

            GeneralFunctions.DamageTarget(collision.gameObject, damage, true);

            if (!justSpawned)
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
            CurrentVelocity = MyRigidBody2D.velocity;
        }
        /// <summary>
        /// Teleports the shaman to the given location
        /// </summary>
        /// <param name="teleportLocation"></param>
        private void TeleportShaman(Vector2 teleportLocation)
        {
            var newLocation = GetAdjustedTeleportLocation(teleportLocation, offSet);

            currentShaman.transform.position = newLocation;

            var lookAt = GeneralFunctions.LookAt2D(currentShaman.transform.position, transform.position);

            transform.rotation = lookAt;

            currentShaman.MyRigidBody2D.velocity = Vector2.zero;

            DestroyBoomerang(true);
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
        public void SetupBoomerang(Shaman shamanToSet, int maxHitsBeforeTeleport, float offSet)
        {
            justSpawned = true;

            currentShaman = shamanToSet;

            this.maxHitsBeforeTeleport = maxHitsBeforeTeleport;

            this.offSet = offSet;

            MyRigidBody2D.AddForce(LaunchDirection * MoveSpeed);

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
    }
}