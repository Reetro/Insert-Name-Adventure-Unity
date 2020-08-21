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

        private Vector2 boxSize;
        private float boxAngle = 0;

        private void Awake()
        {
            myBoxCollider2D = GetComponent<BoxCollider2D>();

            myRigidbody2D = GetComponent<Rigidbody2D>();

            boxSize = myBoxCollider2D.size;

            boxAngle = transform.localEulerAngles.z;

            if (boxAngle.Equals(270))
            {
                boxAngle = -90;
            }
        }
        /// <summary>
        /// Check to see if segment is in the ground if not enable collision
        /// </summary>
        public void CheckCollision()
        {
            Collider2D collider2D = Physics2D.OverlapBox(transform.position, boxSize, boxAngle, WhatIsGround);

            if (collider2D)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);

                myBoxCollider2D.enabled = true;
                myRigidbody2D.isKinematic = false;
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