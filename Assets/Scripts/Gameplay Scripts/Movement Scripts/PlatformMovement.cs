using UnityEngine;
using EnemyCharacter.AI;

namespace LevelObjects.MovingObjects
{
    public class PlatformMovement : MonoBehaviour
    {
        public float speed;
        public Transform startPos;

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
        }
    }
}