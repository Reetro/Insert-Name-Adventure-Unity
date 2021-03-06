﻿using System.Collections;
using GameplayScripts.GeneralMovementScripts;
using GeneralScripts;
using UnityEngine;

namespace EnemyScripts.ShamanScripts
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
        private float maxSpeedMagnitude = 8;
        /// <summary>
        /// Determines if the boomerang can damage the player
        /// </summary>
        private bool canDamage = true;
        /// <summary>
        /// Checks to see if the shaman can actually teleport to the given location
        /// </summary>
        private bool canShamanTeleport = true;
        #endregion

        #region Movement Functions
        /// <summary>
        /// Reflect the projectile when ever it hits a surface
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            var wallNormal = collision.GetContact(0).normal;

            var newDirection = Vector2.Reflect(CurrentVelocity, wallNormal);

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

            if (CurrentHitCount < maxHitsBeforeTeleport) return;
            
            TeleportShaman(collision.GetContact(0).point);

            CurrentHitCount = 0;
        }
        /// <summary>
        /// Keep tracks of the current velocity the projectile has
        /// </summary>
        protected override void FixedUpdate()
        {
            if (MyRigidBody2D.velocity.magnitude >= maxSpeedMagnitude || MyRigidBody2D.velocity.magnitude <= maxSpeedMagnitude)
            {
                MyRigidBody2D.velocity = MyRigidBody2D.velocity.normalized * maxSpeedMagnitude;
            }

            CurrentVelocity = MyRigidBody2D.velocity;
        }
        /// <summary>
        /// If the circle collider overlaps with ground destroy the current boomerang and spawn in a new one
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (!GeneralFunctions.IsObjectOnLayer("Ground", collision.gameObject) && !collision.gameObject.CompareTag("Shaman")) return;
            
            if (justSpawned) return;
            canShamanTeleport = false;

            DestroyBoomerang(true);
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
            if (!canShamanTeleport) return;
            
            var newLocation = GetAdjustedTeleportLocation(teleportLocation, offSet);

            var transform1 = currentShaman.transform;
            transform1.position = newLocation;

            var lookAt = GeneralFunctions.LookAt2D(transform1.position, transform.position);

            transform.rotation = lookAt;

            currentShaman.MyRigidBody2D.velocity = Vector2.zero;

            DestroyBoomerang(true);
        }
        /// <summary>
        /// Destroys this boomerang and can tell the shaman to throw a new one
        /// </summary>
        /// <param name="throwBoomerang"></param>
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
                Debug.LogError("Unable to destroy boomerang " + name + " currentShaman is invalid");
            }
        }
        /// <summary>
        /// Fire a ray2D at the given location
        /// </summary>
        /// <param name="teleportLocation"></param>
        /// <param name="offSet"></param>
        /// <returns>The end point of the ray</returns>
        private static Vector2 GetAdjustedTeleportLocation(Vector2 teleportLocation, float offSet)
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
        /// <param name="maxHitsForTeleport"></param>
        /// <param name="newOffSet"></param>
        /// <param name="newMaxSpeed"></param>
        /// <param name="newDamageDelay"></param>
        public void SetupBoomerang(Shaman shamanToSet, int maxHitsForTeleport, float newOffSet, float newMaxSpeed, float newDamageDelay)
        {
            justSpawned = true;

            currentShaman = shamanToSet;

            maxSpeedMagnitude = newMaxSpeed;

            maxHitsBeforeTeleport = maxHitsForTeleport;

            offSet = newOffSet;

            MyRigidBody2D.AddForce(LaunchDirection * MoveSpeed);

            DamageDelay = newDamageDelay;

            canDamage = true;

            canShamanTeleport = true;

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