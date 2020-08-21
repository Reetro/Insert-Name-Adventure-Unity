using UnityEngine;

namespace EnemyCharacter.AI
{
    public class WormMovement : EnemyBase
    {
        [SerializeField] private float damage = 1f;
        [SerializeField] private LayerMask whatIsGround = new LayerMask();
        [SerializeField] private bool drawDebug = false;

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

                    //wormSegment.CheckCollision();
                }
            }
        }

        private void Update()
        {
            
        }
    }
}