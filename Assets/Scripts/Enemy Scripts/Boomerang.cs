using UnityEngine;

public class Boomerang : ProjectileMovement
{
    private Shaman currentShaman = null;
    private int currentHitCount = 1;
    private int maxHitsBeforeTeleport = 0;

    protected override void Start()
    {
        base.Start();

        GetRigidbody2D().freezeRotation = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 _wallNormal = collision.contacts[0].normal;

        Vector2 newDirection = Vector2.Reflect(GetCurrentVelocity(), _wallNormal);

        UpdateDirection(newDirection);

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<HealthComponent>().ProccessDamage(damage);

            DestroyBoomerang();
        }
        else if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Shaman"))
        {
            currentHitCount++;

            if (currentHitCount >= maxHitsBeforeTeleport)
            {
                TeleportShaman(collision.contacts[0].point);

                currentHitCount = 1;
            }
        }
    }

    private void TeleportShaman(Vector2 teleportLocation)
    {
        currentShaman.transform.position = teleportLocation;

        GeneralFunctions.LookAt2D(currentShaman.transform.position, transform.position, currentShaman.gameObject);

        currentShaman.GetRigidbody2D().velocity = new Vector2(0, 0);
    }

    public void DestroyBoomerang()
    {
        if (currentShaman)
        {
            currentShaman.ThrowBoomerang();

            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("Unable to destroy boomerang " + name + "currentShaman is invalid");
        }
    }

    public void SetCurrentShaman(Shaman shamanToSet, int maxHitsBeforeTeleport)
    {
        currentShaman = shamanToSet;

        this.maxHitsBeforeTeleport = maxHitsBeforeTeleport;
    }
}
