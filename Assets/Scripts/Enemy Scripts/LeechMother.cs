﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeechMother : EnemyShooter
{
    protected override void Shoot()
    {
        if (!GeneralFunctions.IsObjectAbove(GetPlayerTransform().position, transform.position))
        {
            ProjectileMovement bulllet = Instantiate(GetProjectile(), GetFirePoint().position, Quaternion.identity) as ProjectileMovement;

            Vector2 launchDirection = GetPlayerTransform().position - transform.position;

            bulllet.ConstructProjectile(GetProjectileSpeed(), GetProjectileDamage(), launchDirection);
        }
    }
}