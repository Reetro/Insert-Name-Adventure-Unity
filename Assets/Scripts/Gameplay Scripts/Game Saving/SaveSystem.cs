using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System;

namespace PlayerCharacter.GameSaving
{
    public static class SaveSystem
    {
        /// <summary>
        /// File path for where to place the save game files
        /// </summary>
        private static string SaveFilePath
        {
            get { return Application.persistentDataPath + "/player.character"; }
        }
        /// <summary>
        /// Saves all player game data
        /// </summary>
        /// <param name="state"></param>
        /// <param name="player"></param>
        public static void SaveGame(PlayerState state, GameObject player, int slot)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(SaveFilePath + slot, FileMode.Create);

            SaveData saveData = new SaveData(state, player);

            formatter.Serialize(stream, saveData);

            stream.Close();
        }
        /// <summary>
        /// Loads saved data from the given path
        /// </summary>
        /// <returns>The saved game data</returns>
        public static SaveData LoadGame(int slot)
        {
            if (DoesSaveGameExistInSlot(slot))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(SaveFilePath + slot, FileMode.Open);

                SaveData data = formatter.Deserialize(stream) as SaveData;

                stream.Close();

                return data;
            }
            else
            {
                Debug.LogError("Save file not found in " + SaveFilePath + slot);
                return null;
            }
        }
        /// <summary>
        /// Delete the saved game file in the given slot
        /// </summary>
        public static void DeleteSaveGame(int slot)
        {
            try
            {
                File.Delete(SaveFilePath + slot);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
        /// <summary>
        /// Check to see if a saved game exist in a given slot
        /// </summary>
        /// <param name="slot"></param>
        public static bool DoesSaveGameExistInSlot(int slot)
        {
            var path = SaveFilePath + slot;

            return File.Exists(path) ? true : false;
        }
    }
}
