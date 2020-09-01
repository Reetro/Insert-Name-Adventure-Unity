using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using PlayerCharacter.GameSaving;

namespace LevelObjects.SceneLoading
{
    public class LevelLoader : MonoBehaviour
    {
        public Animator transition;
        public float transitionTime = 1f;

        public PlayerState playerState = null;

        public void LoadCheckpoint()
        {
            playerState.ResetHealthToMax();
            StartCoroutine(LoadLevel(playerState.LevelIndex));
        }

        public void LoadNextLevel()
        {
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        }

        IEnumerator LoadLevel(int levelIndex)
        {
            transition.SetTrigger("Start");

            yield return new WaitForSeconds(transitionTime);

            bool goBackToStart = levelIndex >= SceneManager.sceneCountInBuildSettings;

            if (!goBackToStart)
            {
                playerState.SetSceneLoading(true);

                SceneManager.LoadScene(levelIndex);
            }
            else
            {
                playerState.SetSceneLoading(true);

                SceneManager.LoadScene(0);
            }
        }
    }
}