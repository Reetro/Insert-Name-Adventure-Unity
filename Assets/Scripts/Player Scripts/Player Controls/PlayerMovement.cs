using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
	[Header("Movement Settings")]
	[SerializeField] private float jumpForce = 400f;                             // Amount of force added when the player jumps.		
    [SerializeField] private float runSpeed = 35f;
    [Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;	 // How much to smooth out the movement
	[SerializeField] private bool hasAirControl = false;                         // Whether or not a player can steer while jumping;
	[SerializeField] private float playerAcceleration = 10f;

	private Rigidbody2D myRigidbody2D = null;
	private bool isFacingRight = true; // For determining which way the player is currently facing.
	private Vector3 currentVelocity = Vector3.zero;
	private HealthComponent hpComp;
    private PlayerLegs myLegs = null;

	private void Start()
	{
		myRigidbody2D = GetComponent<Rigidbody2D>();
		hpComp = GetComponent<HealthComponent>();
        myLegs = GameObject.FindGameObjectWithTag("Player Legs").GetComponent<PlayerLegs>();
    }

    public void Move(float move, bool jump, bool forceFlip)
	{
		if (!hpComp.GetIsDead())
        {
            //only control the player if grounded or airControl is turned on
            if (myLegs.GetIsGrounded() || hasAirControl)
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
            if (myLegs.GetIsGrounded() && jump)
            {
                // Add a vertical force to the player.
                myLegs.UpdateGrounded(false);
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
