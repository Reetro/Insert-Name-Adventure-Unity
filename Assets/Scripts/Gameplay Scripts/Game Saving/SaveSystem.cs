using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using static PlayerCharacter.GameSaving.PlayerState;

namespace PlayerCharacter.GameSaving
{
    public static class SaveSystem
    {
        #region File Paths
        /// <summary>
        /// File path for where to place player character save game files
        /// </summary>
        private static string PlayerSaveFilePath(int slot)
        {
            var foldername = "Slot " + slot.ToString();
            var filesavename = "PlayerCharacter";
            var fileextnison = ".save";

            return Application.persistentDataPath + "/" + foldername + "/" + filesavename + fileextnison;
        }
        /// <summary>
        /// File path for where to save slot data
        /// </summary>
        private static string SaveSlotFilePath(int slot)
        {
            var foldername = "Slot " + slot.ToString();
            var filesavename = "Slot" + slot.ToString();
            var fileextnison = ".save";

            return Application.persistentDataPath + "/" + foldername + "/" + filesavename + fileextnison;
        }
        /// <summary>
        /// File path for where to create the save folder
        /// </summary>
        private static string SaveFolderFilePath(ref SavedGameSlot SaveSlot)
        {
            return Path.Combine(Application.persistentDataPath, "Slot " + SaveSlot.slot.ToString());
        }
        #endregion

        #region Save Functions
        /// <summary>
        /// Saves all player game data
        /// </summary>
        /// <param name="state"></param>
        /// <param name="player"></param>
        public static void SaveGame(PlayerState state, GameObject player, SavedGameSlot saveSlot)
        {
            SaveGameSlot(ref saveSlot);
            SavePlayerData(ref saveSlot, state, player);
        }
        /// <summary>
        /// Saves all player data
        /// </summary>
        /// <param name="SaveSlot"></param>
        /// <param name="state"></param>
        /// <param name="player"></param>
        public static void SavePlayerData(ref SavedGameSlot SaveSlot, PlayerState state, GameObject player)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            if (!Directory.Exists(SaveFolderFilePath(ref SaveSlot)))
            {
                Directory.CreateDirectory(SaveFolderFilePath(ref SaveSlot));
            }

            FileStream stream = new FileStream(PlayerSaveFilePath(SaveSlot.slot), FileMode.Create);

            PlayerSaveData playerSaveData = new PlayerSaveData(state, player);

            formatter.Serialize(stream, playerSaveData);

            stream.Close();
        }
        /// <summary>
        /// Save the given slot data
        /// </summary>
        /// <param name="SaveSlot"></param>
        public static void SaveGameSlot(ref SavedGameSlot SaveSlot)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            if (!Directory.Exists(SaveFolderFilePath(ref SaveSlot)))
            {
                Directory.CreateDirectory(SaveFolderFilePath(ref SaveSlot));
            }

            FileStream stream = new FileStream(SaveSlotFilePath(SaveSlot.slot), FileMode.Create);

            SaveSlotData saveSlotData = new SaveSlotData(SaveSlot.slot, SaveSlot.isActive);

            formatter.Serialize(stream, saveSlotData);

            stream.Close();
        }
        #endregion

        #region Load Functions
        /// <summary>
        /// Loads saved player data from the given path
        /// </summary>
        /// <returns>The saved player data</returns>
        public static PlayerSaveData LoadPlayerFromSlot(int slot)
        {
            if (DoesPlayerSaveGameExistInSlot(slot))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(PlayerSaveFilePath(slot), FileMode.Open);

                PlayerSaveData data = formatter.Deserialize(stream) as PlayerSaveData;

                stream.Close();

                return data;
            }
            else
            {
                Debug.LogError("Save file not found in " + PlayerSaveFilePath(slot));
                return null;
            }
        }
        /// <summary>
        /// Loads
        /// </summary>
        /// <param name="slot"></param>
        /// <returns>The save slot data</returns>
        public static SaveSlotData LoadSaveSlot(int slot)
        {
            if (File.Exists(SaveSlotFilePath(slot)))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(SaveSlotFilePath(slot), FileMode.Open);

                SaveSlotData data = formatter.Deserialize(stream) as SaveSlotData;

                stream.Close();

                return data;
            }
            else
            {
                Debug.LogError("Save file not found in " + SaveSlotFilePath(slot));
                return null;
            }
        }
        #endregion

        #region Save Management Functions
        /// <summary>
        /// Delete the saved game file in the given slot
        /// </summary>
        public static void DeleteSaveGame(int slot)
        {
            try
            {
                File.Delete(PlayerSaveFilePath(slot));
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }

            try
            {
                File.Delete(SaveSlotFilePath(slot));
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
        /// <summary>
        /// Checks to see if both a player save and save slot files exit
        /// </summary>
        /// <param name="slot"></param>
        public static bool DoesSaveGameExistInSlot(int slot)
        {
            var player = DoesPlayerSaveGameExistInSlot(slot);
            var saveSlot = DoesSaveSlotExist(slot);

            return player && saveSlot;
        }
        /// <summary>
        /// Check to see if a saved player exist in a given slot
        /// </summary>
        /// <param name="slot"></param>
        public static bool DoesPlayerSaveGameExistInSlot(int slot)
        {
            var path = PlayerSaveFilePath(slot);

            return File.Exists(path) ? true : false;
        }
        /// <summary>
        /// Checks to see if a saved slot exist in the given slot
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        public static bool DoesSaveSlotExist(int slot)
        {
            var path = SaveSlotFilePath(slot);

            return File.Exists(path) ? true : false;
        }
        #endregion
    }
}
