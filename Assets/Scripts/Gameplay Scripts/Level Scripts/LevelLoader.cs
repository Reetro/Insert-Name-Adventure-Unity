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

        /// <summary>
        /// Will load the scene that corresponds to the given index
        /// </summary>
        /// <param name="index"></param>
        public void LoadLevelAtIndex(int levelIndex)
        {
            StartCoroutine(LoadLevel(levelIndex));
        }
        /// <summary>
        /// Will reset player health and load the current player checkpoint
        /// </summary>
        public void LoadCheckpoint()
        {
            playerState.ResetHealthToMax();
            StartCoroutine(LoadLevel(playerState.CheckpointLevelIndex));
        }
        /// <summary>
        /// Loads the next level in the build settings
        /// </summary>
        public void LoadNextLevel()
        {
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        }
        /// <summary>
        /// Load level after level transition animation is done
        /// </summary>
        /// <param name="levelIndex"></param>
        private IEnumerator LoadLevel(int levelIndex)
        {
            transition.SetTrigger("Start");

            playerState.SetSceneLoading(true);

            yield return new WaitForSeconds(transitionTime);

            bool goBackToStart = levelIndex >= SceneManager.sceneCountInBuildSettings;

            if (!goBackToStart)
            {
                SceneManager.LoadScene(levelIndex);
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}