using PlayerCharacter.Controller;
using UnityEngine;

namespace LevelObjects.MovingObjects
{
    public class OneShotMovement : PlatformMovement
    {
        public Transform targetDirection;

        private Vector3 normalizeDirection;
        private bool canMove = true;
        private bool touchingGround = false;

        private void Awake()
        {
            normalizeDirection = (targetDirection.position - transform.position).normalized;
        }

        private void Update()
        {
            if (canMove)
            {
                transform.position += normalizeDirection * speed * Time.deltaTime;
            }
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);

            if (collision.gameObject.CompareTag("Ground") || IsPlayerBlockingPath)
            {
                canMove = false;
                touchingGround = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                canMove = true;
                touchingGround = false;
            }
            else if (GeneralFunctions.IsObjectPlayer(collision.gameObject))
            {
                var playerLegs = collision.gameObject.transform.GetChild(0).GetComponent<PlayerLegs>();

                if (playerLegs.TouchingGround() && !touchingGround)
                {
                    canMove = true;
                    IsPlayerBlockingPath = false;
                }
            }
        }
    }
}