using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileMovement : MonoBehaviour
{
    private Rigidbody2D myRigidbody = null;
    private float currentMoveSpeed = 400f;
    private Vector2 currentVelocity;

    private Vector2 launchDirection;
    protected bool canFire = false;
    protected bool useNoise = false;
    protected float damage = 1f;

    private float minNoise = 0f;
    private float maxNoise = 0f;

    [Header("Events")]
    [Space]
    public UnityEvent OnImpact;

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
        useNoise = false;
    }

    public virtual void ConstructProjectileWithNoise(float moveSpeed, float damage, Vector2 launchDirection, float noiseMin, float noiseMax)
    {
        this.damage = damage;
        currentMoveSpeed = moveSpeed;
        this.launchDirection = launchDirection + GetNoise(noiseMin, noiseMax);

        minNoise = noiseMin;
        maxNoise = noiseMax;

        canFire = true;
        useNoise = true;
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

        if (canFire && useNoise)
        {
            var newDirection = launchDirection.normalized + GetNoise(minNoise, maxNoise);

            myRigidbody.velocity = newDirection * currentMoveSpeed * Time.fixedDeltaTime;
        }

        if (canFire)
        {
            myRigidbody.velocity = launchDirection.normalized * currentMoveSpeed * Time.fixedDeltaTime;
        }
    }

    public void OnProjectileImpact()
    {
        Destroy(gameObject);
    }

    public Vector2 GetNoise(float min, float max)
    {
        // Find random angle between min & max inclusive
        float xNoise = Random.Range(min, max);
        float yNoise = Random.Range(min, max);

        Vector2 noise = new Vector2(2 * Mathf.PI * xNoise / 360, 2 * Mathf.PI * yNoise / 360);

        return noise;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HealthComponent healtComp = collision.transform.GetComponent<HealthComponent>();

            healtComp.ProccessDamage(damage, true);

            OnImpact.Invoke();
        }
        else
        {
            OnImpact.Invoke();
        }
    }
}
