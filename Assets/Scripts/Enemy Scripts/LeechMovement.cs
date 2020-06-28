using UnityEngine;

[RequireComponent(typeof(EnemyMovement), typeof(HealthComponent))]
public class LeechMovement : EnemyBase
{
    [SerializeField] EnemyMovement controller = null;

    [Header("Leech Movement Settings")]
    [SerializeField] float leechFlySpeed = 4f;
    [SerializeField] float randomAmountToAddToYmin = 0.005f;
    [SerializeField] float randomAmountToAddToYmax = 0.007f;

    private Rigidbody2D myRigidbody2D = null;
    private Animator myAnimator = null;

    protected override void Start()
    {
        base.Start();

        myRigidbody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!GetHealthComponent().GetIsDead())
        {
            controller.LookAtTarget(GetPlayerTransform());

            var amountToAddToY = GeneralFunctions.CreateRandomVector2OnlyY(randomAmountToAddToYmin, randomAmountToAddToYmax);

            controller.AddToLeechY(transform, amountToAddToY.y);
        }
    }

    private void FixedUpdate()
    {
        if (!GetHealthComponent().GetIsDead())
        {
            controller.MoveAITowards(GetPlayerTransform(), myRigidbody2D, leechFlySpeed);
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
