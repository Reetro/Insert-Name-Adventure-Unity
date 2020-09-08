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

        [Header("Layer Settings")]
        public LayerMask whatIsGround;  // A mask determining what is ground to the character

        [Header("Events")]
        [Space]

        public UnityEvent OnLandEvent;
        /// <summary>
        /// When object is spawned in get a reference to the player object
        /// </summary>
        private void Awake()
        {
            player = GeneralFunctions.GetPlayerGameObject();
        }
        /// <summary>
        /// Check to see if player is touching the ground every frame
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerStay2D(Collider2D collision)
        {
            bool wasGrounded = IsGrounded;
            IsGrounded = false;

            // Check to see if player is on the ground
            if (GeneralFunctions.IsObjectOnLayer(whatIsGround, collision.gameObject))
            {
                if (collision.gameObject != player)
                {
                    IsGrounded = true;
                    if (!wasGrounded)
                    {
                        OnLandEvent.Invoke();
                    }
                }
            }
            // See if the player is on a platform if so attach player to it
            else if (collision.gameObject.CompareTag("Platform"))
            {
                GeneralFunctions.AttachObjectToTransfrom(collision.transform, player);

                IsGrounded = true;
                if (!wasGrounded)
                {
                    OnLandEvent.Invoke();
                }
            }

        }
        /// <summary>
        /// If player was on a moving platform detach player from it
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerExit2D(Collider2D collision)
        {
            // Deattach player from a moving platform when they jump off
            if (collision.gameObject.CompareTag("Platform"))
            {
                GeneralFunctions.DetachFromParent(player);
            }
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
    }
}