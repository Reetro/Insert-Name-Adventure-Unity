using UnityEngine;

namespace GeneralScripts.GeneralComponents
{
    public class GameplayObjectID : MonoBehaviour
    {
        public int ID { get; private set; }
        /// <summary>
        /// Generate this Gameobjects ID
        /// </summary>
        public void ConstructID()
        {
            ID = GeneralFunctions.GenID();
        }

    }
}