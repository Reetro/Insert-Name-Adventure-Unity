using UnityEngine;
using UnityEngine.Events;

namespace PlayerCharacter.Controller
{
    public class PlayerLegs : MonoBehaviour
    {
        /// <summary>
        /// Checks to see if the player is touching the ground
        /// </summary>
        public bool IsGrounded { get; set; }

        const float k_GroundedRadius = .5f;

        private GameObject player = null;

        [Header("Leg Settings")]
        [Tooltip("What layers are considered to be ground to the legs")]
        public LayerMask whatIsGround;
        
        [HideInInspector]
        public UnityEvent OnLandEvent;

        [Header("Collision Settings")]
        [Tooltip("The size of the player ground collision box")]
        [SerializeField] private Vector2 boxSize = Vector2.zero;
        /// The draw location of the ground collision box
        private Transform collisionTransform = null;

        /// <summary>
        /// Get a ref to the child gameobject
        /// </summary>
        private void OnEnable()
        {
            collisionTransform = transform.GetChild(0).transform;
        }
        /// <summary>
        /// When object is spawned in get a reference to the player object
        /// </summary>
        private void Awake()
        {
            player = GeneralFunctions.GetPlayerGameObject();
        }
        /// <summary>
        /// Check to see if legs are colliding with anything
        /// </summary>
        private void FixedUpdate()
        {
            var collision = GetLegCollision();

            if (collision)
            {
                bool wasGrounded = IsGrounded;
                IsGrounded = false;

                if (collision.gameObject != player)
                {
                    IsGrounded = true;
                    if (!wasGrounded)
                    {
                        OnLandEvent.Invoke();
                    }
                }
            }
        }
        /// <summary>
        /// Draw a overlap box
        /// </summary>
        /// <returns>The first collider2D it overlaps</returns>
        public Collider2D GetLegCollision()
        {
            Collider2D collider2D = Physics2D.OverlapBox(collisionTransform.position, boxSize, 0f, whatIsGround);

            return collider2D;
        }
        /// <summary>
        /// Checks to see if the player is touching the ground layer
        /// </summary>
        public bool TouchingGround()
        {
            var hitGround = false;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, k_GroundedRadius);

            for (int index = 0; index < colliders.Length; index++)
            {
                if (colliders[index].gameObject.CompareTag("Ground"))
                {
                    hitGround = true;
                    break;
                }
                else
                {
                    hitGround = false;
                    continue;
                }
            }
            return hitGround;
        }
        /// <summary>
        /// Draw overlap box
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            Gizmos.DrawWireCube(collisionTransform.position, boxSize);
        }
    }
}