using System;
using EnemyScripts.BasicEnemyScripts;
using GeneralScripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace EnemyScripts.ShamanScripts
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
        [FormerlySerializedAs("DamageDelay")]
        [Tooltip("Once the boomerang has damaged the player this delay is used to determine when damage can be applied again")]
        [SerializeField] private float damageDelay = 1f;
        [Tooltip("Boomerang Asset to spawn")]
        [SerializeField] private Boomerang boomerangToSpawn = null;
        /// <summary>
        /// The current boomerang spawning the world
        /// </summary>
        private Boomerang currentBoomerang = null;
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

            var transform1 = transform;
            currentBoomerang = Instantiate(boomerangToSpawn, transform1.position, transform1.rotation);

            var startDirection = GeneralFunctions.GetDirectionVectorFrom2Vectors(playerLocation, currentBoomerang.transform.position);

            currentBoomerang.ConstructProjectile(boomerangSpeed * bommerangSpeedMultipler, boomerangDamage, startDirection);

            currentBoomerang.SetupBoomerang(this, maxHitsBeforeTeleport, teleportOffset, bommerangMaxSpeedMagnitude, damageDelay);

            currentBoomerang.CurrentHitCount = 0;
        }
        /// <summary>
        /// Destroy thrown boomerang on death and this Gameobject
        /// </summary>
        protected override void OnDeath()
        {
            base.OnDeath();

            if (currentBoomerang)
            {
                currentBoomerang.DestroyBoomerang(false);
            }

            Destroy(gameObject);
        }
    }
}