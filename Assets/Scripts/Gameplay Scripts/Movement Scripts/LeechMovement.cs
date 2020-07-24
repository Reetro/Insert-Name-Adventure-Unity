using UnityEngine;
using System;
using System.Collections;

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
        [Tooltip("Multiplier used to knock leech downward")]
        [SerializeField] private float forceMultipler = 0.8f;
        [Tooltip("Delay for downward force")]
        [SerializeField] private float pushTimer = 2f;
        [Tooltip("What layers can add force to leech")]
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
                RotateToMovement();

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

        protected override void OnDeath()
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

            if (!MyHealthComponent.IsCurrentlyDead)
            {
                MyRigidBody2D.AddForce(-transform.up * forceMultipler, ForceMode2D.Impulse);
            }
        }

        private bool CheckCollision()
        {
            RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, transform.up, 1f, ForceLayer);
            
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