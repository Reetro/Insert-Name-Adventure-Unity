using PlayerCharacter.Controller;
using UnityEngine;

namespace PlayerCharacter.GameSaving
{
    [System.Serializable]
    public class PlayerSaveData
    {
        #region Data To Save
        /// <summary>
        /// Saved health amount
        /// </summary>
        public float CurrentHealth { get; private set; }
        /// <summary>
        /// Saved max player health
        /// </summary>
        public float MaxHealth { get; private set; }
        /// <summary>
        /// Saved level current checkpoint level index
        /// </summary>
        public int CheckpointLevelIndex { get; private set; }
        /// <summary>
        /// Saved player location
        /// </summary>
        public float[] PlayerPosition { get; private set; }
        /// <summary>
        /// Saved current level the player is in
        /// </summary>
        public int CurrentLevelIndex { get; private set; }
        #endregion

        /// <summary>
        /// Get all data to save from player state and player Gameobject
        /// </summary>
        public PlayerSaveData (PlayerState state, GameObject player)
        {
            CurrentHealth = state.CurrentHealth;
            MaxHealth = state.MaxHealth;
            CheckpointLevelIndex = state.CheckpointLevelIndex;
            CurrentLevelIndex = state.CurrentLevelIndex;

            PlayerPosition = new float[3];
            PlayerPosition[0] = player.transform.position.x;
            PlayerPosition[1] = player.transform.position.y;
            PlayerPosition[2] = player.transform.position.z;
        }
    }
}