using UnityEngine;

public class PingPongMovement : PlatformMovement
{
    public Transform pos1, pos2;

    private Vector3 nextPos;
    private float oldPosition = 0.0f;
    private Transform lastPos;

    void Start()
    {
        nextPos = startPos.position;

        oldPosition = transform.position.x;
    }

    void Update()
    {
        InvertDirection();

        transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (transform.position.x > oldPosition || transform.position.x < oldPosition)
            {
                nextPos = lastPos.position;
            }
        }
    }

    private void InvertDirection()
    {
        if (Vector3.Distance(transform.position, pos1.position) <= 1)
        {
            nextPos = pos2.position;
            lastPos = pos1;
        }
        if (Vector3.Distance(transform.position, pos2.position) <= 1)
        {
            nextPos = pos1.position;
            lastPos = pos2;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(pos1.position, pos2.position);
    }
}
