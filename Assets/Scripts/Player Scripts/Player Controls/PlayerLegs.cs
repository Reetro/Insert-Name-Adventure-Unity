using UnityEngine;
using UnityEngine.Events;

public class PlayerLegs : MonoBehaviour
{
    [HideInInspector]
    public bool isGrounded = true;

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
}