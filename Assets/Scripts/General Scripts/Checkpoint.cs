using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    public float healAmount = 3f;

    private PlayerState playerState = null;
    private int currentLevelIndex = 0;

    public void ConstructCheckpoint()
    {
        playerState = FindObjectOfType<PlayerState>();
        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerState.SetCheckpointIndex(currentLevelIndex);

            GeneralFunctions.HealTarget(collision.gameObject, healAmount);
        }
    }
}
