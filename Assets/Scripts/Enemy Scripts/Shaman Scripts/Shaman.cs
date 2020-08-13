using UnityEngine;
using System;
using LevelObjects.MovingObjects;

namespace EnemyCharacter.AI
{
    [Serializable]
    public class Shaman : EnemyBase
    {
        [Header("Boomerang settings")]
        [Tooltip("How many hits it takes to make the shaman teleport")]
        [SerializeField] private int maxHitsBeforeTeleport = 2;
        [Tooltip("How fast the spawn boomerang moves")]
        [SerializeField] private float boomerangSpeed = 300f;
        [Tooltip("How much damage the spawned boomerang does")]
        [SerializeField] private float boomerangDamage = 2f;
        [Tooltip("How much to offset the shaman teleport location by")]
        [SerializeField] private float teleportOffset = 0.5f;
        [Tooltip("Speed multiplier used when shaman throws the boomerang")]
        [SerializeField] private float bommerangSpeedMultipler = 2f;
        [Tooltip("The maximum speed the boomerang can go")]
        [SerializeField] private float bommerangMaxSpeedMagnitude = 8f;
        [Tooltip("Boomerang Asset to spawn")]
        [SerializeField] private Boomerang boomerangToSpawn = null;
        /// <summary>
        /// The current boomerang spawning the world
        /// </summary>
        private Boomerang currentBoomrang = null;
        /// <summary>
        /// Throw boomerang when game starts
        /// </summary>
        private void Start()
        {
            ThrowBoomerang();
        }
        /// <summary>
        /// Throw boomerang in direction of the player
        /// </summary>
        public void ThrowBoomerang()
        {
            var playerLocation = GeneralFunctions.GetPlayerGameObject().transform.position;

            currentBoomrang = Instantiate(boomerangToSpawn, transform.position, transform.rotation);

            var startDirection = GeneralFunctions.GetDirectionVectorFrom2Vectors(playerLocation, currentBoomrang.transform.position);

            currentBoomrang.ConstructProjectile(boomerangSpeed * bommerangSpeedMultipler, boomerangDamage, startDirection);

            currentBoomrang.SetupBoomerang(this, maxHitsBeforeTeleport, teleportOffset, bommerangMaxSpeedMagnitude);

            currentBoomrang.CurrentHitCount = 0; 
        }
        /// <summary>
        /// Destroy thrown boomerang on death and this Gameobject
        /// </summary>
        protected override void OnDeath()
        {
            base.OnDeath();

            if (currentBoomrang)
            {
                currentBoomrang.DestroyBoomerang(false);
            }

            Destroy(gameObject);
        }
    }
}