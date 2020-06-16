using UnityEngine;

public class Shaman : MonoBehaviour
{
    [SerializeField] private Boomerang boomerangToSpawn = null;

    [Header("Boomerang settings")]
    [SerializeField] private int maxHitsBeforeTeleport = 2;
    [SerializeField] private float boomerangSpeed = 300f;
    [SerializeField] private float boomerangDamage = 2f;

    private Rigidbody2D myRigidbody = null;

    void Start()
    {
        ThrowBoomerang();

        myRigidbody = GetComponent<Rigidbody2D>();
    }

    public void ThrowBoomerang()
    {
        Boomerang boomerang = Instantiate(boomerangToSpawn, transform.position, transform.rotation) as Boomerang;

        boomerang.SetCurrentShaman(this, maxHitsBeforeTeleport);

        boomerang.ConstructProjectile(boomerangSpeed, boomerangDamage, transform.position);
    }

    public Rigidbody2D GetRigidbody2D()
    {
        return myRigidbody;
    }
}
