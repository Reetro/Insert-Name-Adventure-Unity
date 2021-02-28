using System.Collections;
using GameplayScripts.GeneralMovementScripts;
using UnityEngine;

namespace GameplayScripts.Traps
{
    public class Cannon : MonoBehaviour
    {
        [SerializeField] private ProjectileMovement projectile = null;
        [SerializeField] private Transform fireLocation = null;
        [SerializeField] private float shootIntervale = 3f;
        [SerializeField] private float projectileSpeed = 300f;
        [SerializeField] private float projectileDamage = 4f;

        private void Start()
        {
            StartCoroutine(Shoot());
        }

        private IEnumerator Shoot()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(shootIntervale);

                if (projectile)
                {
                    ProjectileMovement spawnedProjectile = Instantiate(projectile, fireLocation.position, Quaternion.identity);

                    spawnedProjectile.ConstructProjectile(projectileSpeed, projectileDamage, fireLocation.right);
                }
            }
        }
    }
}