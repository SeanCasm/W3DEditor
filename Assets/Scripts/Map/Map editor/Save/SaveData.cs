using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using WEditor.UI;
namespace WEditor
{
    public static class SaveData
    {
        public static string persistentDataPath = Application.persistentDataPath + "/Levels/";

        public static void SaveToLocal()
        {
            string path = $"{persistentDataPath}{DataHandler.currentLevelName}.json";
            if (File.Exists(path))
            {
                MessageHandler.instance.SetPopUpMessage("level_exist", DataHandler.currentLevelName, () => { save(); });
                return;
            }
            save();

            void save()
            {

                GameData data = new();
                data.SetData();
                string json = JsonUtility.ToJson(data);
                File.WriteAllText(path, json);
                MessageHandler.instance.SetMessage("save");
            }
        }
        public static GameData[] LoadLocalLevels()
        {
            string[] levelPaths = Directory.GetFiles(persistentDataPath);
            GameData[] gameDatas = new GameData[levelPaths.Length];

            for (int i = 0; i < levelPaths.Length; i++)
                gameDatas[i] = JsonUtility.FromJson<GameData>(File.ReadAllText(levelPaths[i]));

            return gameDatas;
        }
    }
}