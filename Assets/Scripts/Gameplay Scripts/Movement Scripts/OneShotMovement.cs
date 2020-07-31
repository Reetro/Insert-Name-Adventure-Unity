using UnityEngine;

namespace LevelObjects.MovingObjects
{
    public class OneShotMovement : PlatformMovement
    {
        public Transform targetDirection;

        private Vector3 normalizeDirection;
        private bool canMove = true;

        private void Awake()
        {
            // Get direction to move in
            normalizeDirection = (targetDirection.position - transform.position).normalized;
        }
        /// <summary>
        /// If the platform is able to move then move platform towards normalizeDirection 
        /// </summary>
        private void Update()
        {
            if (canMove)
            {
                transform.position += normalizeDirection * speed * Time.deltaTime;
            }
        }
        /// <summary>
        /// Check to see if the ground or player is blocking the platforms path if so stop moving
        /// </summary>
        /// <param name="collision"></param>
        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);

            if (TouchingGround || IsPlayerBlockingPath)
            {
                canMove = false;
            }
        }
        /// <summary>
        /// Check to see if the ground or player is blocking the platforms path if not start moving
        /// </summary>
        /// <param name="collision"></param>
        protected override void OnTriggerExit2D(Collider2D collision)
        {
            base.OnTriggerExit2D(collision);

            if (collision.gameObject.CompareTag("Ground"))
            {
                canMove = true;
            }
            else if (!IsPlayerBlockingPath && !TouchingGround)
            {
                canMove = true;
            }
        }
    }
}