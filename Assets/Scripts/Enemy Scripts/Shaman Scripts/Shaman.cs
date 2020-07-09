﻿using UnityEngine;

public class Shaman : EnemyBase
{
    [SerializeField] private Boomerang boomerangToSpawn = null;

    [Header("Boomerang settings")]
    [SerializeField] private int maxHitsBeforeTeleport = 2;
    [SerializeField] private float boomerangSpeed = 300f;
    [SerializeField] private float boomerangDamage = 2f;
    [SerializeField] private float boomerangMinRandomFactor = 300f;
    [SerializeField] private float boomerangMaxRandomFactor = 600f;
    [SerializeField] private float teleportOffset = 0.5f;

    private Boomerang currentBoomrang = null;

    private void Start()
    {
        ThrowBoomerang();
    }

    public void ThrowBoomerang()
    {
        var playerLocation = GeneralFunctions.GetPlayerGameObject().transform.position;

        currentBoomrang = Instantiate(boomerangToSpawn, transform.position, transform.rotation);

        currentBoomrang.SetCurrentShaman(this, maxHitsBeforeTeleport, teleportOffset);

        var startDirection = GeneralFunctions.GetDirectionVectroFrom2Vectors(playerLocation, currentBoomrang.transform.position);

        print(startDirection);

        currentBoomrang.ConstructProjectileWithNoise(boomerangSpeed, boomerangDamage, startDirection, boomerangMinRandomFactor, boomerangMaxRandomFactor);

        currentBoomrang.UpdateVelocity(playerLocation - currentBoomrang.transform.position);
    }

    public override void OnDeath()
    {
        base.OnDeath();

        if (currentBoomrang)
        {
            currentBoomrang.DestroyBoomerang(false);
        }

        Destroy(gameObject);
    }
}