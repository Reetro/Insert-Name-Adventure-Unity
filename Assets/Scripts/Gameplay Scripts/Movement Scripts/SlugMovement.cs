using UnityEngine;

namespace EnemyCharacter.SceneObject
{
    public class SlugMovement : EnemyBase
    {
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private float traceDistance = 0.6f;
        [SerializeField] private Vector2 rotateGroundOffset = new Vector2(0.2f, 0.2f);
        [SerializeField] private LayerMask whatCanSlugSee = new LayerMask();

        private bool canMove = true;

        private void Start()
        {
            canMove = true;
        }

        private void Update()
        {
            var hit = LookForCollision();

            if (hit)
            {
                if (GeneralFunctions.IsObjectOnLayer(whatCanSlugSee, hit.transform.gameObject))
                {
                    canMove = false;

                    SnapRotation();
                }
            }

            if (!IsGrounded())
            {
                canMove = false;

                SnapRotation();
            }

            if (canMove)
            {
                MovementComp.MoveAIForward(GetFacingDirection(), moveSpeed);
            }
        }
        /// <summary>
        /// Traces in the direction the slug is facing
        /// </summary>
        /// <returns></returns>
        private RaycastHit2D LookForCollision()
        {
            Vector2 traceStart = transform.position;
            Vector2 traceEnd = GetFacingDirection();

            RaycastHit2D raycastHit2D = Physics2D.Raycast(traceStart, traceEnd, traceDistance, whatCanSlugSee);
            Debug.DrawRay(traceStart, traceEnd, Color.green);

            return raycastHit2D;
        }
        /// <summary>
        /// Check to see if the slug is grounded
        /// </summary>
        private bool IsGrounded()
        {
            Vector2 traceStart = transform.position;
            Vector2 traceEnd = -GetFacingUpVector();

            RaycastHit2D raycastHit2D = Physics2D.Raycast(traceStart, traceEnd, traceDistance, whatCanSlugSee);
            Debug.DrawRay(traceStart, traceEnd, Color.green);

            if (raycastHit2D)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Determines the what the current rotation of the slug should be
        /// </summary>
        private void SnapRotation()
        {
            if (!GeneralFunctions.IsNumberNegative(transform.position.x))
            {
                transform.Rotate(0, 0, 90);

                transform.position = (Vector2)transform.position + rotateGroundOffset;

                canMove = true;
            }
            else
            {
                transform.Rotate(0, 0, 90);

                transform.position = (Vector2)transform.position - rotateGroundOffset;

                canMove = true;
            }
        }
        /// <summary>
        /// Gets the current direction the slug is facing
        /// </summary>
        private Vector2 GetFacingDirection()
        {
            return transform.rotation * Vector2.right;
        }
        /// <summary>
        /// Gets the current facing up vector based the direction the slug is facing
        /// </summary>
        private Vector2 GetFacingUpVector()
        {
            return transform.rotation * Vector2.up;
        }
    }
}