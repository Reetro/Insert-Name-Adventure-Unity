using UnityEngine;

public class LeechMother : EnemyShooter
{
    protected override void Shoot()
    {
        if (!GeneralFunctions.IsObjectAbove(PlayerTransform.position, transform.position))
        {
            ProjectileMovement bulllet = Instantiate(GetProjectile(), GetFirePoint().position, Quaternion.identity) as ProjectileMovement;

            Vector2 launchDirection = PlayerTransform.position - transform.position;

            bulllet.ConstructProjectile(GetProjectileSpeed(), GetProjectileDamage(), launchDirection);
        }
    }
}
