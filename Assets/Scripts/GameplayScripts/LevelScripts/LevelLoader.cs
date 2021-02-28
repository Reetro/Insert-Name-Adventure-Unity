using System.Collections;
using GeneralScripts;
using PlayerScripts.PlayerControls;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameplayScripts.LevelScripts
{
    public class LevelLoader : MonoBehaviour
    {
        public Animator transition;
        public float transitionTime = 1f;

        public PlayerState playerState;
        private bool runSceneTimer;
        private float sceneTimer;
        private static readonly int Start = Animator.StringToHash("Start");

        private void Awake()
        {
            sceneTimer = transitionTime;
        }

        /// <summary>
        /// Will load the scene that corresponds to the given index
        /// </summary>
        /// <param name="levelIndex"></param>
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
            StartCoroutine(LoadLevel(PlayerState.CheckpointLevelIndex));
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
            transition.SetTrigger(Start);

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
        /// <summary>
        /// Bind scene loaded to OnLevelFinishedLoading
        /// </summary>
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }
        /// <summary>
        /// Unbind scene loaded to OnLevelFinishedLoading
        /// </summary>
        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        }
        /// <summary>
        /// Called when scene has finished loading in will start a timer that ends when the transition animation has ended
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="mode"></param>
        void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            runSceneTimer = true;
        }
        /// <summary>
        /// Counts down the time until the transition animation has ended
        /// </summary>
        private void Update()
        {
            if (runSceneTimer)
            {
                sceneTimer -= Time.deltaTime;

                if (sceneTimer <= 0)
                {
                    runSceneTimer = false;

                    sceneTimer = transitionTime;

                    GeneralFunctions.GetGameplayManager().onSceneLoadingDone.Invoke();
                }
            }
        }
    }
}