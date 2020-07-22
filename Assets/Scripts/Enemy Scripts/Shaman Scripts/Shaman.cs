using UnityEngine;
using System;

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
        [Tooltip("Minimum amount of noise to add the Initial spawn of the boomerang")]
        [SerializeField] private float MinNoise = 300f;
        [Tooltip("Maximum amount of noise to add the Initial spawn of the boomerang")]
        [SerializeField] private float MaxNoise = 600f;
        [Tooltip("How much to offset the shaman teleport location by")]
        [SerializeField] private float teleportOffset = 0.5f;

        private Boomerang boomerangToSpawn = null;
        private Boomerang currentBoomrang = null;

        protected override void Awake()
        {
            base.Awake();

            var boomerangObject = Resources.Load("Enemy Projectiles/Boomerang") as GameObject;

            boomerangToSpawn = boomerangObject.GetComponent<Boomerang>();
        }

        private void Start()
        {
            ThrowBoomerang();
        }

        public void ThrowBoomerang()
        {
            var playerLocation = GeneralFunctions.GetPlayerGameObject().transform.position;

            currentBoomrang = Instantiate(boomerangToSpawn, transform.position, transform.rotation);

            var startDirection = GeneralFunctions.GetDirectionVectorFrom2Vectors(playerLocation, currentBoomrang.transform.position);

            currentBoomrang.ConstructProjectileWithNoise(boomerangSpeed, boomerangDamage, startDirection, MinNoise, MaxNoise);

            currentBoomrang.SetCurrentShaman(this, maxHitsBeforeTeleport, teleportOffset);
        }

        public override void OnDeath()
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