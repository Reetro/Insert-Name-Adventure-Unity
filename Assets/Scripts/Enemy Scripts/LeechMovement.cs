using UnityEngine;

[RequireComponent(typeof(EnemyMovement), typeof(HealthComponent))]
public class LeechMovement : MonoBehaviour
{
    [SerializeField] EnemyMovement controller = null;

    [Header("Leech Movement Settings")]
    [SerializeField] float leechFlySpeed = 4f;
    [SerializeField] float randomAmountToAddToYmin = 0.005f;
    [SerializeField] float randomAmountToAddToYmax = 0.007f;

    private Rigidbody2D myRigidbody2D = null;
    private Animator myAnimator = null;
    private Transform player = null;
    private HealthComponent myHealth = null;

    private void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myHealth = GetComponent<HealthComponent>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    private void Update()
    {
        if (!myHealth.GetIsDead())
        {
            controller.LookAtTarget(player);

            var amountToAddToY = GeneralFunctions.CreateRandomVector2OnlyY(randomAmountToAddToYmin, randomAmountToAddToYmax);

            controller.AddToLeechY(transform, amountToAddToY.y);
        }
    }

    private void FixedUpdate()
    {
        if (!myHealth.GetIsDead())
        {
            controller.MoveAITowards(player, myRigidbody2D, leechFlySpeed);
        }
    }

    public void OnDeath()
    {
        myAnimator.SetBool("IsDead", true);

        controller.StopMovement(myRigidbody2D);
    }

    public void OnDeathAnimationEnd()
    {
        Destroy(gameObject);
    }
}
