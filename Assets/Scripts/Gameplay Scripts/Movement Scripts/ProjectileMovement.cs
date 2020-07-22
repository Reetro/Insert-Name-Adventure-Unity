﻿using UnityEngine;
using UnityEngine.Events;

namespace LevelObjects.MovingObjects
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ProjectileMovement : MonoBehaviour
    {
        private float currentMoveSpeed = 400f;
        private Vector2 currentVelocity;

        private Vector2 launchDirection;
        protected bool canFire = false;
        protected bool useNoise = false;
        protected float damage = 1f;

        private float minNoise = 0f;
        private float maxNoise = 0f;
        private Vector2 lastDirection = Vector2.zero;

        public Vector2 LastDirection { get { return lastDirection; } }
        public Vector2 LaunchDirection { get { return launchDirection; } }

        [Header("Events")]
        [Space]
        public UnityEvent OnImpact;

        protected virtual void Awake()
        {
            MyRigidBody2D = GetComponent<Rigidbody2D>();
        }

        public virtual void ConstructProjectile(float moveSpeed, float damage, Vector2 launchDirection)
        {
            this.damage = damage;
            currentMoveSpeed = moveSpeed;
            this.launchDirection = launchDirection;

            canFire = true;
            useNoise = false;
        }

        public virtual void ConstructProjectileWithNoise(float moveSpeed, float damage, Vector2 launchDirection, float noiseMin, float noiseMax)
        {
            this.damage = damage;
            currentMoveSpeed = moveSpeed;
            this.launchDirection = launchDirection + GetNoise(noiseMin, noiseMax);

            minNoise = noiseMin;
            maxNoise = noiseMax;

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

        public Vector2 CurrentVelocity { get { return currentVelocity; } }

        public Rigidbody2D MyRigidBody2D { get; private set; } = null;

        protected virtual void FixedUpdate()
        {
            currentVelocity = MyRigidBody2D.velocity;

            if (canFire && useNoise)
            {
                var newDirection = launchDirection.normalized + GetNoise(minNoise, maxNoise);

                MyRigidBody2D.velocity = newDirection * currentMoveSpeed * Time.fixedDeltaTime;
            }

            if (canFire)
            {
                MyRigidBody2D.velocity = launchDirection.normalized * currentMoveSpeed * Time.fixedDeltaTime;
            }
        }

        public void OnProjectileImpact()
        {
            Destroy(gameObject);
        }

        public Vector2 GetNoise(float min, float max)
        {
            // Find random angle between min & max inclusive
            float xNoise = Random.Range(min, max);
            float yNoise = Random.Range(min, max);

            Vector2 noise = new Vector2(2 * Mathf.PI * xNoise / 360, 2 * Mathf.PI * yNoise / 360);

            return noise;
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