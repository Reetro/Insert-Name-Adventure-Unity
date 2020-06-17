using System.Collections;
using UnityEngine;

public class LeechFather : LeechMother
{
    protected override IEnumerator Shoot()
    {
        while (!GetHealthComponent().GetIsDead())
        {
            yield return new WaitForSecondsRealtime(GetShootInterval());

            ProjectileMovement bulllet = Instantiate(GetProjectile(), GetFirePoint().position, Quaternion.identity) as ProjectileMovement;

            Vector2 launchDirection = gameObject.transform.TransformDirection(GetFirePoint().position);

            bulllet.ConstructProjectile(GetProjectileSpeed(), GetProjectileDamage(), launchDirection);
        }
    }
}
