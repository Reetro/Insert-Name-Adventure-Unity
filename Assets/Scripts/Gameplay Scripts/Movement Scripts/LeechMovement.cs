using UnityEngine;

public class LeechMovement : EnemyBase
{
    [Header("Leech Movement Settings")]
    [SerializeField] float leechFlySpeed = 4f;
    [SerializeField] float randomAmountToAddToYmin = 0.005f;
    [SerializeField] float randomAmountToAddToYmax = 0.007f;

    private void Update()
    {
        if (!MyHealthComponent.GetIsDead())
        {
            LookAtPlayer();

            var amountToAddToY = GeneralFunctions.CreateRandomVector2OnlyY(randomAmountToAddToYmin, randomAmountToAddToYmax);

            MovementComp.AddToLeechY(transform, amountToAddToY.y);
        }
    }

    private void FixedUpdate()
    {
        if (!MyHealthComponent.GetIsDead())
        {
            MovementComp.MoveAITowards(PlayerTransform, MyRigidBody2D, leechFlySpeed);
        }
    }

    public override void OnDeath()
    {
        base.OnDeath();

        MyAnimator.SetBool("IsDead", true);

        MovementComp.StopMovement(MyRigidBody2D);
    }

    public void OnDeathAnimationEnd()
    {
        Destroy(gameObject);
    }
}