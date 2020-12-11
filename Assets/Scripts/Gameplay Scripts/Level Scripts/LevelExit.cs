using UnityEngine;
using GameplayManagement.Assets;

namespace LevelObjects.SceneLoading
{
    public class LevelExit : MonoBehaviour
    {
        private LevelLoader levelLoader = null;

        public void ConsturctExit(LevelLoader levelLoader)
        {
            this.levelLoader = levelLoader;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (GeneralFunctions.IsObjectPlayer(collision.gameObject))
            {
                if (!GeneralFunctions.IsObjectDead(collision.gameObject))
                {
                    GameAssets.GlobalManager.onLevelExitOverlap.Invoke();

                    levelLoader.LoadNextLevel();
                }
            }
        }
    }
}