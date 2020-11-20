using UnityEngine;
using UnityEngine.Events;
using StatusEffects;

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
        /// <summary>
        /// Checks to see if the segment is falling
        /// </summary>
        public bool IsFalling { get; set; } = false;
        /// <summary>
        /// Checks to see if the player is currently squished
        /// </summary>
        public bool IsPlayerSquished { get; set; } = false;

        private BoxCollider2D boxCollider2D = null;
        private CapsuleCollider2D capsuleCollider2D = null;
        private SpriteRenderer spriteRenderer = null;
        private LayerMask whatIsGround = new LayerMask();
        private ScriptableStatusEffect effectToApply = null;
        private Vector3 defaultPlayerScale = Vector3.zero;
        private Vector3 newPlayerScale = Vector3.zero;
        private float defaultOpacity = 0f;
        private float damageToApply = 0f;

        [System.Serializable]
        public class OnSegmentDeath : UnityEvent<WormSegment> { }

        [HideInInspector]
        public OnSegmentDeath SegmentDeath;

        [HideInInspector]
        public UnityEvent onSquishPlayer;

        [HideInInspector]
        public UnityEvent onUnSquishPlayer;

        /// <summary>
        /// Set all internal worm segment values
        /// </summary>
        public void SetupWormSegment(int index, float health, LayerMask whatIsGround, ScriptableStatusEffect effectToApply, Vector3 newPlayerScale, float damageToApply)
        {
            Index = index;

            MyHealthComponent.SetHealth(health);

            this.whatIsGround = whatIsGround;

            this.effectToApply = effectToApply;

            this.newPlayerScale = newPlayerScale;

            this.damageToApply = damageToApply;

            boxCollider2D = GetComponent<BoxCollider2D>();
            capsuleCollider2D = GetComponent<CapsuleCollider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();

            defaultOpacity = spriteRenderer.color.a;
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
        /// Enables collision between ground and segment
        /// </summary>
        public void EnableCollision()
        {
            ToggleGroundCollision(true);

            boxCollider2D.enabled = true;
            capsuleCollider2D.enabled = true;
        }
        /// <summary>
        /// Disables collision between ground and segment
        /// </summary>
        public void DisableCollision()
        {
            ToggleGroundCollision(false);

            boxCollider2D.enabled = false;
            capsuleCollider2D.enabled = false;
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
        /// <summary>
        /// When player is hit by a segment squish the player
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (IsFalling)
            {
                if (GeneralFunctions.IsObjectOnLayer("Player", collision.gameObject))
                {
                    defaultPlayerScale = collision.gameObject.transform.localScale;

                    SquishPlayer(collision.gameObject);
                }
            }
        }
        /// <summary>
        /// Slow Player and set new scale
        /// </summary>
        private void SquishPlayer(GameObject player)
        {
            if (!IsPlayerSquished)
            {
                effectToApply.EffectEnd.AddListener(UnSquishPlayer);

                player.transform.localScale = newPlayerScale;

                GeneralFunctions.ApplyStatusEffectToTarget(player, effectToApply);

                GeneralFunctions.DamageTarget(player, damageToApply, true, gameObject);

                GeneralFunctions.GetPlayerSpear().DisableSpear();

                onSquishPlayer.Invoke();
            }
        }
        /// <summary>
        /// Called when squish debuff ends
        /// </summary>
        private void UnSquishPlayer(GameObject gameObject)
        {
            GeneralFunctions.GetPlayerGameObject().transform.localScale = defaultPlayerScale;

            GeneralFunctions.GetPlayerSpear().EnableSpear();

            onUnSquishPlayer.Invoke();
        }
        /// <summary>
        /// Set the segment opacity the the given value
        /// </summary>
        /// <param name="newOpacity"></param>
        public void SetSpriteOpacity(float newOpacity)
        {
            Color tmp = spriteRenderer.color;

            tmp.a = newOpacity;

            spriteRenderer.color = tmp;
        }
        /// <summary>
        /// Set the segment opacity the the given value if no value defaults to original opacity
        /// </summary>
        /// <param name="newOpacity"></param>
        public void SetSpriteOpacity()
        {
            Color tmp = spriteRenderer.color;

            tmp.a = defaultOpacity;

            spriteRenderer.color = tmp;
        }
        #endregion
    }
}