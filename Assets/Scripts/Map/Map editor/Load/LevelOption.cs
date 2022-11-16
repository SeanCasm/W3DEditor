using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
namespace WEditor.UI
{
    public class LevelOption : MonoBehaviour
    {
        public string currLevelSelected { get; set; }
        public GameData gameData { get; set; }
        public void DeleteLevel()
        {
            File.Delete($"{SaveData.persistentDataPath}{currLevelSelected}.json");
            print(SaveData.persistentDataPath + currLevelSelected);
            MessageHandler.instance.SetMessage("level_del_done");
            Destroy(gameObject);
        }

    }
}