using UnityEngine;

namespace GameplayManagement.Assets
{
    public class GameAssets : MonoBehaviour
    {
        private static GameAssets _Instance;
        /// <summary>
        /// A reference to the current gameplay manager in the world
        /// </summary>
        public static GameplayManager GlobalManager { get; private set; }

        private void Awake()
        {
            GlobalManager = GameObject.FindGameObjectWithTag("Gameplay Manager").GetComponent<GameplayManager>();
        }

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

        public Transform damgeText;
    }
}