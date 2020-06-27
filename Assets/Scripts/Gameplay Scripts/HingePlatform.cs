using UnityEngine;

public class HingePlatform : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!GeneralFunctions.IsObjectPlayer(collision.gameObject) && !GeneralFunctions.IsObjectOnLayer("Attached Leech", collision.gameObject))
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
        if (!GeneralFunctions.IsObjectPlayer(collision.gameObject) && !GeneralFunctions.IsObjectOnLayer("Attached Leech", collision.gameObject))
        {
            RigidbodyManager rigidbodyManager = collision.gameObject.GetComponent<RigidbodyManager>();

            if (rigidbodyManager)
            {
                rigidbodyManager.OnPlatfromExit();
            }
        }
    }
}
