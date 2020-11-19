using UnityEngine;
using UnityEngine.Events;

namespace EnemyCharacter.AI
{
    [RequireComponent(typeof(BoxCollider2D), typeof(CapsuleCollider2D), typeof(SpriteRenderer))]
    public class WormSegment : EnemyBase
    {
        /// <summary>
        /// position of this segment in the worm movement segment array
        /// </summary>
        public int Index { get; private set; } = 0;
        /// <summary>
        /// Check to see if the segment is above ground
        /// </summary>
        public bool AboveGround { get; private set; } = false;

        private BoxCollider2D boxCollider2D = null;
        private CapsuleCollider2D capsuleCollider2D = null;
        private SpriteRenderer spriteRenderer = null;
        private LayerMask whatIsGround = new LayerMask();

        [System.Serializable]
        public class OnSegmentDeath : UnityEvent<WormSegment> { }

        [HideInInspector]
        public OnSegmentDeath SegmentDeath;

        /// <summary>
        /// Set all internal worm segment values
        /// </summary>
        public void SetupWormSegment(int index, float health, LayerMask whatIsGround)
        {
            Index = index;

            MyHealthComponent.SetHealth(health);

            this.whatIsGround = whatIsGround;

            boxCollider2D = GetComponent<BoxCollider2D>();
            capsuleCollider2D = GetComponent<CapsuleCollider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        /// <summary>
        /// Disables collision and hides worm segment sprite's then invokes the segment death event
        /// </summary>
        protected override void OnDeath()
        {
            spriteRenderer.enabled = false;

            DisableCollision();

            SegmentDeath.Invoke(this);
        }

        #region Collision Functions
        /// <summary>
        /// Toggle collision between the worm segment layer and ground
        /// </summary>
        public void ToggleGroundCollision(bool collide)
        {
            var localCollide = !collide;

            var segmentLayer = 22;
            var groundLayer = 8;

            Physics2D.IgnoreLayerCollision(segmentLayer, groundLayer, localCollide);
        }
        /// <summary>
        /// Enables both collision and fixed joint 2D then makes the rigidbody dynamic
        /// </summary>
        public void EnableCollision()
        {
            ToggleGroundCollision(true);

            boxCollider2D.enabled = true;
            capsuleCollider2D.enabled = true;

            MyRigidBody2D.constraints = RigidbodyConstraints2D.None;
        }
        /// <summary>
        /// Disables both collision and fixed joint 2D then freeze rotation and position
        /// </summary>
        public void DisableCollision()
        {
            ToggleGroundCollision(false);

            boxCollider2D.enabled = false;
            capsuleCollider2D.enabled = false;

            MyRigidBody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        /// <summary>
        /// Check to see if this segment is overlapping any ground
        /// </summary>
        public bool IsOverlappingGround()
        {
            Collider2D collider2D = Physics2D.OverlapCapsule(transform.position, capsuleCollider2D.size, capsuleCollider2D.direction, GeneralFunctions.GetObjectEulerAngle(gameObject), whatIsGround);

            if (collider2D)
            {
                AboveGround = false;

                return true;
            }
            else
            {
                AboveGround = true;

                return false;
            }
        }
        #endregion
    }
}