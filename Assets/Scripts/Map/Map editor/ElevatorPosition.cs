using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using WEditor.Events;
using WEditor.Input;
using WEditor.UI;
using WEditor.Utils;

namespace WEditor.Scenario.Editor
{
    /// <summary>
    /// Handle the elevator generation position in the editor grid.
    /// </summary>
    public class ElevatorPosition : MonoBehaviour
    {
        [SerializeField] Sprite elevatorSprite;
        public Tilemap mainTilemap { get; set; }
        public Vector3Int cellPos { get; set; }
        public GameObject elevatorPanel { get; set; }
        public void SetElevatorPosition(string where)
        {
            Tile elv = ScriptableObject.CreateInstance("Tile") as Tile;
            elv.sprite = elevatorSprite;
            Vector3Int elvPos = Position(where);
            if (DataHandler.CheckForWall(elvPos))
            {
                MessageHandler.instance.SetError("grid_elv");
                return;
            }
            elevatorPanel.SetActive(false);
            mainTilemap.SetTile(elvPos, elv);
            elevatorPanel = null;
            DataHandler.SetGrid(cellPos, new EditorGridLevelData(cellPos, elv.name));
            EditorEvent.instance.ElevatorEditingExit();
        }
        private Vector3Int Position(string where) => where switch
        {
            "top" => cellPos.GetTop(),
            "down" => cellPos.GetBottom(),
            "left" => cellPos.GetLeft(),
            "right" => cellPos.GetRight()
        };
    }
}
