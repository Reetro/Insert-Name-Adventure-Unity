using UnityEngine;

[RequireComponent(typeof(HealthComponent), typeof(Rigidbody2D), typeof(GameplayObjectID))]
[RequireComponent(typeof(EnemyMovement), typeof(Animator))]
public class EnemyBase : MonoBehaviour
{
    private HealthComponent healthComp = null;
    private Transform playerTransform = null;
    private Rigidbody2D myRigidBody2D = null;
    private GameplayObjectID idObject = null;
    private EnemyMovement enemyMovement = null;
    private Animator animator = null;

    protected virtual void Awake()
    {
        playerTransform = GeneralFunctions.GetPlayerGameObject().transform;
        healthComp = GetComponent<HealthComponent>();
        myRigidBody2D = GetComponent<Rigidbody2D>();
        idObject = GetComponent<GameplayObjectID>();
        enemyMovement = GetComponent<EnemyMovement>();
        animator = GetComponent<Animator>();

        idObject.ConstructID();
        healthComp.ConstructHealthComponent();
    }
    /// <summary>
    /// Get this Gameobjects health component
    /// </summary>
    public HealthComponent GetHealthComponent()
    {
        return healthComp;
    }
    /// <summary>
    /// Get the players current transform
    /// </summary>
    public Transform GetPlayerTransform()
    {
        return playerTransform;
    }
    /// <summary>
    /// Gets this Gameobjects Rigidbody
    /// </summary>
    public Rigidbody2D GetRigidbody2D()
    {
        return myRigidBody2D;
    }
    /// <summary>
    /// Gets this Gameobjects ID
    /// </summary>
    public int GetID()
    {
        return idObject.GetID();
    }
    /// <summary>
    /// Gets this Gameobjects movement component
    /// </summary>
    public EnemyMovement GetEnemyMovementComponent()
    {
        return enemyMovement;
    }
    /// <summary>
    /// Get this Gameobjects animator component
    /// </summary>
    public Animator GetAnimatorComponent()
    {
        return animator;
    }
    /// <summary>
    /// Make this Gameobject look towards the player
    /// </summary>
    public void LookAtPlayer()
    {
        enemyMovement.LookAtTarget(playerTransform);
    }
    /// <summary>
    /// Called when the current health on health component is 0 or below by default will only disable enemy collision
    /// </summary>
    public virtual void OnDeath()
    {
        GetComponent<Collider2D>().enabled = false;
    }
}