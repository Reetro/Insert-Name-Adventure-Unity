using UnityEngine;

public class LeechMovement : EnemyBase
{
    [Header("Leech Movement Settings")]
    [SerializeField] float leechFlySpeed = 4f;
    [SerializeField] float randomAmountToAddToYmin = 0.005f;
    [SerializeField] float randomAmountToAddToYmax = 0.007f;

    private void Update()
    {
        if (!GetHealthComponent().GetIsDead())
        {
            LookAtPlayer();

            var amountToAddToY = GeneralFunctions.CreateRandomVector2OnlyY(randomAmountToAddToYmin, randomAmountToAddToYmax);

            GetEnemyMovementComponent().AddToLeechY(transform, amountToAddToY.y);
        }
    }

    private void FixedUpdate()
    {
        if (!GetHealthComponent().GetIsDead())
        {
            GetEnemyMovementComponent().MoveAITowards(GetPlayerTransform(), GetRigidbody2D(), leechFlySpeed);
        }
    }

    public override void OnDeath()
    {
        base.OnDeath();

        GetAnimatorComponent().SetBool("IsDead", true);

        GetEnemyMovementComponent().StopMovement(GetRigidbody2D());
    }

    public void OnDeathAnimationEnd()
    {
        Destroy(gameObject);
    }
}