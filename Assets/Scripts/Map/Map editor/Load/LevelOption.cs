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
            PlayerPrefs.DeleteKey($"{currLevelSelected}-Score");
            PlayerPrefs.DeleteKey($"{currLevelSelected}-Kills");
            PlayerPrefs.DeleteKey($"{currLevelSelected}-Teasures");
            File.Delete($"{SaveData.LevelFolder(currLevelSelected)}.json");
            MessageHandler.instance.SetMessage("level_del_done");
            Destroy(gameObject);
        }

    }
}