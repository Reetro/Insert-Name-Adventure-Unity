using UnityEngine;

namespace EnemyCharacter.AI
{
    public class WormMovement : EnemyBase
    {
        //[SerializeField] private float rotationSpeed = 15f;
        [SerializeField] private float damage = 1f;
        [SerializeField] private LayerMask whatIsGround = new LayerMask();
        [SerializeField] private bool drawDebug = false;

        private Quaternion targetRotation;
        private WormSegment[] childSegments;

        /// <summary>
        /// Get a random rotation to move towards then setup segment damage
        /// </summary>
        private void Start()
        {
            childSegments = GetComponentsInChildren<WormSegment>();
            foreach (WormSegment wormSegment in childSegments)
            {
                if (wormSegment)
                {
                    // Set worm segment vars
                    wormSegment.WhatIsGround = whatIsGround;
                    wormSegment.DamageToApply = damage;
                    wormSegment.DrawDebug = drawDebug;

                    wormSegment.CheckCollision();
                }
            }

            //targetRotation = Quaternion.Euler(0, 0, Random.Range(-360, 360));
        }

        private void Update()
        {
            //transform.localRotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}