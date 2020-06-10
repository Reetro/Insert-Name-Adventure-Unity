using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Running Settings")]
    [SerializeField] float runSpeed = 8f;

    [Header("Jump Settings")]
    [SerializeField] float jumpForce = 10f;
    [SerializeField] Transform groundCheck = null;
    [SerializeField] float checkRadius = 0.5f;
    public LayerMask whatIsGround;
 

    Rigidbody2D myRigidbody = null;
    Animator myAnimator = null;

    bool isFacingRight = true;
    private float moveInput = 0f;
    private bool isGrounded;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        MovePlayer();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
    }

    private void MovePlayer()
    {
        moveInput = Input.GetAxis("Horizontal");

        myRigidbody.velocity = new Vector2(moveInput * runSpeed, 0f);

        if (!isFacingRight && moveInput > 0)
        {
            isFacingRight = !isFacingRight;
            GeneralFunctions.FlipObject(gameObject);
        }
        else if (isFacingRight && moveInput < 0)
        {
            isFacingRight = !isFacingRight;
            GeneralFunctions.FlipObject(gameObject);
        }

        myAnimator.SetBool("Running", GeneralFunctions.IsObjectMovingHorizontaly(gameObject));
    }

    private void Jump()
    {
        myAnimator.SetTrigger("Jump");
        myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpForce);
    }
}
