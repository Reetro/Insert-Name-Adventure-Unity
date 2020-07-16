using UnityEngine;

public class LeechMother : EnemyShooter
{
    protected override void Shoot()
    {
        if (!GeneralFunctions.IsObjectAbove(PlayerTransform.position, transform.position))
        {
            ProjectileMovement bulllet = Instantiate(ProjectileToShoot, FireTransform.position, Quaternion.identity);

            Vector2 launchDirection = PlayerTransform.position - transform.position;

            bulllet.ConstructProjectile(ProjectileSpeed, ProjectileDamage, launchDirection);
        }
    }
}