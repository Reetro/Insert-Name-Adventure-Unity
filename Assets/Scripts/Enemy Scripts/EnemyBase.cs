using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    private HealthComponent healthComp = null;
    private Transform playerTransform = null;

    protected virtual void Start()
    {
        playerTransform = GeneralFunctions.GetPlayerGameObject().transform;
        healthComp = GetComponent<HealthComponent>();

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
}
