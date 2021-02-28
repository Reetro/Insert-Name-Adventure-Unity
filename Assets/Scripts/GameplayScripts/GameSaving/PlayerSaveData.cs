using PlayerScripts.PlayerControls;
using UnityEngine;

namespace GameplayScripts.GameSaving
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
            CurrentHealth = PlayerState.CurrentHealth;
            MaxHealth = PlayerState.MaxHealth;
            CheckpointLevelIndex = PlayerState.CheckpointLevelIndex;
            CurrentLevelIndex = state.CurrentLevelIndex;

            PlayerPosition = new float[3];
            var position = player.transform.position;
            PlayerPosition[0] = position.x;
            PlayerPosition[1] = position.y;
            PlayerPosition[2] = position.z;
        }
    }
}