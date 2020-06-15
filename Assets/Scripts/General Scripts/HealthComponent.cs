using UnityEngine;
using UnityEngine.Events;

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

    void Start()
    {
        isDead = false;
        currentHealth = maxHealth;

        if (healthBar)
        {
            healthBar.SetMaxHealth(maxHealth);
        }
    }

    public void AddHealth(float amountToAdd)
    {
        currentHealth = Mathf.Clamp(currentHealth + amountToAdd, 0, maxHealth);

        if (healthBar)
        {
            healthBar.SetHealth(currentHealth);
        }
    }

    public void ProccessDamage(float damage)
    {
        if (!isDead)
        {
            currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);

            DamageText.CreateDamageText(damage, transform.localPosition, combatTextSpeed, combatTextUpTime);

            onTakeAnyDamage.Invoke(damage);

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

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public bool GetIsDead()
    {
        return isDead;
    }
}
