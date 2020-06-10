using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Running Settings")]
    [SerializeField] float runSpeed = 2f;

    [Header("Jump Settings")]
    [SerializeField] float jumpForce = 10f;

    Rigidbody2D myRigidbody = null;
    BoxCollider2D myCollision = null;
    Animator myAnimator = null;

    bool isFacingRight = true;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myCollision = GetComponent<BoxCollider2D>();
        myAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        MovePlayer();
        Jump();
    }

    private void MovePlayer()
    {
        float controlThrow = Input.GetAxis("Horizontal");
        float moveSpeed = controlThrow * runSpeed;
        Vector2 playerVelocity = new Vector2(moveSpeed, myRigidbody.velocity.y);

        myRigidbody.velocity = playerVelocity;

        if (controlThrow < 0 && isFacingRight)
        {
            FlipSprite();
        }
        if (controlThrow > 0 && !isFacingRight)
        {
            FlipSprite();
        }    

        myAnimator.SetBool("Running", GeneralFunctions.IsObjectMovingHorizontaly(gameObject));
        Debug.Log(GeneralFunctions.IsObjectMovingHorizontaly(gameObject));
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && myCollision.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            myAnimator.SetTrigger("Jump");

            // Todo Redo jump code
        }
    }

    private void FlipSprite()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(Vector3.up * 180);
    }
}
