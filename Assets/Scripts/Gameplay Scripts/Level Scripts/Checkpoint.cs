using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    [Tooltip("Amount to heal the player")]
    public float healAmount = 3f;
    [Tooltip("The amount of health the player has to have or below to be healed")]
    public float healThreshold = 8f;

    private PlayerState playerState = null;
    private int currentLevelIndex = 0;
    private bool healedPlayer = false;

    public void ConstructCheckpoint()
    {
        playerState = FindObjectOfType<PlayerState>();
        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;

        healedPlayer = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!healedPlayer)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (!GeneralFunctions.IsObjectDead(collision.gameObject))
                {
                    playerState.SetCheckpointIndex(currentLevelIndex);

                    if (GeneralFunctions.GetGameObjectHealthComponent(GeneralFunctions.GetPlayerGameObject()).CurrentHealth < healThreshold)
                    {
                        GeneralFunctions.HealTarget(collision.gameObject, healAmount);

                        healedPlayer = true;
                    }
                }
            }
        }
    }
}
