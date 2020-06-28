using UnityEngine;

[RequireComponent(typeof(HealthComponent), typeof(Rigidbody2D), typeof(GameplayObjectID))]
public class EnemyBase : MonoBehaviour
{
    private HealthComponent healthComp = null;
    private Transform playerTransform = null;
    private Rigidbody2D myRigidBody2D = null;
    private GameplayObjectID idObject = null;

    protected virtual void Start()
    {
        playerTransform = GeneralFunctions.GetPlayerGameObject().transform;
        healthComp = GetComponent<HealthComponent>();
        myRigidBody2D = GetComponent<Rigidbody2D>();
        idObject = GetComponent<GameplayObjectID>();

        GeneralFunctions.ConstructHPComponent(gameObject);
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
}
