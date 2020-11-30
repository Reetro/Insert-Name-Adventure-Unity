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
        /// <summary>
        /// The current object under the player legs
        /// </summary>
        public GameObject ObjectUnderLeg { get; private set; }

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
                ObjectUnderLeg = collision.gameObject;

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
        /// <summary>
        /// Draw overlap box
        /// </summary>
        private void OnDrawGizmos()
        {
            collisionTransform = transform.GetChild(0).transform;

            Gizmos.color = Color.green;

            Gizmos.DrawWireCube(collisionTransform.position, boxSize);
        }
    }
}