using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Game;
using WEditor.Game.Scriptables;
namespace WEditor.Scenario
{
    public class DoorGeneration : MonoBehaviour
    {
        [SerializeField] KeyDoorScriptable platinum, golden, defaultKey, elevatorKey;
        [SerializeField] GameObject doorPrefab;
        [SerializeField] GameObject elevatorPrefab;
        public GameObject StartGeneration(Door door)
        {
            print(JsonUtility.ToJson(door));
            return door.tileName.Contains("_elv") ? GenerateElevator(door) : GenerateDoor(door);
        }
        private GameObject GenerateElevator(Door door)
        {
            Vector3 doorPosition = Vector3.zero;
            GameObject doorObject = Instantiate(elevatorPrefab, doorPosition, Quaternion.identity, null);

            if (door.topBottomSide == WallSide.TopBottom)
            {
                doorObject.transform.eulerAngles = new Vector3(0, 90, 0);
                doorPosition = new Vector3(door.position.x + .5f, .5f, door.position.y + .5f);
            }

            doorObject.GetComponentInChildren<Sensor>().keyDoorScriptable = GetKey(door.tileName);
            return doorObject;
        }
        private GameObject GenerateDoor(Door door)
        {
            Vector3 doorPosition = new Vector3(door.position.x + .5f, .5f, door.position.y + .5f);
            GameObject doorObject = Instantiate(doorPrefab, doorPosition, Quaternion.identity, null);

            if (door.topBottomSide == WallSide.TopBottom)
                doorObject.transform.eulerAngles = new Vector3(0, 90, 0);

            doorObject.GetComponentInChildren<Sensor>().keyDoorScriptable = GetKey(door.tileName);
            return doorObject;
        }

        private KeyDoorScriptable GetKey(string tileName) => tileName switch
        {
            "Doors_4" => defaultKey,
            "Doors_end_0" => elevatorKey
        };
    }
}
