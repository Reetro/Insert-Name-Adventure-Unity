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

    public void ProccessDamage(float damage, bool showDamageText, LayerMask damageLayer)
    {
        if (GeneralFunctions.IsObjectOnLayer(damageLayer, gameObject))
        {
            if (!isDead)
            {
                currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);

                if (showDamageText)
                {
                    DamageText.CreateDamageText(damage, transform.position, gameplayManager.combatTextSpeed, gameplayManager.combatTextUpTime, gameplayManager.combatRandomVectorMinX, gameplayManager.combatRandomVectorMaxX, gameplayManager.combatRandomVectorMinY, gameplayManager.combatRandomVectorMaxY);
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

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public bool GetIsDead()
    {
        return isDead;
    }

    public void FindPlayerState()
    {
        playerState = FindObjectOfType<PlayerState>();

        ConstructHealthComponent();
    }

    private void UpdatePlayerState()
    {
        if (playerState)
        {
            playerState.UpdatePlayerStateHP(currentHealth, maxHealth);
        }
    }

    private bool IsOnPlayer()
    {
        var player = gameObject.GetComponentInParent<PlayerController>();

        return player;
    }
}
