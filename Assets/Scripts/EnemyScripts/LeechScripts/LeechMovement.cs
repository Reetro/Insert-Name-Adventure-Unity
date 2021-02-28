using System;
using EnemyScripts.BasicEnemyScripts;
using GeneralScripts;
using UnityEngine;

namespace EnemyScripts.LeechScripts
{
    [Serializable]
    public class LeechMovement : EnemyBase
    {
        [Header("Leech Movement Settings")]
        [Tooltip("How fast the leech flies towards the player")]
        [SerializeField] private float leechFlySpeed = 4f;
        [Tooltip("Minimum amount to add to the leeches current Y vector")]
        [SerializeField] private float randomYMin = 0.005f;
        [Tooltip("Maximum amount to add to the leeches current Y vector")]
        [SerializeField] private float randomYMax = 0.007f;

        private static readonly int IsDead = Animator.StringToHash("IsDead");

        /// <summary>
        /// Rotate the leech face the direction it's moving in
        /// </summary>
        private void Update()
        {
            if (GeneralFunctions.GetGameplayManager().IsGamePaused) return;
            if (MyHealthComponent.IsCurrentlyDead) return;
            
            if (MyRigidBody2D.velocity.magnitude > 0)
            {
                MyMovementComp.RotateToMovement(MyRigidBody2D);
            }

            var amountToAddToY = GeneralFunctions.CreateRandomVector2OnlyY(randomYMin, randomYMax);

            AddToLeechY(transform, amountToAddToY.y);
        }
        /// <summary>
        /// Moves the leech towards the player
        /// </summary>
        private void FixedUpdate()
        {
            if (!MyHealthComponent.IsCurrentlyDead)
            {
                MyMovementComp.MoveAITowards(PlayerTransform, MyRigidBody2D, leechFlySpeed);
            }
        }
        /// <summary>
        /// Starts the leech death animation
        /// </summary>
        protected override void OnDeath()
        {
            base.OnDeath();

            MyAnimator.SetBool(IsDead, true);

            EnemyMovement.StopMovement(MyRigidBody2D);
        }
        /// <summary>
        /// Called when the leach death animation is over destroys the leech game objects
        /// </summary>
        public void OnDeathAnimationEnd()
        {
            Destroy(gameObject);
        }
        /// <summary>
        /// Adds a given number to the given leech Y coordinate to give a bouncing effect
        /// </summary>
        /// <param name="leechTransfrom"></param>
        /// <param name="amountToAdd"></param>
        public void AddToLeechY(Transform leechTransfrom, float amountToAdd)
        {
            var temp = leechTransfrom.position;

            temp.y += amountToAdd;

            leechTransfrom.position = temp;
        }
    }
}