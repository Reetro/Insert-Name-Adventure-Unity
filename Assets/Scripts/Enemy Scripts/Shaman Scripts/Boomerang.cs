using UnityEngine;
using LevelObjects.MovingObjects;
using System.Collections;

namespace EnemyCharacter.AI
{
    public class Boomerang : ProjectileMovement
    {
        private Shaman currentShaman = null;
        private int maxHitsBeforeTeleport = 0;
        private float offSet = 0.5f;
        private bool justSpawned = true;

        public int CurrentHitCount { get; set; }

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

        protected override void FixedUpdate()
        {
            CurrentVelocity = MyRigidBody2D.velocity;
        }

        private void TeleportShaman(Vector2 teleportLocation)
        {
            var newLocation = GetAdjustedTeleportLocation(teleportLocation, offSet);

            currentShaman.transform.position = newLocation;

            var lookAt = GeneralFunctions.LookAt2D(currentShaman.transform.position, transform.position);

            transform.rotation = lookAt;

            currentShaman.MyRigidBody2D.velocity = Vector2.zero;

            DestroyBoomerang(true, false);
        }

        public void DestroyBoomerang(bool throwBoomerang, bool randomThrow)
        {
            if (currentShaman)
            {
                if (throwBoomerang)
                {
                    currentShaman.ThrowBoomerang(randomThrow);
                }

                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("Unable to destroy boomerang " + name.ToString() + " currentShaman is invalid");
            }
        }

        public Vector2 GetAdjustedTeleportLocation(Vector2 teleportLocation, float offSet)
        {
            Ray2D ray = new Ray2D(teleportLocation, teleportLocation.normalized);
            ray.direction = -ray.direction;

            var point = ray.GetPoint(offSet);

            return point;
        }

        public void SetupBoomerang(Shaman shamanToSet, int maxHitsBeforeTeleport, float offSet)
        {
            justSpawned = true;

            currentShaman = shamanToSet;

            this.maxHitsBeforeTeleport = maxHitsBeforeTeleport;

            this.offSet = offSet;

            MyRigidBody2D.AddForce(LaunchDirection * MoveSpeed);

            StartCoroutine(SpawnTimer());
        }

        private IEnumerator SpawnTimer()
        {
            yield return new WaitForSeconds(0.1f);

            justSpawned = false;
        }

        public override void OnProjectileImpact()
        {
            // Do nothing on default impact
        }
    }
}