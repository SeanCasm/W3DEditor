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
        [SerializeField] GameObject elevatorPanel;

        private Tilemap mainTilemap;
        private Vector3Int doorPos;
        private bool inPlace = false;
        public void EnablePanel(Tilemap mainTilemap, Vector3Int doorPos)
        {
            elevatorPanel.SetActive(true);
            MessageHandler.instance.SetMessage("level_elv");

            EditorEvent.instance.ElevatorEditing();
            this.doorPos = doorPos;
            this.mainTilemap = mainTilemap;
        }
        public void SetElevatorPosition(string where)
        {
            if (inPlace)
            {
                MessageHandler.instance.SetError("level_end_door");
                EditorEvent.instance.ElevatorPlacementFailed(doorPos);
                return;
            }
            Tile elv = ScriptableObject.CreateInstance("Tile") as Tile;
            elv.sprite = elevatorSprite;
            elv.name = elevatorSprite.name;
            Vector3Int elvPos = Position(where);

            if (elvPos.x < 0 || elvPos.x > mainTilemap.size.x
            || elvPos.y > mainTilemap.size.y || elvPos.y < 0)
            {
                MessageHandler.instance.SetError("level_bounds");
                elevatorPanel.SetActive(false);
                EditorEvent.instance.ElevatorEditingExit();
                EditorEvent.instance.ElevatorPlacementFailed(doorPos);
                return;
            }
            if (DataHandler.CheckForWall(elvPos))
            {
                MessageHandler.instance.SetError("grid_elv");
                elevatorPanel.SetActive(false);
                EditorEvent.instance.ElevatorEditingExit();
                EditorEvent.instance.ElevatorPlacementFailed(doorPos);
                return;
            }
            elevatorPanel.SetActive(false);
            mainTilemap.SetTile(elvPos, elv);
            DataHandler.SetGrid(elvPos, new EditorGridLevelData(elvPos, elv.name));
            EditorEvent.instance.ElevatorEditingExit();
            inPlace = true;
        }
        public void Delete()
        {
            inPlace = false;
        }
        private Vector3Int Position(string where) => where switch
        {
            "top" => doorPos.GetTop(),
            "down" => doorPos.GetBottom(),
            "left" => doorPos.GetLeft(),
            "right" => doorPos.GetRight()
        };
    }
}
