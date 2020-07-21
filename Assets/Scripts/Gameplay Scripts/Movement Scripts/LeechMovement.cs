using UnityEngine;
using System;

namespace EnemyCharacter.AI
{
    [Serializable]
    public class LeechMovement : EnemyBase
    {
        [Header("Leech Movement Settings")]
        [SerializeField] float leechFlySpeed = 4f;
        [SerializeField] float randomYMin = 0.005f;
        [SerializeField] float randomYMax = 0.007f;

        private void Update()
        {
            if (!MyHealthComponent.IsCurrentlyDead)
            {
                LookAtPlayer();

                var amountToAddToY = GeneralFunctions.CreateRandomVector2OnlyY(randomYMin, randomYMax);

                MovementComp.AddToLeechY(transform, amountToAddToY.y);
            }
        }

        private void FixedUpdate()
        {
            if (!MyHealthComponent.IsCurrentlyDead)
            {
                MovementComp.MoveAITowards(PlayerTransform, MyRigidBody2D, leechFlySpeed);
            }
        }

        public override void OnDeath()
        {
            base.OnDeath();

            MyAnimator.SetBool("IsDead", true);

            MovementComp.StopMovement(MyRigidBody2D);
        }

        public void OnDeathAnimationEnd()
        {
            Destroy(gameObject);
        }
    }
}