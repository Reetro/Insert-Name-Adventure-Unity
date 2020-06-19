using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class HealthComponent : MonoBehaviour
{
    [System.Serializable]
    public class TakeAnyDamge : UnityEvent<float> { }

    [Header("Health Settings")]
    [SerializeField] float maxHealth = 10f;
    [SerializeField] float combatTextSpeed = 4f;
    [SerializeField] float combatTextUpTime = 0.5f;

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

    private static bool setMaxhealth = false;

    void Start()
    {
        isDead = false;

        if (IsOnPlayer())
        {
            playerState = FindObjectOfType<PlayerState>();

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

            DamageText.CreateDamageText(damage, transform.localPosition, combatTextSpeed, combatTextUpTime);

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
