using UnityEngine;

public class PingPongMovement : PlatformMovement
{
    public Transform pos1, pos2;
    private Vector3 nextPos;

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

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(pos1.position, pos2.position);
    }
}
