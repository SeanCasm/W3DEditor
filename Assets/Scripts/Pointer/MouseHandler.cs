using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using WEditor.Scenario.Editor;
using WEditor.Events;
using static WInput;
namespace WEditor.Input
{
    public class MouseHandler : MonoBehaviour, IMapEditorActions
    {
        public static MouseHandler instance;
        [SerializeField] Camera mainCamera;
        [SerializeField] GameObject itemPanel;
        [SerializeField] Sprite eraserSprite, spawnSprite, boldSprite;
        [SerializeField] SpriteRenderer cursor;
        public MouseType mouseType { get; set; }
        public Sprite cursorSprite { get => cursor.sprite; set => cursor.sprite = value; }
        private Tile tileRef;
        private Vector2 mousePosition;
        private bool isSpawn { get => mouseType == MouseType.Spawn; }
        private Vector3 worldPosition;
        private void OnEnable()
        {
            MapEditorInput.instance.EnableAndSetCallbacks(this);
            OnMouseEnabled();
            GameEvent.instance.onEditorEnter += OnMouseEnabled;
            GameEvent.instance.onPreviewModeEnter += OnMouseDisabled;
            GameEvent.instance.onEditorExit += OnMouseDisabled;
            GameEvent.instance.onPreviewModeExit += OnMouseEnabled;
        }
        private void OnDisable()
        {
            OnMouseDisabled();
            GameEvent.instance.onPreviewModeEnter -= OnMouseDisabled;
            GameEvent.instance.onPreviewModeExit -= OnMouseEnabled;
            GameEvent.instance.onEditorExit -= OnMouseDisabled;
            GameEvent.instance.onEditorEnter -= OnMouseEnabled;
        }
        private void Start()
        {
            instance = this;
        }

        private void OnMouseEnabled()
        {
            cursor.enabled = true;
            mouseType = MouseType.None;
            MapEditorInput.instance.ChangeInputActiveState(true);
        }
        private void OnMouseDisabled()
        {
            cursor.enabled = false;
            cursorSprite = null;
            tileRef = null;
            mouseType = MouseType.None;
            MapEditorInput.instance.ChangeInputActiveState(false);
        }
        public void Button_SetSpawn()
        {
            mouseType = MouseType.Spawn;
            MapEditorInput.instance.ChangeInputOnInventory(true);
            cursorSprite = spawnSprite;
        }
        public void Button_SetEraser()
        {
            mouseType = MouseType.Eraser;
            cursorSprite = eraserSprite;
        }
        public void Button_SetPen()
        {
            mouseType = MouseType.None;
        }
        private void Disable()
        {
            gameObject.SetActive(false);
        }
        public void SetAsset(Sprite sprite, Tile tile)
        {
            if (isSpawn)
                mouseType = MouseType.None;

            cursorSprite = sprite;
            tileRef = tile;
        }
        public void OnAim(InputAction.CallbackContext context)
        {
            mousePosition = context.ReadValue<Vector2>();
            worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, mainCamera.nearClipPlane * mainCamera.transform.position.y));
            worldPosition = new Vector3(worldPosition.x, worldPosition.y, worldPosition.z);
            transform.position = worldPosition;
            EditorGrid.instance.SetPreviewTileOnAim(worldPosition);
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                if (isSpawn)
                {
                    EditorGrid.instance.SetSpawnObject(worldPosition);
                    return;
                }
                if (tileRef != null)
                {
                    switch (mouseType)
                    {
                        case MouseType.Eraser:
                            EditorGrid.instance.EraseTile(worldPosition);
                            break;
                        case MouseType.None:
                            EditorGrid.instance.SetTile(worldPosition, tileRef);
                            break;
                    }
                }
            }
        }
        public void OnOpeninventary(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                GameEvent.instance.EditorInventoryActiveChanged(!itemPanel.activeSelf);
                MapEditorInput.instance.ChangeInputOnInventory(!itemPanel.activeSelf);
            }
        }

        public void OnEraser(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                if (mouseType != MouseType.Eraser)
                {
                    mouseType = MouseType.Eraser;
                    cursor.sprite = cursorSprite;
                    tileRef = null;
                }
                else
                {
                    mouseType = MouseType.None;
                    cursor.sprite = null;
                }
            }
        }
    }
    public enum MouseType
    {
        Spawn, Bold, Eraser, None
    }
}
