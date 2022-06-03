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
        public List<GameObject> StartGeneration(Door door, Door[,] doorGrid)
        {
            List<GameObject> objects = new List<GameObject>();

            Vector3 doorPosition = new Vector3(door.position.x, .5f, door.position.y).FixTilePivot();
            Vector3Int doorPositionInt = Vector3Int.FloorToInt(doorPosition);

            int x = doorPositionInt.x;
            int y = doorPositionInt.z;

            GameObject doorObject = Instantiate(doorPrefab, doorPosition, Quaternion.identity, null);

            if (door.topBottomSide == WallSide.TopBottom)
                doorObject.transform.eulerAngles = new Vector3(0, 90, 0);

            doorObject.GetComponentInChildren<IInteractable>().keyDoorScriptable = GetKey(door.tileName);
            objects.Add(doorObject);

            if (IsEndDoor(door.tileName))
            {
                GameObject elevator = doorObject.transform.GetChild(3).gameObject;
                elevator.GetComponentInChildren<IInteractable>().keyDoorScriptable = GetKey("Doors_elv_2");
                if ((x - 1, y).InsideBounds(doorGrid.GetLength(0), doorGrid.GetLength(1))
                && doorGrid[x - 1, y] != null)
                {
                    elevator.transform.localPosition = new Vector3(0, 0, -1);
                }
                else if ((x + 1, y).InsideBounds(doorGrid.GetLength(0), doorGrid.GetLength(1))
                && doorGrid[x + 1, y] != null)
                {
                    elevator.transform.Rotate(new Vector3(0, 180, 0), Space.World);
                    elevator.transform.localPosition = new Vector3(0, 0, 1);
                }
                else if ((x, y - 1).InsideBounds(doorGrid.GetLength(0), doorGrid.GetLength(1))
                && doorGrid[x, y - 1] != null)
                {
                    elevator.transform.localPosition = new Vector3(0, 0, -1);
                }
                else if ((x, y + 1).InsideBounds(doorGrid.GetLength(0), doorGrid.GetLength(1))
                && doorGrid[x, y + 1] != null)
                {
                    elevator.transform.Rotate(new Vector3(0, 180, 0), Space.World);
                    elevator.transform.localPosition = new Vector3(0, 0, 1);
                }
                elevator.SetActive(true);
                objects.Add(elevator);
            }

            return objects;
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
