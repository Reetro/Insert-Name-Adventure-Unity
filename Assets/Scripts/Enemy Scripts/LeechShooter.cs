using System.Collections;
using UnityEngine;

public class LeechShooter : MonoBehaviour
{
    [SerializeField] ProjectileMovement projectilePrefab = null;
    [SerializeField] float shootIntervale = 2f;
    [SerializeField] float projectileSpeed = 4f;
    [SerializeField] float projectileDamage = 1f;
    [SerializeField] Transform firePoint = null;

    private HealthComponent healthComp = null;
    private Transform playerTransform = null;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        healthComp = GetComponent<HealthComponent>();

        StartCoroutine(Shoot());
    }

    public IEnumerator Shoot()
    {
        while (!healthComp.GetIsDead())
        {
            yield return new WaitForSecondsRealtime(shootIntervale);

            if (!GeneralFunctions.IsObjectAbove(playerTransform.position, transform.position))
            {
                ProjectileMovement bulllet = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity) as ProjectileMovement;

                Vector2 launchDirection = playerTransform.position - transform.position;

                bulllet.ConstructProjectile(projectileSpeed, projectileDamage, launchDirection);
            }
        }
    }
}
