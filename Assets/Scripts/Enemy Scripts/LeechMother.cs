using System.Collections;
using UnityEngine;

public class LeechMother : MonoBehaviour
{
    [SerializeField] private ProjectileMovement projectilePrefab = null;
    [SerializeField] private float shootIntervale = 2f;
    [SerializeField] private float projectileSpeed = 400f;
    [SerializeField] private float projectileDamage = 1f;
    [SerializeField] private Transform firePoint = null;

    private HealthComponent healthComp = null;
    private Transform playerTransform = null;

    protected virtual void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        healthComp = GetComponent<HealthComponent>();

        StartCoroutine(Shoot());
    }

    protected virtual IEnumerator Shoot()
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

    public ProjectileMovement GetProjectile()
    {
        return projectilePrefab;
    }

    public float GetShootInterval()
    {
        return shootIntervale;
    }

    public float GetProjectileSpeed()
    {
        return projectileSpeed;
    }

    public float GetProjectileDamage()
    {
        return projectileDamage;
    }

    public Transform GetFirePoint()
    {
        return firePoint;
    }

    public HealthComponent GetHealthComponent()
    {
        return healthComp;
    }

    public Transform GetPlayerTransform()
    {
        return playerTransform;
    }
}