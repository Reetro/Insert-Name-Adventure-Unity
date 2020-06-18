using UnityEngine;

public class LevelExit : MonoBehaviour
{
    private LevelLoader levelLoader = null;

    private void Start()
    {
        levelLoader = GameObject.FindGameObjectWithTag("Level Loader").GetComponent<LevelLoader>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            levelLoader.LoadNextLevel();
        }
    }
}
