using UnityEngine;
using EnemyCharacter.AI;
using PlayerCharacter.Controller;
using UnityEngine.Events;

namespace LevelObjects.MovingObjects
{
    public class PlatformMovement : MonoBehaviour
    {
        [Tooltip("How fast the platform should move")]
        public float speed;
        [Tooltip("Where the platform starts")]
        public Transform startPos;
        [Tooltip("Should the pressure plate wait for pressure plate input to move")]
        public bool usePresurePlate = false;
        /// <summary>
        /// Checks to see if a object is currently blocking the platforms path
        /// </summary>
        public bool IsPathBlocked { get; protected set; } = false;
        /// <summary>
        /// Checks to see if the platform is touching the ground
        /// </summary>
        public bool TouchingGround { get; protected set; } = false;
        /// <summary>
        /// whether or not the current platform is moving
        /// </summary>
        public bool IsPlatformActive { get; protected set; } = false;
        /// <summary>
        /// Checks to see if the pressure plate has pressed
        /// </summary>
        public bool hasPlateBeenPreesed { get; protected set; } = false;

        /// <summary>
        /// Called when a connected pressure plate is pressed
        /// </summary>
        public virtual void OnPresurePressed()
        {
            if (usePresurePlate && !IsPlatformActive)
            {
                if (hasPlateBeenPreesed && TouchingGround)
                {
                    IsPlatformActive = false;
                    return;
                }

                IsPlatformActive = true;
                hasPlateBeenPreesed = true;
            }
        }
        /// <summary>
        /// Check to see if the platform needs a pressure plate to move if so disable movement
        /// </summary>
        protected virtual void Start()
        {
            if (!usePresurePlate)
            {
                IsPlatformActive = true;
            }
            else
            {
                IsPlatformActive = false;
            }
        }
        /// <summary>
        /// When an object collides with the platform check to see if can be attached then attach object to the platform
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!GeneralFunctions.IsObjectPlayer(collision.gameObject) && !GeneralFunctions.IsObjectOnLayer("Attached Leech", collision.gameObject))
            {
                GeneralFunctions.AttachObjectToTransfrom(gameObject.transform, collision.gameObject, collision.gameObject.transform.rotation);

                RigidbodyManager rigidbodyManager = collision.gameObject.GetComponent<RigidbodyManager>();

                if (rigidbodyManager)
                {
                    rigidbodyManager.OnPlatfromEnterCall();
                }
            }
        }
        /// <summary>
        /// Check to see if a given object was attached then de parent it 
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (!GeneralFunctions.IsObjectPlayer(collision.gameObject) && !GeneralFunctions.IsObjectOnLayer("Attached Leech", collision.gameObject))
            {
                GeneralFunctions.DetachFromParent(collision.gameObject);

                RigidbodyManager rigidbodyManager = collision.gameObject.GetComponent<RigidbodyManager>();

                if (rigidbodyManager)
                {
                    rigidbodyManager.OnPlatfromExitCall();
                }
            }
        }
        /// <summary>
        /// Checks to see if a enemy is blocking it's path if it hits a enemy that's close to ground enemy is killed if a player is blocking it's path it will set IsPathBlocked to true
        /// </summary>
        /// <param name="collision"></param>
        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (GeneralFunctions.IsObjectOnLayer("Enemy", collision.gameObject))
            {
                var enemy = collision.gameObject.GetComponent<EnemyMovement>();

                if (enemy)
                {
                    if (enemy.TouchingGround())
                    {
                        GeneralFunctions.KillTarget(collision.gameObject);
                    }
                }
                else
                {
                    IsPathBlocked = true;
                }
            }
            else if (GeneralFunctions.IsObjectPlayer(collision.gameObject))
            {
                GeneralFunctions.AttachObjectToTransfrom(transform, collision.gameObject);

                UpdatePlayerPathBlocking(collision);
            }
            else if (collision.gameObject.CompareTag("Ground"))
            {
                TouchingGround = true;
            }
        }
        /// <summary>
        /// If player has stopped overlapping the platform and was touching the ground IsPathBlocked is set to false
        /// </summary>
        /// <param name="collision"></param>
        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            if (GeneralFunctions.IsObjectPlayer(collision.gameObject))
            {
                GeneralFunctions.DetachFromParent(collision.gameObject);

                var playerLegs = collision.gameObject.transform.GetChild(0).GetComponent<PlayerLegs>();

                if (playerLegs.TouchingGround() && !TouchingGround)
                {
                    IsPathBlocked = false;
                }
            }
            else if (collision.gameObject.CompareTag("Ground"))
            {
                TouchingGround = false;
            }
            else if (GeneralFunctions.IsObjectOnLayer("Enemy", collision.gameObject))
            {
                IsPathBlocked = false;
            }
        }
        /// <summary>
        /// Checks to see if a enemy is blocking it's path if it hits a enemy that's close to ground enemy is killed if any other object is blocking it's path it will set IsPathBlocked to true
        /// </summary>
        /// <param name="collision"></param>
        protected virtual void OnTriggerStay2D(Collider2D collision)
        {
            if (GeneralFunctions.IsObjectOnLayer("Enemy", collision.gameObject))
            {
                var enemy = collision.gameObject.GetComponent<EnemyMovement>();

                if (enemy)
                {
                    if (enemy.TouchingGround())
                    {
                        GeneralFunctions.KillTarget(collision.gameObject);
                    }
                }
                else
                {
                    IsPathBlocked = true;
                }
            }
            else if (GeneralFunctions.IsObjectPlayer(collision.gameObject))
            {
                UpdatePlayerPathBlocking(collision);
            }
            else if (collision.gameObject.CompareTag("Ground"))
            {
                TouchingGround = true;
            }
        }
        /// <summary>
        /// Get player legs and check to see if the player grounded if so player is blocking the platform
        /// </summary>
        /// <param name="collision"></param>
        private void UpdatePlayerPathBlocking(Collider2D collision)
        {
            var playerLegs = collision.gameObject.transform.GetChild(0).GetComponent<PlayerLegs>();

            if (playerLegs)
            {
                if (playerLegs.TouchingGround())
                {
                    IsPathBlocked = true;
                }
                else
                {
                    IsPathBlocked = false;
                }
            }
        }
    }
}