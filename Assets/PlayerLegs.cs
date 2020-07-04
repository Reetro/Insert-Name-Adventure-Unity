using UnityEngine;
using UnityEngine.Events;

public class PlayerLegs : MonoBehaviour
{
    private bool isGrounded = true;
    private Vector3 defaultScale;

    [Header("Layer Settings")]
    public LayerMask whatIsGround;  // A mask determining what is ground to the character

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    private void Start()
    {
        defaultScale = transform.localScale;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        bool wasGrounded = isGrounded;
        isGrounded = false;

        if (collision.gameObject.CompareTag("Platform"))
        {
            GeneralFunctions.AttachObjectToTransfrom(collision.transform, gameObject);
        }
        // Check to see if player is on the ground
        else if (GeneralFunctions.IsObjectOnLayer(whatIsGround, collision.gameObject))
        {
            if (collision.gameObject != gameObject)
            {
                isGrounded = true;
                if (!wasGrounded)
                {
                    OnLandEvent.Invoke();
                }
            }
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

    public bool GetIsGrounded()
    {
        return isGrounded;
    }

    public void UpdateGrounded(bool newValue)
    {
        isGrounded = newValue;
    }
}
