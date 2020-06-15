using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(EnemyMovement))]
public class ProjectileMovement : MonoBehaviour
{
    public EnemyMovement controller = null;

    private Transform playerTransform = null;
    private Rigidbody2D myRigidbody = null;
    private float moveSpeed = 4f;

    private Vector2 launchDirection;
    private bool canFire = false;
    private float damage = 1f;

    public void ConstructProjectile(float moveSpeed, float damage)
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        myRigidbody = GetComponent<Rigidbody2D>();

        this.damage = damage;
        this.moveSpeed = moveSpeed;

        launchDirection = playerTransform.position - transform.position;

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
