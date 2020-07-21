using UnityEngine;

namespace LevelObjects.Trap
{
    public class LockedDoor : MonoBehaviour
    {
        public Transform moveTo;
        public float speed = 2f;

        private bool canMove = false;
        public void SetCanMove()
        {
            canMove = true;
        }

        void Update()
        {
            if (canMove)
            {
                transform.position = Vector3.MoveTowards(transform.position, moveTo.position, speed * Time.deltaTime);
            }
        }
    }
}