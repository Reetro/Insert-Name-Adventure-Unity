using UnityEngine;
using System;
using System.Collections;

namespace EnemyCharacter.AI
{
    [Serializable]
    public class LeechMovement : EnemyBase
    {
        [Header("Leech Movement Settings")]
        [SerializeField] private float leechFlySpeed = 4f;
        [SerializeField] private float randomYMin = 0.005f;
        [SerializeField] private float randomYMax = 0.007f;
        [SerializeField] private float downwardForceMultipler = 0.8f;
        [SerializeField] private float pushTimer = 2f;
        public LayerMask ForceLayer;

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (CheckCollision())
            {
                StartCoroutine(ApplyDownForce());
            }
        }

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

        private IEnumerator ApplyDownForce()
        {
            yield return new WaitForSeconds(pushTimer);

            MyRigidBody2D.AddForce(-transform.up * downwardForceMultipler, ForceMode2D.Impulse);
        }

        private bool CheckCollision()
        {
            RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, transform.up, 1f, ForceLayer);

            Debug.DrawRay(transform.position, transform.up * 1f, Color.red);
            
            if (raycastHit2D)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}