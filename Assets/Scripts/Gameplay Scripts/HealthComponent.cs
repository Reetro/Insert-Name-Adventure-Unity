using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class HealthComponent : MonoBehaviour
{
    [System.Serializable]
    public class TakeAnyDamge : UnityEvent<float> { }

    [Header("Health Settings")]
    [SerializeField] float maxHealth = 10f;

    [Header("Health Bar Settings")]
    [SerializeField] HealthBar healthBar = null;

    [Header("Combat Text Settings")]
    [SerializeField] [Range(0.01f, 1f)] float combatTextSpeed = 0.01f;
    [SerializeField] float combatTextUpTime = 0.5f;
    [SerializeField] float combatRandomVectorMinX = -0.5f;
    [SerializeField] float combatRandomVectorMaxX = 1f;
    [SerializeField] float combatRandomVectorMinY = -0.5f;
    [SerializeField] float combatRandomVectorMaxY = 1f;

    [Header("Events")]
    [Space]
    public UnityEvent OnDeath;
    [Space]
    public TakeAnyDamge onTakeAnyDamage;

    private float currentHealth = 0f;
    private bool isDead = false;
    private PlayerState playerState = null;

    private static bool setMaxhealth = false;

    public void ConstructHealthComponent()
    {
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

    public void ProccessDamage(float damage)
    {
        if (!isDead)
        {
            currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);

            DamageText.CreateDamageText(damage, transform.position, combatTextSpeed, combatTextUpTime, combatRandomVectorMinX, combatRandomVectorMaxX, combatRandomVectorMinY, combatRandomVectorMaxY);

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
