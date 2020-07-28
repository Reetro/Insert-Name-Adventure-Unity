using UnityEngine;

namespace LevelObjects.MovingObjects
{
    public class ShootableLeechEgg : ProjectileMovement
    {
        [SerializeField] private GameObject leechToSpawn = null;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            SpawnLeech();
        }

        public void SpawnLeech()
        {
            Instantiate(leechToSpawn, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}