using UnityEngine;
using System;

namespace EnemyCharacter.AI
{
    [Serializable]
    public class Shaman : EnemyBase
    {
        [Header("Boomerang settings")]
        [SerializeField] private int maxHitsBeforeTeleport = 2;
        [SerializeField] private float boomerangSpeed = 300f;
        [SerializeField] private float boomerangDamage = 2f;
        [SerializeField] private float MinNoise = 300f;
        [SerializeField] private float MaxNoise = 600f;
        [SerializeField] private float teleportOffset = 0.5f;
        [SerializeField] private float hitTimerLength = 0.05f;

        private Boomerang boomerangToSpawn = null;
        private Boomerang currentBoomrang = null;

        protected override void Awake()
        {
            base.Awake();

            var boomerangObject = Resources.Load("Enemy/Boomerang") as GameObject;

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

            currentBoomrang.SetCurrentShaman(this, maxHitsBeforeTeleport, teleportOffset, hitTimerLength);

            var startDirection = GeneralFunctions.GetDirectionVectorFrom2Vectors(playerLocation, currentBoomrang.transform.position);

            currentBoomrang.ConstructProjectileWithNoise(boomerangSpeed, boomerangDamage, startDirection, MinNoise, MaxNoise);

            currentBoomrang.UpdateVelocity(playerLocation - currentBoomrang.transform.position);
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