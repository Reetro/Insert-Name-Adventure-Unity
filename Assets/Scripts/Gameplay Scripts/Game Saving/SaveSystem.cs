using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System;

namespace PlayerCharacter.GameSaving
{
    public static class SaveSystem
    {
        /// <summary>
        /// File path for where to place the save game file
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
        public static void SaveGame(PlayerState state, GameObject player)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(SaveFilePath, FileMode.Create);

            SaveData saveData = new SaveData(state, player);

            formatter.Serialize(stream, saveData);

            stream.Close();
        }
        /// <summary>
        /// Loads saved data from the given path
        /// </summary>
        /// <returns>The saved game data</returns>
        public static SaveData LoadGame()
        {
            if (File.Exists(SaveFilePath))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(SaveFilePath, FileMode.Open);

                SaveData data = formatter.Deserialize(stream) as SaveData;

                stream.Close();

                return data;
            }
            else
            {
                Debug.LogError("Save file not found in " + SaveFilePath);
                return null;
            }
        }
        /// <summary>
        /// Delete the saved game file
        /// </summary>
        public static void DeleteSaveGame()
        {
            try
            {
                File.Delete(SaveFilePath);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }
}
