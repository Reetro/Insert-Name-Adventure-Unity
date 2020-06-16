using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileMovement : MonoBehaviour
{
    private Rigidbody2D myRigidbody = null;
    private float moveSpeed = 4f;

    private Vector2 launchDirection;
    private bool canFire = false;
    private float damage = 1f;

    public virtual void ConstructProjectile(float moveSpeed, float damage, Vector2 launchDirection)
    {
        myRigidbody = GetComponent<Rigidbody2D>();

        this.damage = damage;
        this.moveSpeed = moveSpeed;

        this.launchDirection = launchDirection;

        canFire = true;
    }

    private void FixedUpdate()
    {
        if (canFire)
        {
            myRigidbody.velocity = launchDirection.normalized * moveSpeed * Time.fixedDeltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var hitTag = collision.transform.tag;

        if (hitTag == "Player")
        {
            HealthComponent healtComp = collision.transform.GetComponent<HealthComponent>();

            healtComp.ProccessDamage(damage);

            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
