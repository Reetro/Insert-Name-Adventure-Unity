using System.Collections;
using UnityEngine;

namespace EnemyCharacter.AI
{
    public class SlugMovement : EnemyBase
    {
        [Header("Movement Settings")]
        [Range(0, 4)]
        [Tooltip("How fast the slug can move")]
        [SerializeField] private float moveSpeed = 2f;
        [Tooltip("How far the slug can see")]
        [SerializeField] private float traceDistance = 0.6f;
        [Tooltip("What layers the slug can rotate on")]
        [SerializeField] private LayerMask whatCanSlugSee = new LayerMask();

        [Space]

        [Header("Debug Settings")]
        [Tooltip("Whether or not to draw debug lines")]
        [SerializeField] private bool drawDebug = false;

        [Space]

        [Header("Damage Settings")]
        [Tooltip("How much damage to apply to the player on hit")]
        [SerializeField] private float damageToPlayer = 1f;
        [Tooltip("Amount of knock back force to apply to the player on hit")]
        [SerializeField] private float knockBackForce = 2f;

        #region Local Vars
        private bool canMove = true;
        private float currentRotation = 0;
        private bool isGrounded = false;
        private bool ignoreIsGrounded = false;
        private GameObject traceStartObject = null;
        #endregion

        #region Movement Code
        private void Start()
        {
            traceStartObject = transform.GetChild(0).gameObject;

            currentRotation = transform.localEulerAngles.z;
            
            if (currentRotation.Equals(270))
            {
                currentRotation = -90;
            }

            if (drawDebug)
            {
                print("Starting Rotation: " + currentRotation.ToString());
            }

            canMove = true;
        }

        private void Update()
        {
            if (!MyHealthComponent.IsCurrentlyDead)
            {
                var hit = LookForCollision();
                isGrounded = IsGrounded();

                if (!ignoreIsGrounded)
                {
                    canMove = isGrounded;
                }
                else
                {
                    canMove = true;
                }

                if (hit)
                {
                    if (GeneralFunctions.IsObjectOnLayer(whatCanSlugSee, hit.transform.gameObject))
                    {
                        canMove = false;

                        SnapRotation();
                    }
                }

                if (canMove)
                {
                    MyMovementComp.MoveAIForward(GetFacingDirection(), moveSpeed);
                }
                else if (!isGrounded && !ignoreIsGrounded)
                {
                    SnapRotation();
                }
            }
        }

        protected override void OnDeath()
        {
            base.OnDeath();

            Destroy(gameObject, 0.1f);
        }
        #endregion

