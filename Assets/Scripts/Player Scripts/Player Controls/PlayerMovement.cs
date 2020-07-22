using System;
using UnityEngine;

namespace PlayerCharacter.Controller
{
    [Serializable]
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        [Tooltip("Amount of force added when the player jumps")]
        [SerializeField] private float jumpForce = 400f;
        [Tooltip("How fast the player can run")]
        [SerializeField] private float runSpeed = 35f;
        [Tooltip("How much to smooth out the movement")]
        [Range(0, .3f)] [SerializeField] private float movementSmoothing = 0.05f;
        [Tooltip("Whether or not a player can steer while jumping")]
        [SerializeField] private bool hasAirControl = false;
        [Tooltip("How fast the player accelerates")]
        [SerializeField] private float playerAcceleration = 10f;

        private Rigidbody2D myRigidbody2D = null;
        private bool isFacingRight = true; // For determining which way the player is currently facing.
        private Vector3 currentVelocity = Vector3.zero;
        private HealthComponent hpComp;
        private PlayerLegs myLegs = null;

        private void Awake()
        {
            myRigidbody2D = GetComponent<Rigidbody2D>();
            hpComp = GetComponent<HealthComponent>();
            myLegs = GameObject.FindGameObjectWithTag("Player Legs").GetComponent<PlayerLegs>();
        }

        public void Move(float move, bool jump, bool forceFlip)
        {
            if (!hpComp.IsCurrentlyDead)
            {
                //only control the player if grounded or airControl is turned on
                if (myLegs.isGrounded || hasAirControl)
                {
                    // Move the character by finding the target velocity
                    Vector3 targetVelocity = new Vector2(move * runSpeed * playerAcceleration, myRigidbody2D.velocity.y);
                    // And then smoothing it out and applying it to the character
                    myRigidbody2D.velocity = Vector3.SmoothDamp(myRigidbody2D.velocity, targetVelocity, ref currentVelocity, movementSmoothing);

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
                if (myLegs.isGrounded && jump)
                {
                    // Add a vertical force to the player.
                    myLegs.isGrounded = false;
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
        public void ResetMovment()
        {
            myRigidbody2D.WakeUp();

            myRigidbody2D.constraints = RigidbodyConstraints2D.None;

            myRigidbody2D.freezeRotation = true;
        }
    }
}