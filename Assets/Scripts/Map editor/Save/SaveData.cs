using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using WEditor.UI;
namespace WEditor
{
    public static class SaveData
    {
        private static string tailPath = ".wEditor";
        private static string tailPath2 = ".wEditor.paths";
        public static void SaveToLocal(string levelName,Vector3Int levelSpawn)
        {
            string path = Application.persistentDataPath + $"/{levelName}{tailPath}";
            if (File.Exists(path))
            {
                TextMessageHandler.instance.SetError("ln_ep");
                return;
            }

            string pathContainer = Application.persistentDataPath + $"/{tailPath2}";

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Create);

            if (PlayerPrefs.HasKey("levelNames"))
            {
                string levels = PlayerPrefs.GetString("levelNames");
                levels = $"{levels}_{levelName}";
                PlayerPrefs.SetString("levelNames", levels);
            }
            else
            {
                PlayerPrefs.SetString("levelNames", levelName);
            }

            GameData data = new GameData(levelName,levelSpawn);
            formatter.Serialize(stream, data);
            stream.Close();
        }
        public static GameData LoadLocalLevel(string levelName)
        {

            string path = Application.persistentDataPath + $"/{levelName}{tailPath}";

            if (!File.Exists(path))
            {
                return null;
            }

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            GameData data = formatter.Deserialize(stream) as GameData;
            stream.Close();
            return data;
        }
    }
}