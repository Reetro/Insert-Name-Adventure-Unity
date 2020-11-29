using UnityEngine;

namespace EnemyCharacter.AI
{
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
        /// Move AI towards a fixed target
        /// </summary>
        /// <param name="target"></param>
        /// <param name="speed"></param>
        public void MoveAITowards(Vector2 target, float speed)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
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
        /// <summary>
        /// Move the AI in the current direction it's facing
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="speed"></param>
        public void MoveAIForward(float speed, Rigidbody2D rigidbody)
        {
            rigidbody.AddRelativeForce(transform.right * speed, ForceMode2D.Force);
        }
        /// <summary>
        /// Move the AI in the current direction it's facing with a locked speed
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="rigidbody"></param>
        /// <param name="maxSpeedMagnatuide"></param>
        public void MoveAIForward(float speed, Rigidbody2D rigidbody, float maxSpeedMagnitude)
        {
            rigidbody.AddRelativeForce(transform.right * speed, ForceMode2D.Force);

            if (rigidbody.velocity.magnitude >= maxSpeedMagnitude || rigidbody.velocity.magnitude <= maxSpeedMagnitude)
            {
                rigidbody.velocity = rigidbody.velocity.normalized * maxSpeedMagnitude;
            }
        }
        /// <summary>
        /// Move the AI in the provided direction with no rigidbody
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="moveSpeed"></param>
        public void MoveAIForward(Vector2 direction, float moveSpeed)
        {
            if (direction.y >= 1 || direction.y <= -1)
            {
                transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
            }
            else if (transform.localEulerAngles.z == 180)
            {
                transform.Translate(-direction * moveSpeed * Time.deltaTime);
            }
            else
            {
                transform.Translate(direction * moveSpeed * Time.deltaTime);
            }
        }
        /// <summary>
        /// Will move the AI towards the target location
        /// </summary>
        /// <param name="target"></param>
        /// <param name="moveSpeed"></param>
        /// <param name="minDistance"></param>
        /// <returns>Returns true if current position is less then or equal to minDistance</returns>
        public bool MoveAIToPoint(Vector2 target, float moveSpeed, float minDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, target) <= minDistance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        ///  Will move the AI towards the target location
        /// </summary>
        /// <param name="target"></param>
        /// <param name="moveSpeed"></param>
        /// <param name="minDistance"></param>
        /// <param name="isMoving"></param>
        /// <returns></returns>
        public bool MoveAIToPoint(Vector2 target, float moveSpeed, float minDistance, out bool isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, target) <= minDistance)
            {
                isMoving = false;

                return true;
            }
            else
            {
                isMoving = true;

                return false;
            }
        }
        /// <summary>
        /// Make the AI face the direction it's moving in
        /// </summary>
        public void RotateToMovement(Rigidbody2D rigidbody2D)
        {
            if (rigidbody2D.velocity != Vector2.zero)
            {
                var direction = rigidbody2D.velocity.normalized;

                if (GeneralFunctions.IsNumberNegative(direction.x))
                {
                    transform.rotation = new Quaternion(0, 180, 0, 0);
                }
                else
                {
                    transform.rotation = new Quaternion(0, 0, 0, 0);
                }
            }
        }
        /// <summary>
        /// Fire a raycast from the current AI to the given transform and checks to if there is ground between the transform and and the enemy
        /// </summary>
        /// <returns>A bool that determines enemy sight</returns>
        public bool IsTransformVisiable(LayerMask layerMask, Transform startTransform, Transform targetTransform, string tag, float sightRange, bool drawDebug)
        {
            var direction = GeneralFunctions.GetDistanceBetweenVectors(targetTransform.position, startTransform.position);

            RaycastHit2D hit2D = Physics2D.Raycast(startTransform.position, direction, sightRange, layerMask);

            if (hit2D)
            {
                if (hit2D.transform.gameObject.CompareTag(tag))
                {
                    if (drawDebug)
                    {
                        Debug.DrawLine(startTransform.position, direction * sightRange, Color.green);
                    }

                    return true;
                }
                else
                {
                    if (drawDebug)
                    {
                        Debug.DrawLine(startTransform.position, direction * sightRange, Color.red);
                    }


                    return false;
                }
            }
            else
            {
                if (drawDebug)
                {
                    Debug.DrawLine(startTransform.position, direction * sightRange, Color.red);
                }

                return false;
            }
        }
    }
}