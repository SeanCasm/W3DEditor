using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace WEditor
{
    public static class SaveData
    {

        public static void SaveGroundWallMap(int slotIndex)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + GetSlotPath(slotIndex);
            FileStream stream = new FileStream(path, FileMode.Create);
            GameData data = new GameData();
            formatter.Serialize(stream, data);
            stream.Close();
        }
        private static string GetSlotPath(int slotIndex)
        {
            string path = "/scenario.xd" + slotIndex;
            return path;
        }
    }
}
