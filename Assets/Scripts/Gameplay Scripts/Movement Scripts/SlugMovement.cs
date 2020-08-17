using System.Collections;
using UnityEngine;

namespace EnemyCharacter.AI
{
    [RequireComponent(typeof(SlugInstanceSettings))]
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

        [Header("Damage Settings")]
        [Tooltip("How much damage to apply to the player on hit")]
        [SerializeField] private float damageToPlayer = 1f;
        [Tooltip("Amount of knock back force to apply to the player on hit")]
        [SerializeField] private float knockBackForce = 2f;

        #region Local Vars
        private bool canMove = true;
        private float lastRotation = 0;
        private float currentRotation = 0;
        private bool isGrounded = false;
        private bool ignoreIsGrounded = false;
        private GameObject traceStartObject = null;

        [HideInInspector]
        public bool onFloatingPlatform = false;
        [HideInInspector]
        public bool drawDebug = false;
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
                MovementComp.MoveAIForward(GetFacingDirection(), moveSpeed);
            }
            else if (!isGrounded && !ignoreIsGrounded)
            {
                SnapRotation();
            }
        }
        #endregion

        #region Rotation Code
        /// <summary>
        /// Determines what the current rotation of the slug should be
        /// </summary>
        private void SnapRotation()
        {
            if (lastRotation.Equals(0) && currentRotation.Equals(0))
            {
                ignoreIsGrounded = true;
                
                if (isGrounded)
                {
                    UpdateRotation(90);
                }
                else
                {
                    UpdateRotation(-90);

                    transform.position = new Vector3(transform.position.x, transform.position.y - 0.3f, 0);
                }

                StartCoroutine(RestartGroundCheck());
            }
            else if (lastRotation.Equals(0) && currentRotation.Equals(180))
            {
                ignoreIsGrounded = true;
                
                UpdateRotation(90);

                transform.position = new Vector3(transform.position.x, transform.position.y + 0.3f, 0);

                StartCoroutine(RestartGroundCheck());
            }
            else if (lastRotation.Equals(0) && currentRotation.Equals(90))
            {
                ignoreIsGrounded = true;
                
                if (isGrounded)
                {
                    UpdateRotation(180);
                }
                else
                {
                    UpdateRotation(0);

                    transform.position = new Vector3(transform.position.x + 0.38f, transform.position.y, 0);
                }

                StartCoroutine(RestartGroundCheck());
            }
            else if (lastRotation.Equals(90) && currentRotation.Equals(180))
            {
                ignoreIsGrounded = true;
                
                if (isGrounded)
                {
                    UpdateRotation(-90);
                }
                else
                {
                    UpdateRotation(90);

                    transform.position = new Vector3(transform.position.x, transform.position.y + 0.3f, 0);
                }

                StartCoroutine(RestartGroundCheck());
            }
            else if (lastRotation.Equals(180) && currentRotation.Equals(-90))
            {
                ignoreIsGrounded = true;
                
                if (isGrounded)
                {
                    UpdateRotation(0);
                }
                else
                {
                    UpdateRotation(180);
                    transform.position = new Vector3(transform.position.x - 0.3f, transform.position.y + 0.05f, 0);
                }

                StartCoroutine(RestartGroundCheck());
            }
            else if (lastRotation.Equals(-90) && currentRotation.Equals(180))
            {
                ignoreIsGrounded = true;
                
                if (isGrounded)
                {
                    UpdateRotation(-90);
                }
                else
                {
                    UpdateRotation(90);
                }

                transform.position = new Vector3(transform.position.x, transform.position.y + 0.3f, 0);

                StartCoroutine(RestartGroundCheck());
            }
            else if (lastRotation.Equals(180) && currentRotation.Equals(90))
            {
                ignoreIsGrounded = true;
                
                if (isGrounded)
                {
                    UpdateRotation(180);
                }
                else
                {
                    UpdateRotation(0);

                    transform.position = new Vector3(transform.position.x + 0.38f, transform.position.y, 0);
                }

                StartCoroutine(RestartGroundCheck());
            }
            else if (lastRotation.Equals(-90) && currentRotation.Equals(0))
            {
                ignoreIsGrounded = true;

                if (isGrounded)
                {
                    UpdateRotation(90);
                }
                else
                {
                    UpdateRotation(-90);
                }

                StartCoroutine(RestartGroundCheck());
            }
            else if (lastRotation.Equals(90) && currentRotation.Equals(180))
            {
                ignoreIsGrounded = true;
                UpdateRotation(0);
                StartCoroutine(RestartGroundCheck());
            }
            else if (lastRotation.Equals(90) && currentRotation.Equals(0))
            {
                ignoreIsGrounded = true;
                
                if (isGrounded)
                {
                    UpdateRotation(0);
                }
                else
                {
                    UpdateRotation(-90);

                    transform.position = new Vector3(transform.position.x - 0.05f, transform.position.y - 0.2f, 0);
                }

                StartCoroutine(RestartGroundCheck());
            }
            else if (lastRotation.Equals(0) && currentRotation.Equals(-90))
            {
                ignoreIsGrounded = true;

                if (isGrounded)
                {
                    UpdateRotation(0);
                }
                else
                {
                    if (!onFloatingPlatform)
                    {
                        UpdateRotation(-90);

                        transform.position = new Vector3(transform.position.x - 0.05f, transform.position.y - 0.2f, 0);
                    }
                    else
                    {
                        UpdateRotation(180);

                        transform.position = new Vector3(transform.position.x - 0.38f, transform.position.y, 0);
                    }
                }

                StartCoroutine(RestartGroundCheck());
            }
            else if (lastRotation.Equals(-90) && currentRotation.Equals(-90))
            {
                ignoreIsGrounded = true;
                
                if (isGrounded)
                {
                    UpdateRotation(0);
                }
                else
                {
                    UpdateRotation(180);

                    transform.position = new Vector3(transform.position.x - 0.38f, transform.position.y, 0);
                }

                StartCoroutine(RestartGroundCheck());
            }

            if (drawDebug)
            {
                print("Current Rotation: " + currentRotation + " last Rotation: " + lastRotation);
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
            lastRotation = currentRotation;
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