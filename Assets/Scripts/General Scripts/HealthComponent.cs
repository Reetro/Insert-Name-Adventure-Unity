using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] float maxHealth = 10f;

    [Header("Health Bar Settings")]
    [SerializeField] HealthBar healthBar = null;

    [Header("Events")]
    [Space]
    public UnityEvent OnDeath;

    private float currentHealth = 0f;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar)
        {
            healthBar.SetMaxHealth(maxHealth);
        }
    }

    public void AddHealth(float amountToAdd)
    {
        currentHealth = Mathf.Clamp(currentHealth + amountToAdd, 0, maxHealth);
    }

    public void ProccessDamage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);

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

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public bool GetIsDead()
    {
        return isDead;
    }
}
