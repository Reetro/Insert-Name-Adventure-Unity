using UnityEngine;

namespace EnemyCharacter.AI
{
    public class WormSegment : MonoBehaviour
    {
        /// <summary>
        /// Amount of damage to apply to the player
        /// </summary>
        public float DamageToApply { get; set; }
        /// <summary>
        /// What layers are ground
        /// </summary>
        public LayerMask WhatIsGround { get; set; }
        /// <summary>
        /// Whether or not to draw debug lines
        /// </summary>
        public bool DrawDebug { get; set; }
        /// <summary>
        /// BoxCollider2D attached to segment
        /// </summary>
        public BoxCollider2D myBoxCollider2D { get; private set; } = null;
        /// <summary>
        /// Rigidbody2D attached to segment
        /// </summary>
        public Rigidbody2D myRigidbody2D { get; private set; } = null;
        /// <summary>
        /// JointToggler attached to segment
        /// </summary>
        public JointToggler jointToggler { get; private set; } = null;

        private Vector2 boxSize;
        private float boxAngle = 0;
        private float myWidth = 0;

        /// <summary>
        /// Set all needed references and disable physics
        /// </summary>
        private void Awake()
        {
            myBoxCollider2D = GetComponent<BoxCollider2D>();

            myRigidbody2D = GetComponent<Rigidbody2D>();

            boxSize = myBoxCollider2D.size;

            boxAngle = GeneralFunctions.GetObjectAngle(gameObject);

            jointToggler = GetComponent<JointToggler>();

            myWidth = GeneralFunctions.GetSpriteWidth(GetComponent<SpriteRenderer>());

            //jointToggler.enabled = false;
            //myBoxCollider2D.enabled = false;
            //myRigidbody2D.isKinematic = true;
        }
        /// <summary>
        /// Check to see if segment is in the ground if not enable collision and physics
        /// </summary>
        public void CheckCollision()
        {
            var traceDown = (Vector2)transform.position - Vector2.down * myWidth;

            RaycastHit2D raycastHit2DDown = Physics2D.Raycast(transform.position, traceDown, 1f);

            if (raycastHit2DDown)
            {
                if (!raycastHit2DDown.transform.gameObject.CompareTag("Worm Segment"))
                {
                    if (DrawDebug)
                    {
                        Debug.DrawLine(transform.position, traceDown * 1f, Color.green);
                    }

                    jointToggler.enabled = true;
                    myBoxCollider2D.enabled = true;
                    myRigidbody2D.isKinematic = false;
                }
            }
            else
            {
                if (DrawDebug)
                {
                    Debug.DrawLine(transform.position, traceDown * 1f, Color.red);
                }

                Collider2D collider2D = Physics2D.OverlapBox(transform.position, boxSize, boxAngle, WhatIsGround);

                if (collider2D)
                {
                    jointToggler.enabled = false;
                    myBoxCollider2D.enabled = false;
                    myRigidbody2D.isKinematic = true;
                }
                else
                {
                    var traceUp = (Vector2)transform.position + Vector2.up / myWidth;

                    RaycastHit2D raycastHit2DUp = Physics2D.Raycast(transform.position, traceUp, 1f, WhatIsGround);

                    if (raycastHit2DUp)
                    {
                        if (DrawDebug)
                        {
                            Debug.DrawLine(transform.position, traceUp * 1f, Color.green);
                        }

                        jointToggler.enabled = false;
                        myBoxCollider2D.enabled = false;
                        myRigidbody2D.isKinematic = true;
                    }
                    else
                    {
                        if (DrawDebug)
                        {
                            Debug.DrawLine(transform.position, traceUp * 1f, Color.red);
                        }

                        jointToggler.enabled = true;
                        myBoxCollider2D.enabled = true;
                        myRigidbody2D.isKinematic = false;
                    }
                }
            }
        }
        /// <summary>
        /// If debug is active draw collision cube
        /// </summary>
        private void OnDrawGizmos()
        {
            if (DrawDebug)
            {
                Gizmos.DrawCube(transform.position, boxSize);
            }
        }
        /// <summary>
        /// When hit by player deal damage
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                //GeneralFunctions.DamageTarget(collision.gameObject, DamageToApply, true, gameObject);
            }
        }
    }
}