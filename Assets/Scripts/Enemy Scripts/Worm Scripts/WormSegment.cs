using UnityEngine;
using UnityEngine.Events;

namespace EnemyCharacter.AI
{
    public class WormSegment : MonoBehaviour
    {
        private GameplayObjectID idObject = null;
        private SpriteRenderer spriteRenderer = null;
        private BoxCollider2D boxCollider2D = null;
        private Vector2 boxSize;
        private float boxAngle = 0;
        private float myWidth = 0;

        [System.Serializable]
        public class OnSegmentDeath : UnityEvent<WormSegment> { }

        [HideInInspector]
        public OnSegmentDeath SegmentDeath;

        /// <summary>
        /// Set all needed references
        /// </summary>
        private void Awake()
        {
            idObject = GetComponent<GameplayObjectID>();
            MyHealthComponent = GetComponent<HealthComponent>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            boxCollider2D = GetComponent<BoxCollider2D>();
            myBoxCollider2D = GetComponent<BoxCollider2D>();
            myRigidbody2D = GetComponent<Rigidbody2D>();
            jointToggler = GetComponent<JointToggler>();

            boxSize = myBoxCollider2D.size;

            boxAngle = GeneralFunctions.GetObjectAngle(gameObject);

            idObject.ConstructID();
            MyHealthComponent.ConstructHealthComponent();
            MyHealthComponent.OnDeath.AddListener(OnDeath);

            jointToggler.enabled = false;
            myBoxCollider2D.enabled = false;
            myRigidbody2D.isKinematic = true;
        }
        /// <summary>
        /// Called when segment dies disables both collision and sprite renderer then invokes an OnSegmentDeath event
        /// </summary>
        private void OnDeath()
        {
            spriteRenderer.enabled = false;
            boxCollider2D.enabled = false;

            SegmentDeath.Invoke(this);
        }
        /// <summary>
        /// Check to see if segment is in the ground if not enable collision
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
                    AboveGround = true;
                }
                else
                {
                    CheckOverlapBox(traceDown);
                }
            }
            else
            {
                CheckOverlapBox(traceDown);
            }
        }
        /// <summary>
        /// Draws a box and checks to see if anything is overlapping it if not enable collision
        /// </summary>
        /// <param name="traceDown"></param>
        private void CheckOverlapBox(Vector2 traceDown)
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
                AboveGround = false;
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
                    AboveGround = false;
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
                    AboveGround = true;
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
                GeneralFunctions.DamageTarget(collision.gameObject, DamageToApply, true, gameObject);
            }
        }
        /// <summary>
        /// Amount of damage to apply to the player
        /// </summary>
        public float DamageToApply { get; set; }
        /// <summary>
        /// Gets this Gameobjects ID
        /// </summary>
        public int MyID { get { return idObject.ID; } }
        /// <summary>
        /// Get this Gameobjects health component
        /// </summary>
        public HealthComponent MyHealthComponent { get; private set; } = null;
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
        /// <summary>
        /// What layers are ground
        /// </summary>
        public LayerMask WhatIsGround { get; set; }
        /// <summary>
        /// Whether or not to draw debug lines
        /// </summary>
        public bool DrawDebug { get; set; }
        /// <summary>
        /// Checks to see if the current segment is above ground
        /// </summary>
        public bool AboveGround { get; private set; }
    }
}