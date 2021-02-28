using GameplayScripts.LevelScripts;
using GeneralScripts.GeneralComponents;
using GeneralScripts.UI;
using PlayerScripts.PlayerControls;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameplayScripts
{
    public class GameAssets : MonoBehaviour
    {
        #region Properites
        /// <summary>
        /// A reference to the current GameAsset in the world
        /// </summary>
        // ReSharper disable once InconsistentNaming
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
        public static PlayerUIManager PlayerHudManager { get; private set; }
        /// <summary>
        /// A reference to the current PlayerState in the world
        /// </summary>
        public static PlayerState PlayerCurrentState { get; private set; }
        /// <summary>
        /// A reference to the current LevelLoader in the world
        /// </summary>
        public static LevelLoader CurrentLevelLoader { get; private set; }

        /// <summary>
        /// A reference to the Player's Leg component currently in the world
        /// </summary>
        public static PlayerLegs PlayerObjectLegs { get; private set; }
        /// <summary>
        ///  A reference to the CameraShake component on the virtual camera
        /// </summary>
        public static CameraShake CameraShakeComponent { get; private set; }

        [FormerlySerializedAs("damgeText")] public Transform damageText;
        #endregion

        #region Functions
        /// <summary>
        /// Called when a new scene is loaded will update internal asset references
        /// </summary>
        public static void UpdateReferences()
        {
            GlobalManager = FindObjectOfType<GameplayManager>();

            PlayerGameObject = GameObject.FindGameObjectWithTag("Player");

            PlayerGameObject.GetComponent<PlayerController>();

            PlayerObjectLegs = PlayerGameObject.transform.GetChild(0).GetComponent<PlayerLegs>();

            PlayerHudManager = FindObjectOfType<PlayerUIManager>();

            PlayerCurrentState = FindObjectOfType<PlayerState>();

            CurrentLevelLoader = FindObjectOfType<LevelLoader>();

            CameraShakeComponent = FindObjectOfType<CameraShake>();
        }
        /// <summary>
        /// Gets the current instances of this object in the world
        /// </summary>
        public static GameAssets Instance
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