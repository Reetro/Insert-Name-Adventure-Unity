using UnityEngine;

public class Shaman : MonoBehaviour
{
    [SerializeField] private Boomerang boomerangToSpawn = null;

    [Header("Boomerang settings")]
    [SerializeField] private int maxHitsBeforeTeleport = 2;
    [SerializeField] private float boomerangSpeed = 300f;
    [SerializeField] private float boomerangDamage = 2f;
    [SerializeField] private float boomerangMinRandomFactor = 300f;
    [SerializeField] private float boomerangMaxRandomFactor = 600f;
    [SerializeField] private float teleportOffset = 0.5f;

    private Rigidbody2D myRigidbody = null;
    private Boomerang currentBoomrang = null;

    void Start()
    {
        ThrowBoomerang();

        myRigidbody = GetComponent<Rigidbody2D>();

        GeneralFunctions.ConstructHPComponent(gameObject);
    }

    public void ThrowBoomerang()
    {
        currentBoomrang = Instantiate(boomerangToSpawn, transform.position, transform.rotation) as Boomerang;

        currentBoomrang.SetCurrentShaman(this, maxHitsBeforeTeleport, teleportOffset);

        currentBoomrang.ConstructProjectileWithNoise(boomerangSpeed, boomerangDamage, transform.position, boomerangMinRandomFactor, boomerangMaxRandomFactor);
    }

    public void OnDeath()
    {
        currentBoomrang.DestroyBoomerang(false);

        Destroy(gameObject);
    }

    public Rigidbody2D GetRigidbody2D()
    {
        return myRigidbody;
    }
}
