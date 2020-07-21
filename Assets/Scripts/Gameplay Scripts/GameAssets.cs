using UnityEngine;

namespace GameplayManagement.Assets
{
    public class GameAssets : MonoBehaviour
    {
        private static GameAssets _Instance;

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