using UnityEngine;

public class PingPongMovement : MonoBehaviour
{
    public Transform pos1, pos2;
    public float speed;
    public Transform startPos;

    Vector3 nextPos;

    void Start()
    {
        nextPos = startPos.position;
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, pos1.position) <= 1)
        {
            nextPos = pos2.position;
        }
        if (Vector3.Distance(transform.position, pos2.position) <= 1)
        {
            nextPos = pos1.position;
        }

        transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
    }

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

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(pos1.position, pos2.position);
    }
}
