using UnityEngine;

namespace EnemyCharacter.SceneObject
{
    public class LeechEggRipe : MonoBehaviour
    {
        [Tooltip("Higher values will make the egg fall faster")]
        [SerializeField] private float newGravityScale = 1f;
        [Tooltip("The leech to actually spawn")]
        [SerializeField] private GameObject leechToSpawn = null;
        [Tooltip("Determines if the leech is dropping down to player or if it just spawns as soon as the player enters the collision radius")]
        [SerializeField] private bool dropping = true;

        private Rigidbody2D myRigidbody = null;
        private bool dropped = false;

        void Start()
        {
            myRigidbody = GetComponent<Rigidbody2D>();
            dropped = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (dropping)
            {
                var hitTag = collision.transform.tag;

                if (hitTag == "Player")
                {
                    myRigidbody.gravityScale = newGravityScale;
                    dropped = true;
                }
            }
            else
            {
                var hitTag = collision.transform.tag;

                if (hitTag == "Player")
                {
                    SpawnLeech();
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (dropped)
            {
                SpawnLeech();
            }
        }

        public void SpawnLeech()
        {
            Instantiate(leechToSpawn, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}