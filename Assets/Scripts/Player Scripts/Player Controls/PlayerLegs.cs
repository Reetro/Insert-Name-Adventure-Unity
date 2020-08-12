using UnityEngine;
using UnityEngine.Events;

namespace PlayerCharacter.Controller
{
    public class PlayerLegs : MonoBehaviour
    {
        [HideInInspector]
        public bool isGrounded = true;
        const float k_GroundedRadius = .5f;

        private GameObject player = null;

        [Header("Layer Settings")]
        public LayerMask whatIsGround;  // A mask determining what is ground to the character

        [Header("Events")]
        [Space]

        public UnityEvent OnLandEvent;

        private void Awake()
        {
            player = GeneralFunctions.GetPlayerGameObject();
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            bool wasGrounded = isGrounded;
            isGrounded = false;

            // Check to see if player is on the ground
            if (GeneralFunctions.IsObjectOnLayer(whatIsGround, collision.gameObject))
            {
                if (collision.gameObject != player)
                {
                    isGrounded = true;
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

                isGrounded = true;
                if (!wasGrounded)
                {
                    OnLandEvent.Invoke();
                }
            }

        }

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