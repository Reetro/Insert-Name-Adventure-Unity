using UnityEngine;
using StatusEffects;
using EnemyCharacter;
using EnemyCharacter.AI;
using ComponentLibrary;

namespace PlayerCharacter.Collision
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
            if (collision.gameObject.CompareTag("Attachable Leech"))
            {
                var leechID = collision.gameObject.GetComponent<LeechMovement>().MyID;

                if (GeneralFunctions.CanLeechAttach(tagToAttach, leechID) && !GeneralFunctions.IsObjectDead(collision.gameObject))
                {
                    var leechHP = collision.gameObject.GetComponent<HealthComponent>();

                    var spawnTransform = GeneralFunctions.GetLeechAttachPointByTag(tagToAttach);

                    GeneralFunctions.SpawnLeechAttach(leechToAttach, spawnTransform, leechHP.CurrentHealth, player, leechID);

                    if (GeneralFunctions.IsObjectOnLayer("Enemy", collision.gameObject))
                    {
                        Destroy(collision.gameObject);
                    }
                }
            }
        }

        private void LateUpdate()
        {
            transform.position = player.transform.position + (Vector3)offset;
        }
    }
}