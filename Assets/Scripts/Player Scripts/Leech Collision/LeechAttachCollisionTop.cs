using UnityEngine;
using AuraSystem;
using EnemyCharacter;

namespace PlayerCharacter.Collision
{
    public class LeechAttachCollisionTop : MonoBehaviour
    {
        [SerializeField] private AttachedLeech leechToAttach = null;
        [SerializeField] private Vector2 offset = new Vector2(0, 0);
        [SerializeField] private string tagToAttach = "Leech Collision Top";

        private GameObject player = null;
        private AuraManager auraManager = null;

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");

            auraManager = player.GetComponent<AuraManager>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Attachable Leech"))
            {
                if (GeneralFunctions.CanLeechAttach(tagToAttach) && !GeneralFunctions.IsObjectDead(collision.gameObject))
                {
                    var leechHP = collision.gameObject.GetComponent<HealthComponent>();

                    var spawnTransform = GeneralFunctions.GetLeechAttachPointByTag(tagToAttach);

                    GeneralFunctions.SpawnLeechAttach(auraManager, leechToAttach, spawnTransform, leechHP.CurrentHealth, player);

                    Destroy(collision.gameObject);
                }
            }
        }

        private void LateUpdate()
        {
            transform.position = player.transform.position + (Vector3)offset;
        }
    }
}