using System;
using GeneralScripts;
using UnityEngine;

namespace EnemyScripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyMovement : MonoBehaviour
    {
        private bool facingRight = false;
        private const float KGroundedRadius = .3f;

        /// <summary>
        /// Use the provided rigidbody AddRelativeForce function to move the AI towards a transform
        /// </summary>
        /// <param name="target"></param>
        /// <param name="enemyRigidbody2D"></param>
        /// <param name="speed"></param>
        public void MoveAITowards(Transform target, Rigidbody2D enemyRigidbody2D, float speed)
        {
            var direction = target.position - transform.position;

            enemyRigidbody2D.AddRelativeForce(direction.normalized * speed, ForceMode2D.Force);
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

            if (!(target.position.x > transform.position.x) || !facingRight) return;
            facingRight = !facingRight;

            GeneralFunctions.FlipObject(gameObject);
        }
        /// <summary>
        /// Completely stop all movement on a given rigidbody
        /// </summary>
        /// <param name="enemyRigidbody2D"></param>
        public static void StopMovement(Rigidbody2D enemyRigidbody2D)
        {
            enemyRigidbody2D.angularVelocity = 0;
            enemyRigidbody2D.velocity = Vector2.zero;

            enemyRigidbody2D.Sleep();
        }
        /// <summary>
        /// Checks to see if the enemy is touching the ground layer
        /// </summary>
        public bool TouchingGround()
        {
            var hitGround = false;

            // ReSharper disable once Unity.PreferNonAllocApi
            var colliders = Physics2D.OverlapCircleAll(transform.position, KGroundedRadius);

            foreach (var t in colliders)
            {
                if (t.gameObject.CompareTag("Ground"))
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
        /// <param name="speed"></param>
        /// <param name="enemyRigidbody2D"></param>
        public void MoveAIForward(float speed, Rigidbody2D enemyRigidbody2D)
        {
            enemyRigidbody2D.AddRelativeForce(transform.right * speed, ForceMode2D.Force);
        }

        /// <summary>
        /// Move the AI in the current direction it's facing with a locked speed
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="enemyRigidbody2D"></param>
        /// <param name="maxSpeedMagnitude"></param>
        public void MoveAIForward(float speed, Rigidbody2D enemyRigidbody2D, float maxSpeedMagnitude)
        {
            enemyRigidbody2D.AddRelativeForce(transform.right * speed, ForceMode2D.Force);

            if (enemyRigidbody2D.velocity.magnitude >= maxSpeedMagnitude || enemyRigidbody2D.velocity.magnitude <= maxSpeedMagnitude)
            {
                enemyRigidbody2D.velocity = enemyRigidbody2D.velocity.normalized * maxSpeedMagnitude;
            }
        }

        /// <summary>
        /// Move the AI in the provided direction with no rigidbody
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="moveSpeed"></param>
        /// <param name="tolerance"></param>
        public void MoveAIForward(Vector2 direction, float moveSpeed, float tolerance)
        {
            if (direction.y >= 1 || direction.y <= -1)
            {
                transform.Translate(direction * (moveSpeed * Time.deltaTime), Space.World);
            }
            else if (Math.Abs(transform.localEulerAngles.z - 180) < tolerance)
            {
                transform.Translate(-direction * (moveSpeed * Time.deltaTime));
            }
            else
            {
                transform.Translate(direction * (moveSpeed * Time.deltaTime));
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
            Transform transform1;
            (transform1 = transform).position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

            return Vector2.Distance(transform1.position, target) <= minDistance;
        }
        /// <summary>
        ///  Will move the AI towards the target location
        /// </summary>
        /// <param name="target"></param>
        /// <param name="moveSpeed"></param>
        /// <param name="minDistance"></param>
        /// <param name="isMoving"></param>
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
        public void RotateToMovement(Rigidbody2D enemyRigidbody2D)
        {
            if (enemyRigidbody2D.velocity != Vector2.zero)
            {
                var direction = enemyRigidbody2D.velocity.normalized;

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
        /// Fire a raycast from the current AI to the given transform and checks to if there is ground between the transform and and the object
        /// </summary>
        /// <returns>A bool that determines enemy sight</returns>
        public static bool IsTransformVisible(LayerMask layerMask, Transform startTransform, Transform targetTransform, string tag, float sightRange, bool drawDebug)
        {
            var position = startTransform.position;
            var direction = GeneralFunctions.GetDistanceBetweenVectors(targetTransform.position, position);

            var hit2D = Physics2D.Raycast(position, direction, sightRange, layerMask);

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