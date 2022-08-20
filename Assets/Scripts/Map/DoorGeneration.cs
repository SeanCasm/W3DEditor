using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Game;
using WEditor.Game.Scriptables;
using WEditor.Utils;

namespace WEditor.Scenario
{
    public class DoorGeneration : MonoBehaviour
    {
        [SerializeField] KeyDoorScriptable platinum, golden, defaultKey, elevatorKey;
        [SerializeField] GameObject doorPrefab;
        private bool IsEndDoor(string doorName) => doorName.Contains("_end");
        public void StartGeneration(Door door, Door[,] doorGrid, List<GameObject> levelDoors)
        {

            Vector3 doorPosition = new Vector3(door.position.x, .5f, door.position.y).FixTilePivot();
            Vector3Int doorPositionInt = Vector3Int.FloorToInt(doorPosition);

            int x = doorPositionInt.x;
            int y = doorPositionInt.z;

            GameObject doorObject = Instantiate(doorPrefab, doorPosition, Quaternion.identity, null);

            if (door.topBottomSide == WallSide.TopBottom)
                doorObject.transform.eulerAngles = new Vector3(0, 90, 0);

            doorObject.GetComponentInChildren<IInteractable>().keyDoorScriptable = GetKey(door.tileName);
            levelDoors.Add(doorObject);

            if (!IsEndDoor(door.tileName))
                return;

        
            GameObject elevator = doorObject.transform.GetChild(3).gameObject;
            elevator.GetComponentInChildren<IInteractable>().keyDoorScriptable = GetKey("Doors_elv_2");
            if (doorGrid[x - 1, y] != null && doorGrid[x - 1, y].tileName.Contains("_elv")) //left-side
            {
                elevator.transform.localPosition = new Vector3(0, 0, -1);
            }
            else if (doorGrid[x + 1, y] != null && doorGrid[x + 1, y].tileName.Contains("_elv"))//right-side
            {
                elevator.transform.Rotate(new Vector3(0, 180, 0), Space.World);
                elevator.transform.localPosition = new Vector3(0, 0, 1);
            }
            else if (doorGrid[x, y - 1] != null && doorGrid[x, y - 1].tileName.Contains("_elv"))//bottom-side
            {
                elevator.transform.localPosition = new Vector3(0, 0, -1);
            }
            else if (doorGrid[x, y + 1] != null && doorGrid[x, y + 1].tileName.Contains("_elv"))//top-side
            {
                elevator.transform.Rotate(new Vector3(0, 180, 0), Space.World);
                elevator.transform.localPosition = new Vector3(0, 0, 1);
            }
            elevator.SetActive(true);

        }
        private KeyDoorScriptable GetKey(string tileName) => tileName switch
        {
            "Doors_default" => defaultKey,
            "Doors_end" => elevatorKey,
            "Doors_elv" => elevatorKey,
            "Doors_platinum" => platinum,
            "Doors_golden" => golden,
            _ => defaultKey
        };
    }
}