        #region Rotation Code
        /// <summary>
        /// Determines what the current rotation of the slug should be
        /// </summary>
        private void SnapRotation()
        {
            switch (currentRotation)
            {
                case 0:
                    ignoreIsGrounded = true;

                    if (isGrounded)
                    {
                        UpdateRotation(90);
                    }
                    else
                    {
                        UpdateRotation(-90);

                        NudgeSlugDown(0, 0.3f);
                    }

                    StartCoroutine(RestartGroundCheck());
                    break;

                case 90:
                    ignoreIsGrounded = true;

                    if (isGrounded)
                    {
                        UpdateRotation(180);
                    }
                    else
                    {
                        UpdateRotation(0);

                        NudgeSlugUp(0.2f, 0);
                    }

                    StartCoroutine(RestartGroundCheck());
                    break;

                case 180:
                    ignoreIsGrounded = true;

                    if (isGrounded)
                    {
                        UpdateRotation(-90);
                    }
                    else
                    {
                        UpdateRotation(90);

                        NudgeSlugUp(0, 0.2f);
                    }

                    StartCoroutine(RestartGroundCheck());
                    break;

                case -90:
                    ignoreIsGrounded = true;

                    if (isGrounded)
                    {
                        UpdateRotation(0);
                    }
                    else
                    {
                        UpdateRotation(180);

                        NudgeSlugDown(0.2f, 0);
                    }

                    StartCoroutine(RestartGroundCheck());
                    break;
            }

            if (drawDebug)
            {
                print("Current Rotation: " + currentRotation);
            }
        }
        /// <summary>
        /// Push the slug backward / downward
        /// </summary>
        /// <param name="xOffset"></param>
        /// <param name="yOffset"></param>
        private void NudgeSlugDown(float xOffset, float yOffset)
        {
            transform.position = new Vector3(transform.position.x - xOffset, transform.position.y - yOffset, 0);
        }
        /// <summary>
        /// Push the slug froward / upward
        /// </summary>
        /// <param name="xOffset"></param>
        /// <param name="yOffset"></param>
        private void NudgeSlugUp(float xOffset, float yOffset)
        {
            transform.position = new Vector3(transform.position.x + xOffset, transform.position.y + yOffset, 0);
        }
        /// <summary>
        /// Gets the current direction the slug is facing
        /// </summary>
        private Vector2 GetFacingDirection()
        {
            return transform.rotation * Vector2.right;
        }
        /// <summary>
        /// Gets the current upward direction the slug is facing
        /// </summary>
        private Vector2 GetFacingUpward()
        {
            return transform.rotation * Vector2.up;
        }
        /// <summary>
        /// Wait 0.2 seconds before starting ground checks 
        /// </summary>
        private IEnumerator RestartGroundCheck()
        {
            yield return new WaitForSeconds(0.2f);

            ignoreIsGrounded = false;
        }
        /// <summary>
        /// Update the current rotation of the Z axis
        /// </summary>
        /// <param name="newRotation"></param>
        private void UpdateRotation(float newRotation)
        {
            currentRotation = newRotation;

            transform.eulerAngles = new Vector3(0, 0, newRotation);

            canMove = true;
        }
        #endregion

        #region Collision Code
        /// <summary>
        /// Check to see if slug is touching the ground
        /// </summary>
        private bool IsGrounded()
        {
            Vector2 traceStart = traceStartObject.transform.position;
            Vector2 traceEnd = -GetFacingUpward();

            RaycastHit2D raycastHits2D = Physics2D.Raycast(traceStart, traceEnd, traceDistance, whatCanSlugSee);

            if (raycastHits2D)
            {
                if (drawDebug)
                {
                    Debug.DrawRay(traceStart, traceEnd, Color.green);
                }

                return true;
            }
            else
            {
                if (drawDebug)
                {
                    Debug.DrawRay(traceStart, traceEnd, Color.red);
                }

                return false;
            }
        }
        /// <summary>
        /// Damage and knock back player on hit
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                GeneralFunctions.DamageTarget(collision.gameObject, damageToPlayer, true, gameObject);

                Vector2 direction = GeneralFunctions.GetDirectionVectorFrom2Vectors(collision.gameObject.transform, transform);

                if (Mathf.Abs(direction.y) >= Mathf.Abs(direction.x))
                {
                    direction.x = 0;
                }
                else
                {
                    direction.y = 0;
                }

                GeneralFunctions.ApplyKnockback(collision.gameObject, direction * knockBackForce, ForceMode2D.Impulse);

                GeneralFunctions.StunPlayer(collision.gameObject, 0.1f);
            }
        }
        /// <summary>
        /// Traces in the direction the slug is facing and check to see if there is a surface to rotate on
        /// </summary>
        private RaycastHit2D LookForCollision()
        {
            Vector2 traceStart = transform.position;
            Vector2 traceEnd = GetFacingDirection();

            RaycastHit2D raycastHit2D = Physics2D.Raycast(traceStart, traceEnd, traceDistance, whatCanSlugSee);

            if (raycastHit2D)
            {
                if (drawDebug)
                {
                    Debug.DrawRay(traceStart, traceEnd, Color.green);
                }
            }
            else
            {
                if (drawDebug)
                {
                    Debug.DrawRay(traceStart, traceEnd, Color.red);
                }
            }

            return raycastHit2D;
        }
        #endregion
    }
}