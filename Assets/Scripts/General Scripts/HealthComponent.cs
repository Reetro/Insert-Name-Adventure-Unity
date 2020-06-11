using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] float MaxHealth = 10f;

    [Header("Events")]
    [Space]
    public UnityEvent OnDeath;

    private float currentHealth = 0f;
    private bool isDead = false;

    void Start()
    {
        currentHealth = MaxHealth;
    }

    public void AddHealth(float amountToAdd)
    {
        currentHealth = Mathf.Clamp(currentHealth + amountToAdd, 0, MaxHealth);
    }

    public void ProccessDamage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, MaxHealth);

        Debug.Log(currentHealth);

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
