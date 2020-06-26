using UnityEngine;

public class OneShotMovement : PlatformMovement
{
    public Transform targetDirection;

    private Vector3 normalizeDirection;

    private void Start()
    {
        normalizeDirection = (targetDirection.position - transform.position).normalized;
    }

    private void Update()
    {
        transform.position += normalizeDirection * speed * Time.deltaTime;
    }
}
