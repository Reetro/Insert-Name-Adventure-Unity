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

        private BoxCollider2D boxCollider2D = null;
        private CapsuleCollider2D capsuleCollider2D = null;
        private SpriteRenderer spriteRenderer = null;
        private FixedJoint2D fixedJoint2D = null;

        [System.Serializable]
        public class OnSegmentDeath : UnityEvent<WormSegment> { }

        [HideInInspector]
        public OnSegmentDeath SegmentDeath;

        /// <summary>
        /// Set all internal worm segment values
        /// </summary>
        public void SetupWormSegment(int index, float health)
        {
            this.Index = index;

            MyHealthComponent.SetHealth(health);

            boxCollider2D = GetComponent<BoxCollider2D>();
            capsuleCollider2D = GetComponent<CapsuleCollider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            fixedJoint2D = GetComponent<FixedJoint2D>();
        }
        /// <summary>
        /// Disables collision and hides worm segment sprite's then invokes the segment death event
        /// </summary>
        protected override void OnDeath()
        {
            boxCollider2D.enabled = false;
            capsuleCollider2D.enabled = false;
            spriteRenderer.enabled = false;

            if (fixedJoint2D)
            {
                fixedJoint2D.enabled = false;
            }

            SegmentDeath.Invoke(this);
        }
    }
}
