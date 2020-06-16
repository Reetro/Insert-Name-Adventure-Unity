using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileMovement : MonoBehaviour
{
    private Rigidbody2D myRigidbody = null;
    private float currentMoveSpeed = 4f;
    private Vector2 currentVelocity;

    private Vector2 launchDirection;
    protected bool canFire = false;
    protected float damage = 1f;

    public bool destroyOnImpact = true;

    protected virtual void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    public virtual void ConstructProjectile(float moveSpeed, float damage, Vector2 launchDirection)
    {
        this.damage = damage;
        currentMoveSpeed = moveSpeed;
        this.launchDirection = launchDirection;

        canFire = true;
    }

    public void UpdateDirection(Vector2 newDirection)
    {
        launchDirection = newDirection;
    }

    public Vector2 GetCurrentVelocity()
    {
        return currentVelocity;
    }

    public Rigidbody2D GetRigidbody2D()
    {
        return myRigidbody;
    }

    protected virtual void FixedUpdate()
    {
        currentVelocity = myRigidbody.velocity;

        if (canFire)
        {
            myRigidbody.velocity = launchDirection.normalized * currentMoveSpeed * Time.fixedDeltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HealthComponent healtComp = collision.transform.GetComponent<HealthComponent>();

            healtComp.ProccessDamage(damage);

            if (destroyOnImpact)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (destroyOnImpact)
            {
                Destroy(gameObject);
            }
        }
    }
}
