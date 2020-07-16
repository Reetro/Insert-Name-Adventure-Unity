using UnityEngine;

namespace EnemyCharacter.AI
{
    public class Boomerang : ProjectileMovement
    {
        private Shaman currentShaman = null;
        private int currentHitCount = 0;
        private int maxHitsBeforeTeleport = 0;
        private float offSet = 0.5f;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Vector2 _wallNormal = collision.GetContact(0).normal;

            Vector2 newDirection = Vector2.Reflect(CurrentVelocity, _wallNormal);

            UpdateDirection(newDirection);

            currentHitCount++;

            GeneralFunctions.DamageTarget(collision.gameObject, damage, true);

            if (currentHitCount >= maxHitsBeforeTeleport)
            {
                TeleportShaman(collision.GetContact(0).point);

                currentHitCount = 0;
            }
        }

        private void TeleportShaman(Vector2 teleportLocation)
        {
            var newLocation = GetAdjustedTeleportLocation(teleportLocation, offSet);

            print(newLocation);

            currentShaman.transform.position = newLocation;

            GeneralFunctions.LookAt2D(currentShaman.transform.position, transform.position, currentShaman.gameObject);

            currentShaman.MyRigidBody2D.velocity = new Vector2(0, 0);
        }

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
                Debug.LogError("Unable to destroy boomerang " + name + "currentShaman is invalid");
            }
        }

        public Vector2 GetAdjustedTeleportLocation(Vector2 teleportLocation, float offSet)
        {
            Ray2D ray = new Ray2D(teleportLocation, teleportLocation.normalized);
            ray.direction = -ray.direction;

            var point = ray.GetPoint(offSet);

            return point;
        }

        public void SetCurrentShaman(Shaman shamanToSet, int maxHitsBeforeTeleport, float offSet)
        {
            currentShaman = shamanToSet;

            this.maxHitsBeforeTeleport = maxHitsBeforeTeleport;

            this.offSet = offSet;
        }
    }
}