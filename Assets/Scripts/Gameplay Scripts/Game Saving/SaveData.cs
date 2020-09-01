using PlayerCharacter.Controller;
using UnityEngine;

namespace PlayerCharacter.GameSaving
{
    [System.Serializable]
    public class SaveData
    {
        #region Data To Save
        public float currentHealth = 0f;
        public float maxHealth = 0f;
        public int levelIndex = 0;
        public float[] position;
        #endregion

        /// <summary>
        /// Get all data to save from player state
        /// </summary>
        /// <param name="state"></param>
        public SaveData (PlayerState state, GameObject player)
        {
            currentHealth = state.CurrentHealth;
            maxHealth = state.MaxHealth;
            levelIndex = state.LevelIndex;

            position = new float[3];
            position[0] = player.transform.position.x;
            position[1] = player.transform.position.y;
            position[2] = player.transform.position.z;
        }
    }
}