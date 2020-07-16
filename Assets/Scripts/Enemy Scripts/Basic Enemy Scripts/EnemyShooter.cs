using System.Collections;
using UnityEngine;

public class EnemyShooter : EnemyBase
{
    [SerializeField] private ProjectileMovement projectilePrefab = null;
    [SerializeField] private float shootIntervale = 2f;
    [SerializeField] private float projectileSpeed = 400f;
    [SerializeField] private float projectileDamage = 1f;
    [SerializeField] private Transform firePoint = null;

    private void Start()
    {
        StartCoroutine(ShootInterval());
    }

    private IEnumerator ShootInterval()
    {
        while (MyHealthComponent.GetIsDead())
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
}