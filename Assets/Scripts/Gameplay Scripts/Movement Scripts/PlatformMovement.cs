using UnityEngine;
using EnemyCharacter.AI;
using PlayerCharacter.Controller;

namespace LevelObjects.MovingObjects
{
    public class PlatformMovement : MonoBehaviour
    {
        public float speed;
        public Transform startPos;

        /// <summary>
        /// Checks to see if player is currently blocking the platforms path
        /// </summary>
        public bool IsPlayerBlockingPath { get; protected set; } = false;
        /// <summary>
        /// Checks to see if the platform is touching the ground
        /// </summary>
        public bool TouchingGround { get; protected set; } = false;
        /// <summary>
        /// When an object collides with the platform check to see if can be attached then attach object to the platform
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!GeneralFunctions.IsObjectPlayer(collision.gameObject) && !GeneralFunctions.IsObjectOnLayer("Attached Leech", collision.gameObject))
            {
                GeneralFunctions.AttachObjectToTransfrom(gameObject.transform, collision.gameObject);

                RigidbodyManager rigidbodyManager = collision.gameObject.GetComponent<RigidbodyManager>();

                if (rigidbodyManager)
                {
                    rigidbodyManager.OnPlatfromEnter();
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
                    rigidbodyManager.OnPlatfromExit();
                }
            }
        }
        /// <summary>
        /// Checks to see if a enemy is blocking it's path if it hits a enemy that's close to ground enemy is killed if a player is blocking it's path it will set IsPlayerBlockingPath to true
        /// </summary>
        /// <param name="collision"></param>
        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (GeneralFunctions.IsObjectOnLayer("Enemy", collision.gameObject))
            {
                var enemy = collision.gameObject.GetComponent<EnemyMovement>();

                if (enemy.TouchingGround())
                {
                    GeneralFunctions.KillTarget(collision.gameObject);
                }
            }
            else if (GeneralFunctions.IsObjectPlayer(collision.gameObject))
            {
                var playerLegs = collision.gameObject.transform.GetChild(0).GetComponent<PlayerLegs>();

                if (playerLegs.TouchingGround())
                {
                    IsPlayerBlockingPath = true;
                }
                else
                {
                    IsPlayerBlockingPath = false;
                }
            }
            else if (collision.gameObject.CompareTag("Ground"))
            {
                TouchingGround = true;
            }
        }
        /// <summary>
        /// If player has stopped overlapping the platform and was touching the ground IsPlayerBlockingPath is set to false
        /// </summary>
        /// <param name="collision"></param>
        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            if (GeneralFunctions.IsObjectPlayer(collision.gameObject))
            {
                var playerLegs = collision.gameObject.transform.GetChild(0).GetComponent<PlayerLegs>();

                if (playerLegs.TouchingGround() && !TouchingGround)
                {
                    IsPlayerBlockingPath = false;
                }
            }
            else if (collision.gameObject.CompareTag("Ground"))
            {
                TouchingGround = false;
            }
        }
        /// <summary>
        /// Checks to see if a enemy is blocking it's path if it hits a enemy that's close to ground enemy is killed if a player is blocking it's path it will set IsPlayerBlockingPath to true
        /// </summary>
        /// <param name="collision"></param>
        protected virtual void OnTriggerStay2D(Collider2D collision)
        {
            if (GeneralFunctions.IsObjectOnLayer("Enemy", collision.gameObject))
            {
                var enemy = collision.gameObject.GetComponent<EnemyMovement>();

                if (enemy.TouchingGround())
                {
                    GeneralFunctions.KillTarget(collision.gameObject);
                }
            }
            else if (GeneralFunctions.IsObjectPlayer(collision.gameObject))
            {
                var playerLegs = collision.gameObject.transform.GetChild(0).GetComponent<PlayerLegs>();

                if (playerLegs.TouchingGround())
                {
                    IsPlayerBlockingPath = true;
                }
                else
                {
                    IsPlayerBlockingPath = false;
                }
            }
            else if (collision.gameObject.CompareTag("Ground"))
            {
                TouchingGround = true;
            }
        }
    }
}