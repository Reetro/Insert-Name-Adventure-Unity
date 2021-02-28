using EnemyScripts.LeechScripts;
using GeneralScripts;
using GeneralScripts.GeneralComponents;
using UnityEngine;

namespace PlayerScripts.LeechCollision
{
    public class LeechAttachCollisionTop : MonoBehaviour
    {
        [SerializeField] private AttachedLeech leechToAttach = null;
        [SerializeField] private Vector2 offset = new Vector2(0, 0);
        [SerializeField] private string tagToAttach = "Leech Collision Top";

        private GameObject player = null;

        private void Awake()
        {
            player = GeneralFunctions.GetPlayerGameObject();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.gameObject.CompareTag("Attachable Leech")) return;
            var leechID = collision.gameObject.GetComponent<LeechMovement>().MyID;

            if (!GeneralFunctions.CanLeechAttach(tagToAttach, leechID) ||
                GeneralFunctions.IsObjectDead(collision.gameObject)) return;
            var leechHp = collision.gameObject.GetComponent<HealthComponent>();

            var spawnTransform = GeneralFunctions.GetLeechAttachPointByTag(tagToAttach);

            GeneralFunctions.SpawnLeechAttach(leechToAttach, spawnTransform, leechHp.CurrentHealth, player, leechID);

            if (GeneralFunctions.IsObjectOnLayer("Enemy", collision.gameObject))
            {
                Destroy(collision.gameObject);
            }
        }

        private void LateUpdate()
        {
            transform.position = player.transform.position + (Vector3)offset;
        }
    }
}