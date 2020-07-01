using System.Collections;
using UnityEngine;

public class LeechFather : EnemyShooter
{
    protected override void Shoot()
    {
        ProjectileMovement bulllet = Instantiate(GetProjectile(), GetFirePoint().position, Quaternion.identity) as ProjectileMovement;

        Vector2 launchDirection = gameObject.transform.TransformDirection(GetFirePoint().position);

        bulllet.ConstructProjectile(GetProjectileSpeed(), GetProjectileDamage(), launchDirection);
    }
}
