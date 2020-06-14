using UnityEngine;

public class Leech : MonoBehaviour
{
    [SerializeField] EnemyMovement controller = null;

    [Header("Leech Movement Settings")]
    [SerializeField] float leechFlySpeed = 20f;

    private float moveDirection = 0f;
    private void Start()
    {
        
    }

    private void Update()
    {
        moveDirection = GetMoveDirection() * leechFlySpeed;
    }

    private void FixedUpdate()
    {
        
    }

    private float GetMoveDirection()
    {
        return 0;
    }
}
