using LevelObjects.SceneLoading;
using PlayerCharacter.GameSaving;
using PlayerUI;
using UnityEngine;

namespace GameplayManagement.Assets
{
    public class GameAssets : MonoBehaviour
    {
        #region Properites
        /// <summary>
        /// A reference to the current GameAsset in the world
        /// </summary>
        private static GameAssets _Instance;
        /// <summary>
        /// A reference to the current GameplayManager in the world
        /// </summary>
        public static GameplayManager GlobalManager { get; private set; }
        /// <summary>
        /// A reference to the current Player Gameobject in the world
        /// </summary>
        public static GameObject PlayerGameObject { get; private set; }
        /// <summary>
        /// A reference to the current PlayerUIManager in the world
        /// </summary>
        public static PlayerUIManager PlayerHUDManager { get; private set; }
        /// <summary>
        /// A reference to the current PlayerState in the world
        /// </summary>
        public static PlayerState PlayerCurrentState { get; private set; }
        /// <summary>
        /// A reference to the current LevelLoader in the world
        /// </summary>
        public static LevelLoader CurrentLevelLoader { get; private set; }

        public Transform damgeText;
        #endregion

        #region Functions
        /// <summary>
        /// Called when a new scene is loaded will update internal asset references
        /// </summary>
        public void UpdateReferences()
        {
            GlobalManager = FindObjectOfType<GameplayManager>();

            PlayerGameObject = GameObject.FindGameObjectWithTag("Player");

            PlayerHUDManager = FindObjectOfType<PlayerUIManager>();

            PlayerCurrentState = FindObjectOfType<PlayerState>();

            CurrentLevelLoader = FindObjectOfType<LevelLoader>();
        }
        /// <summary>
        /// Gets the current instances of this object in the world
        /// </summary>
        public static GameAssets instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = Instantiate(Resources.Load<GameAssets>("GameAssetLoader")).GetComponent<GameAssets>();
                }
                return _Instance;
            }
        }
        #endregion
    }
}