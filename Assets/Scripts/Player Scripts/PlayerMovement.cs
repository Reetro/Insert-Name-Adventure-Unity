using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Running Settings")]
    [SerializeField] float runSpeed = 2f;

    [Header("Jump Settings")]
    [SerializeField] float jumpForce = 10f;

    Rigidbody2D _rigidbody = null;
    BoxCollider2D _myCollision = null;

    float horizontalAxis = 0f;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _myCollision = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        MovePlayer();
        Jump();

        if (GeneralFunctions.IsObjectMovingHorizontaly(gameObject))
        {
            GeneralFunctions.FlipSprite(gameObject, horizontalAxis);
        }
    }

    private void MovePlayer()
    {
        horizontalAxis = Input.GetAxis("Horizontal");

        transform.position += new Vector3(horizontalAxis, 0f, 0f) * Time.deltaTime * runSpeed;
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && _myCollision.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            _rigidbody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
    }
}
