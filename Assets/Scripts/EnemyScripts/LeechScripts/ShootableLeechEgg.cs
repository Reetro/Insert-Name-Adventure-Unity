using GameplayScripts.GeneralMovementScripts;
using UnityEngine;

namespace EnemyScripts.LeechScripts
{
    public class ShootableLeechEgg : ProjectileMovement
    {
        [SerializeField] private GameObject leechToSpawn = null;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            SpawnLeech();
        }

        private void SpawnLeech()
        {
            Instantiate(leechToSpawn, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}