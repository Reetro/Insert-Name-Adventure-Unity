using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    bool facingRight = false;

    public void MoveAITowards(Transform target, Rigidbody2D rigidbody, float speed)
    {
        var direction = target.position - transform.position;

        rigidbody.AddRelativeForce(direction.normalized * speed, ForceMode2D.Force);
    }

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

    public void AddToLeechY(Transform leechTransfrom, float amountToAdd)
    {
        var temp = leechTransfrom.position;

        temp.y += amountToAdd;

        leechTransfrom.position = temp;
    }

    public void StopMovement(Rigidbody2D rigidbody)
    {
        rigidbody.angularVelocity = 0;
        rigidbody.velocity = Vector2.zero;

        rigidbody.Sleep();
    }
}
