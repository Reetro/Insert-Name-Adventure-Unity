﻿using System.Collections;
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
        public void LoadSavedLevel(int levelIndex)
        {
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
        /// <summary>
        /// Will reset player health and load the current player checkpoint
        /// </summary>
        public void LoadCheckpoint()
        {
            playerState.ResetHealthToMax();
            StartCoroutine(LoadLevel(playerState.LevelIndex));
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