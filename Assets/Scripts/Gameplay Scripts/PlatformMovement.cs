using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    public float speed;
    public Transform startPos;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GeneralFunctions.AttachObjectToPlatform(gameObject, collision.gameObject);

        if (!GeneralFunctions.IsObjectPlayer(collision.gameObject))
        {
            RigidbodyManager rigidbodyManager = collision.gameObject.GetComponent<RigidbodyManager>();

            if (rigidbodyManager)
            {
                rigidbodyManager.OnPlatfromEnter();
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        GeneralFunctions.DetachFromParent(collision.gameObject);

        if (!GeneralFunctions.IsObjectPlayer(collision.gameObject))
        {
            RigidbodyManager rigidbodyManager = collision.gameObject.GetComponent<RigidbodyManager>();

            if (rigidbodyManager)
            {
                rigidbodyManager.OnPlatfromExit();
            }
        }
    }
}
