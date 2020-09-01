using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

namespace PlayerCharacter.GameSaving
{
    public static class SaveSystem
    {
        /// <summary>
        /// Saves all player game data
        /// </summary>
        /// <param name="state"></param>
        /// <param name="player"></param>
        public static void SaveGame(PlayerState state, GameObject player)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/player.character";
            FileStream stream = new FileStream(path, FileMode.Create);

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
            string path = Application.persistentDataPath + "/player.character";

            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                SaveData data = formatter.Deserialize(stream) as SaveData;

                stream.Close();

                return data;
            }
            else
            {
                Debug.LogError("Save file not found in " + path);
                return null;
            }
        }
    }
}
