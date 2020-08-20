using UnityEngine;
using System;

namespace EnemyCharacter.AI
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

        private void Update()
        {
            if (!MyHealthComponent.IsCurrentlyDead)
            {
                if (MyRigidBody2D.velocity.magnitude > 0)
                {
                    MyMovementComp.RotateToMovement(MyRigidBody2D);
                }

                var amountToAddToY = GeneralFunctions.CreateRandomVector2OnlyY(randomYMin, randomYMax);

                MyMovementComp.AddToLeechY(transform, amountToAddToY.y);
            }
        }

        private void FixedUpdate()
        {
            if (!MyHealthComponent.IsCurrentlyDead)
            {
                MyMovementComp.MoveAITowards(PlayerTransform, MyRigidBody2D, leechFlySpeed);
            }
        }

        protected override void OnDeath()
        {
            base.OnDeath();

            MyAnimator.SetBool("IsDead", true);

            MyMovementComp.StopMovement(MyRigidBody2D);
        }

        public void OnDeathAnimationEnd()
        {
            Destroy(gameObject);
        }
    }
}