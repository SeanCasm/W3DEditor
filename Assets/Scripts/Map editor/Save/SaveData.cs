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
        private static string persistentDataPath = Application.persistentDataPath + "/";

        public static void SaveToLocal(string levelName, Vector3Int levelSpawn, (int w, int h) size)
        {
            string path = persistentDataPath + $"{levelName}{tailPath}";
            if (File.Exists(path))
            {
                TextMessageHandler.instance.SetError("ln_ep");
                return;
            }

            string pathContainer = persistentDataPath + $"{tailPath2}";

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Create);

            GameData data = new GameData(levelName, levelSpawn, size, DataHandler.levelTiles);
            formatter.Serialize(stream, data);
            stream.Close();
            TextMessageHandler.instance.SetMessage("ss_cc");
        }
        public static GameData[] LoadLocalLevels()
        {

            string[] levelPaths = Directory.GetFiles(persistentDataPath);
            GameData[] gameDatas = new GameData[levelPaths.Length];
            for (int i = 0; i < levelPaths.Length; i++)
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(levelPaths[i], FileMode.Open);
                GameData data = formatter.Deserialize(stream) as GameData;
                gameDatas[i] = data;
                stream.Close();
            }
            return gameDatas;
        }
    }
}