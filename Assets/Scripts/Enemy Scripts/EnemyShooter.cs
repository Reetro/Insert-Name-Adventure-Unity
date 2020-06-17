using System.Collections;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
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

        StartCoroutine(ShootInterval());
    }

    private IEnumerator ShootInterval()
    {
        while (!healthComp.GetIsDead())
        {
            yield return new WaitForSecondsRealtime(shootIntervale);

            Shoot();
        }
    }

    protected virtual void Shoot()
    {
        // for use in children
        Debug.LogWarning(gameObject.name + "shoot script has no implementation");
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