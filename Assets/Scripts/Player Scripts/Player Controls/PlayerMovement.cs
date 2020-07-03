using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
	[Header("Movement Settings")]
	[SerializeField] private float m_JumpForce = 400f;							// Amount of force added when the player jumps.		
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
	[SerializeField] private float playerAcceleration = 10f;

	[Header("Layer Settings")]
	public LayerMask m_WhatIsGround;											// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck = null;					// A position marking where to check if the player is grounded.

	[SerializeField] float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	private Rigidbody2D m_Rigidbody2D = null;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;
	private Vector3 defaultScale;
	private HealthComponent hpComp;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	private void Start()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		hpComp = GetComponent<HealthComponent>();

		defaultScale = transform.localScale;
	}

	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a circle cast to the ground check position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int index = 0; index < colliders.Length; index++)	
		{
			if (colliders[index].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}
	}

	public void Move(float move, bool jump, bool forceFlip)
	{
		if (!hpComp.GetIsDead())
        {
            //only control the player if grounded or airControl is turned on
            if (m_Grounded || m_AirControl)
            {
                // Move the character by finding the target velocity
                Vector3 targetVelocity = new Vector2(move * playerAcceleration, m_Rigidbody2D.velocity.y);
                // And then smoothing it out and applying it to the character
                m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !m_FacingRight && forceFlip)
                {
                    // ... flip the player.
                    // Switch the way the player is labeled as facing.
                    m_FacingRight = !m_FacingRight;

                    GeneralFunctions.FlipObject(gameObject);
                }
                // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && m_FacingRight && forceFlip)
                {
                    // Switch the way the player is labeled as facing.
                    m_FacingRight = !m_FacingRight;

                    GeneralFunctions.FlipObject(gameObject);
                }
            }
            // If the player should jump...
            if (m_Grounded && jump)
            {
                // Add a vertical force to the player.
                m_Grounded = false;
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            }
        }
	}
	// Attach player to a moving platform
    private void OnTriggerStay2D(Collider2D collision)
    {
		if (collision.gameObject.CompareTag("Platform"))
        {
            GeneralFunctions.AttachObjectToTransfrom(collision.transform, gameObject);
        }
    }
	// Deattach player from a moving platform when they jump off
    private void OnTriggerExit2D(Collider2D collision)
    {
		if (collision.gameObject.CompareTag("Platform"))
        {
            GeneralFunctions.DetachFromParent(gameObject);

			gameObject.transform.localScale = defaultScale;
        }
    }
	/// <summary>
	/// Completely stop all current movement on player will also completely freeze all incoming movement then sleeps the player rigidbody 
	/// </summary>
    public void StopMovement()
    {
		m_Rigidbody2D.angularVelocity = 0;
		m_Rigidbody2D.velocity = Vector2.zero;
		m_Velocity = Vector3.zero;

		m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;

		m_Rigidbody2D.Sleep();
    }
	/// <summary>
	/// Will reset player movement and wake up the players rigidbody
	/// </summary>
	public void ResetMovment()
    {
		m_Rigidbody2D.WakeUp();

		m_Rigidbody2D.constraints = RigidbodyConstraints2D.None;

		m_Rigidbody2D.freezeRotation = true;
	}
}
