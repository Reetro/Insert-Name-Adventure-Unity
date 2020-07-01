﻿using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    bool facingRight = false;
    const float k_GroundedRadius = .3f;

    /// <summary>
    /// Use the provided rigidbody AddRelativeForce function to move the AI towards a transform
    /// </summary>
    /// <param name="target"></param>
    /// <param name="rigidbody"></param>
    /// <param name="speed"></param>
    public void MoveAITowards(Transform target, Rigidbody2D rigidbody, float speed)
    {
        var direction = target.position - transform.position;

        rigidbody.AddRelativeForce(direction.normalized * speed, ForceMode2D.Force);
    }
    /// <summary>
    /// Makes the AI look at a specific transform
    /// </summary>
    /// <param name="target"></param>
    public void LookAtTarget(Transform target)
    {
        if (target.position.x < transform.position.x && !facingRight)
        {
            facingRight = !facingRight;

            GeneralFunctions.FlipObject(gameObject);
        }
        if (target.position.x > transform.position.x && facingRight)
        {
            facingRight = !facingRight;

            GeneralFunctions.FlipObject(gameObject);
        }
    }
    /// <summary>
    /// Adds a given number to the given leech Y coordinate to give a bouncing effect
    /// </summary>
    /// <param name="leechTransfrom"></param>
    /// <param name="amountToAdd"></param>
    public void AddToLeechY(Transform leechTransfrom, float amountToAdd)
    {
        var temp = leechTransfrom.position;

        temp.y += amountToAdd;

        leechTransfrom.position = temp;
    }
    /// <summary>
    /// Completely stop all movement on a given rigidbody
    /// </summary>
    /// <param name="rigidbody"></param>
    public void StopMovement(Rigidbody2D rigidbody)
    {
        rigidbody.angularVelocity = 0;
        rigidbody.velocity = Vector2.zero;

        rigidbody.Sleep();
    }
    /// <summary>
    /// Checks to see if the enemy is touching the ground layer
    /// </summary>
    public bool TouchingGround()
    {
        var hitGround = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, k_GroundedRadius);

        for (int index = 0; index < colliders.Length; index++)
        {
            if (colliders[index].gameObject.CompareTag("Ground"))
            {
                hitGround = true;
                break;
            }
            else
            {
                hitGround = false;
                continue;
            }
        }
        return hitGround;
    }
}