using System;
using GeneralScripts;
using GeneralScripts.GeneralComponents;
using UnityEngine;

namespace PlayerScripts.PlayerControls
{
    [Serializable]
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        [Tooltip("Amount of force added when the player jumps")]
        public float jumpForce = 400f;
        [Tooltip("How fast the player can run")]
        public float runSpeed = 35f;
        [Tooltip("How much to smooth out the movement")]
        [Range(0, .3f)] [SerializeField] private float movementSmoothing = 0.05f;
        [Tooltip("Whether or not a player can steer while jumping")]
        [SerializeField] private bool hasAirControl;
        [Tooltip("How fast the player accelerates")]
        [SerializeField] private float playerAcceleration = 10f;

        private Rigidbody2D myRigidbody2D;
        private bool isFacingRight = true; // For determining which way the player is currently facing.
        private HealthComponent hpComp;
        private PlayerLegs myLegs;
        private Vector3 currentVelocity = Vector3.zero;

        /// <summary>
        /// Set all internal values
        /// </summary>
        private void Awake()
        {
            myRigidbody2D = GetComponent<Rigidbody2D>();
            hpComp = GetComponent<HealthComponent>();
            myLegs = transform.GetComponentInChildren<PlayerLegs>();
        }
        /// <summary>
        /// Will move the player in the direction of input
        /// </summary>
        /// <param name="move"></param>
        /// <param name="jump"></param>
        /// <param name="forceFlip"></param>
        public void Move(float move, bool jump, bool forceFlip)
        {
            if (!hpComp.IsCurrentlyDead)
            {
                //only control the player if grounded or airControl is turned on
                if (myLegs.IsGrounded || hasAirControl)
                {
                    // Move the character by finding the target velocity
                    var velocity = myRigidbody2D.velocity;
                    Vector3 targetVelocity = new Vector2(move * runSpeed * playerAcceleration, velocity.y);
                    // And then smoothing it out and applying it to the character
                    myRigidbody2D.velocity = Vector3.SmoothDamp(velocity, targetVelocity, ref currentVelocity, movementSmoothing);

                    // If the input is moving the player right and the player is facing left...
                    if (move > 0 && !isFacingRight && forceFlip)
                    {
                        // ... flip the player.
                        // Switch the way the player is labeled as facing.
                        isFacingRight = !isFacingRight;

                        GeneralFunctions.FlipObject(gameObject);
                    }
                    // Otherwise if the input is moving the player left and the player is facing right...
                    else if (move < 0 && isFacingRight && forceFlip)
                    {
                        // Switch the way the player is labeled as facing.
                        isFacingRight = !isFacingRight;

                        GeneralFunctions.FlipObject(gameObject);
                    }
                }
                // If the player should jump...
                if (myLegs.IsGrounded && jump)
                {
                    // Add a vertical force to the player.
                    myLegs.IsGrounded = false;

                    // Remove all force towards player
                    myRigidbody2D.angularVelocity = 0f;
                    myRigidbody2D.velocity = Vector2.zero;

                    // Apply the actual jump force
                    myRigidbody2D.AddForce(new Vector2(0f, jumpForce));
                }
            }
        }
        /// <summary>
        /// Completely stop all current movement on player will also completely freeze all incoming movement then sleeps the player rigidbody 
        /// </summary>
        public void StopMovement()
        {
            myRigidbody2D.angularVelocity = 0;
            myRigidbody2D.velocity = Vector2.zero;
            currentVelocity = Vector3.zero;

            myRigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;

            myRigidbody2D.Sleep();
        }
        /// <summary>
        /// Will reset player movement and wake up the players rigidbody
        /// </summary>
        public void ResetMovement()
        {
            myRigidbody2D.WakeUp();

            myRigidbody2D.constraints = RigidbodyConstraints2D.None;

            myRigidbody2D.freezeRotation = true;
        }
    }
}