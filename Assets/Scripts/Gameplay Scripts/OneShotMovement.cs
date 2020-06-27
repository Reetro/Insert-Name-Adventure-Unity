using UnityEngine;

public class OneShotMovement : PlatformMovement
{
    public Transform targetDirection;

    private Vector3 normalizeDirection;
    private bool canMove = true;

    private void Start()
    {
        normalizeDirection = (targetDirection.position - transform.position).normalized;
    }

    private void Update()
    {
        if (canMove)
        {
            transform.position += normalizeDirection * speed * Time.deltaTime;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canMove = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canMove = true;
        }
    }
}
