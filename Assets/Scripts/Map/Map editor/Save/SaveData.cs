using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using WEditor.UI;
namespace WEditor
{
    public static class SaveData
    {
        private static string documentPath => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private static string editorFolder => Path.Combine(documentPath, Application.companyName, Application.productName);
        public static string LevelFolder(string levelName) => Path.Combine(editorFolder, levelName);
        public static void SaveToLocal()
        {
            if (!Directory.Exists(editorFolder))
                Directory.CreateDirectory(editorFolder);
            string path = Path.Combine(documentPath, editorFolder, DataHandler.currentLevelName);
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
            string[] levelPaths = Directory.GetFiles(Path.Combine(documentPath, editorFolder));
            GameData[] gameDatas = new GameData[levelPaths.Length];

            for (int i = 0; i < levelPaths.Length; i++)
                gameDatas[i] = JsonUtility.FromJson<GameData>(File.ReadAllText(levelPaths[i]));

            return gameDatas;
        }
    }
}