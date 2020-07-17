using UnityEngine;
using System.Collections.Generic;

namespace GameplayManagement
{
    public class GameplayManager : MonoBehaviour
    {
        [Header("Combat Text Settings")]
        [Range(0.01f, 1f)] public float combatTextSpeed = 0.01f;
        public float combatTextUpTime = 0.5f;
        public float combatRandomVectorMinX = -0.5f;
        public float combatRandomVectorMaxX = 1f;
        public float combatRandomVectorMinY = -0.5f;
        public float combatRandomVectorMaxY = 1f;
        public float dissapearTime = 3f;

        [Header("Tooltip Settings")]
        public float nameFontSize = 36f;
        public float descriptionFontSize = 28f;

        [Space]

        [Header("Damage Settings")]
        public LayerMask whatCanBeDamaged;

        public int GenID()
        {
            var newID = Random.Range(1, 1000000);

            for (int index = 0; index < gameIDS.Count; index++)
            {
                if (gameIDS.Contains(newID))
                {
                    newID = Random.Range(1, 1000000);
                    break;
                }
            }

            gameIDS.Add(newID);
            return newID;
        }

        public List<int> gameIDS { get; } = new List<int>();
    }
}