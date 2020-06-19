using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerState : MonoBehaviour
{
    public static float currentHealth = 0f;
    public static float maxHealth = 0f;
    public static int checkpointIndex = 0;

    public PlayerController player = null;
    public HealthBar playerHPBar = null;

    private void Awake()
    {
        int playerStateCount = FindObjectsOfType<PlayerState>().Length;

        if (playerStateCount > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SetCheckpointIndex(int index)
    {
        checkpointIndex = index;
    }

    public int GetCheckpointIndex()
    {
        return checkpointIndex;
    }

    public void SetPlayerHealth()
    {
        player = FindObjectOfType<PlayerController>();

        player.GetComponent<HealthComponent>().SetHealth(currentHealth, maxHealth);

        playerHPBar.SetHealth(currentHealth);
    }

    public float GetCurrentMaxHealth()
    {
        return maxHealth;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public void ResetHealthToMax()
    {
        currentHealth = maxHealth;
    }

    public void UpdatePlayerStateHP(float currentHP, float maxHP)
    {
        currentHealth = currentHP;
        maxHealth = maxHP;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        SetPlayerHealth();
    }
}
