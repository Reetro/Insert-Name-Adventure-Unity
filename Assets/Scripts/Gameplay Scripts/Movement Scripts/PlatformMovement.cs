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
        }

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
        }
    }
}