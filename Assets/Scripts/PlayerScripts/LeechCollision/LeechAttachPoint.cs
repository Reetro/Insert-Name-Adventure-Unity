using UnityEngine;

namespace PlayerScripts.LeechCollision
{
    public class LeechAttachPoint : MonoBehaviour
    {
        [SerializeField] private Vector2 offset = new Vector2(0, 0);
        private GameObject player = null;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        private void LateUpdate()
        {
            transform.position = player.transform.position + (Vector3)offset;
        }
    }
}