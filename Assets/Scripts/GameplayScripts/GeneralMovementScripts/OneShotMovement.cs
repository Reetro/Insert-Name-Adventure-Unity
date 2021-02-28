using UnityEngine;

namespace GameplayScripts.GeneralMovementScripts
{
    public class OneShotMovement : PlatformMovement
    {
        public Transform targetDirection;
        private Vector3 normalizeDirection;

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
            if (IsPlatformActive)
            {
                transform.position += normalizeDirection * (speed * Time.deltaTime);
            }
        }
        /// <summary>
        /// Check to see if the ground or player is blocking the platforms path if so stop moving
        /// </summary>
        /// <param name="collision"></param>
        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);

            if (TouchingGround || IsPathBlocked)
            {
                IsPlatformActive = false;
            }
        }
        /// <summary>
        /// Check to see if the ground or player is blocking the platforms path if not start moving
        /// </summary>
        /// <param name="collision"></param>
        protected override void OnTriggerExit2D(Collider2D collision)
        {
            base.OnTriggerExit2D(collision);

            if (!usePressurePlate)
            {
                RestartMovement(collision);
            }
            else if (HasPlateBeenPressed)
            {
                RestartMovement(collision);
            }
        }
        /// <summary>
        /// Restarts the platforms movement when platform stops overlapping touching an object
        /// </summary>
        /// <param name="collision"></param>
        private void RestartMovement(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                IsPlatformActive = true;
            }
            else if (!IsPathBlocked && !TouchingGround)
            {
                IsPlatformActive = true;
            }
        }
    }
}