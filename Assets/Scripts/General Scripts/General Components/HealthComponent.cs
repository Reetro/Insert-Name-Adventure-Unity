using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    [System.Serializable]
    public class TakeAnyDamge : UnityEvent<float> { }

    [Header("Health Settings")]
    [SerializeField] float maxHealth = 10f;
    
    [Header("Health Bar Settings")]
    [SerializeField] HealthBar healthBar = null;

    [Header("Events")]
    [Space]
    public UnityEvent OnDeath;
    [Space]
    public TakeAnyDamge onTakeAnyDamage;

    private float currentHealth = 0f;
    private bool isDead = false;
    private PlayerState playerState = null;
    private GameplayManager gameplayManager = null;

    private static bool setMaxhealth = false;

    /// <summary>
    /// Setup all needed health component variables and update player state if on player
    /// </summary>
    public void ConstructHealthComponent()
    {
        gameplayManager = GameObject.FindGameObjectWithTag("Gameplay Manager").GetComponent<GameplayManager>();

        isDead = false;

        if (IsOnPlayer())
        {
            if (!setMaxhealth)
            {
                currentHealth = maxHealth;

                if (healthBar)
                {
                    healthBar.SetMaxHealth(maxHealth);
                }

                setMaxhealth = true;

                UpdatePlayerState();
            }
            else
            {
                maxHealth = playerState.GetCurrentMaxHealth();
                currentHealth = playerState.GetCurrentHealth();

                if (healthBar)
                {
                    healthBar.SetHealth(currentHealth);
                }

                UpdatePlayerState();
            }
        }
        else
        {
            currentHealth = maxHealth;

            if (healthBar)
            {
                healthBar.SetMaxHealth(maxHealth);
            }
        }
    }
    /// <summary>
    /// Add the given value to this Gameobjects current health
    /// </summary>
    /// <param name="amountToAdd"></param>
    public void AddHealth(float amountToAdd)
    {
        currentHealth = Mathf.Clamp(currentHealth + amountToAdd, 0, maxHealth);

        if (healthBar)
        {
            healthBar.SetHealth(currentHealth);
        }

        if (IsOnPlayer())
        {
            UpdatePlayerState();
        }
    }
    /// <summary>
    /// Apply damage to the Gameobject this component is on if current health is below 0 the Gameobject is killed
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="showDamageText"></param>
    /// <param name="damageLayer"></param>
    public void ProccessDamage(float damage, bool showDamageText, LayerMask damageLayer)
    {
        if (GeneralFunctions.IsObjectOnLayer(damageLayer, gameObject))
        {
            if (!isDead)
            {
                currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);

                if (showDamageText)
                {
                    DamageText.CreateDamageText(damage, transform.position, gameplayManager.combatTextSpeed, gameplayManager.combatTextUpTime, gameplayManager.combatRandomVectorMinX, 
                        gameplayManager.combatRandomVectorMaxX, gameplayManager.combatRandomVectorMinY, gameplayManager.combatRandomVectorMaxY, gameplayManager.dissapearTime);
                }

                onTakeAnyDamage.Invoke(damage);

                if (IsOnPlayer())
                {
                    UpdatePlayerState();
                }

                if (healthBar)
                {
                    healthBar.SetHealth(currentHealth);
                }

                if (currentHealth <= 0)
                {
                    isDead = true;
                    OnDeath.Invoke();
                }
            }
        }
    }
    /// <summary>
    /// Set current health and to the given value
    /// </summary>
    /// <param name="value"></param>
    public void SetHealth(float value)
    {
        currentHealth = value;
        maxHealth = value;

        if (healthBar)
        {
            healthBar.SetHealth(currentHealth);
        }

        if (IsOnPlayer())
        {
            UpdatePlayerState();
        }
    }
    /// <summary>
    /// Set current health and to the given values
    /// </summary>
    /// <param name="currentHP"></param>
    /// <param name="maxHP"></param>
    public void SetHealth(float currentHP, float maxHP)
    {
        currentHealth = currentHP;
        maxHealth = maxHP;

        if (IsOnPlayer())
        {
            UpdatePlayerState();
        }

        if (healthBar)
        {
            healthBar.SetHealth(currentHealth);
        }
    }
    /// <summary>
    /// Get this Gameobjects current health
    /// </summary>
    public float GetCurrentHealth()
    {
        return currentHealth;
    }
    /// <summary>
    /// Check to see if the current Gameobject is dead
    /// </summary>
    /// <returns></returns>
    public bool GetIsDead()
    {
        return isDead;
    }
    /// <summary>
    /// Find the player state in the level then construct the health component
    /// </summary>
    public void FindPlayerState()
    {
        playerState = FindObjectOfType<PlayerState>();

        ConstructHealthComponent();
    }
    /// <summary>
    /// Update player state health values
    /// </summary>
    private void UpdatePlayerState()
    {
        if (playerState)
        {
            playerState.UpdatePlayerStateHP(currentHealth, maxHealth);
        }
    }
    /// <summary>
    /// Checks to see if this health component is on the player game object
    /// </summary>
    private bool IsOnPlayer()
    {
        var player = gameObject.GetComponentInParent<PlayerController>();

        return player;
    }
}