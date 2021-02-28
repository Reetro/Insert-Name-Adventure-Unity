namespace GameplayScripts.GameSaving
{
    [System.Serializable]
    public class SaveSlotData
    {
        #region Properties
        /// <summary>
        /// Slot the game is saved to
        /// </summary>
        public int Slot { get; private set; }
        /// <summary>
        /// Checks to see if this is the active game slot
        /// </summary>
        public bool IsSlotActive { get; private set; }
        #endregion

        public SaveSlotData (int slot, bool isSlotActive)
        {
            Slot = slot;
            IsSlotActive = isSlotActive;
        }
    }
}
