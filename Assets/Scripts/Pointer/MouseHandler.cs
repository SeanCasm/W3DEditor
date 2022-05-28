using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using WEditor.Scenario.Editor;
using WEditor.Events;
using UnityEngine.UI;
using static WInput;
namespace WEditor.Input
{
    public class MouseHandler : MonoBehaviour, IMapEditorActions
    {
        public static MouseHandler instance;
        [SerializeField] Camera mainCamera;
        [SerializeField] GameObject itemPanel, mouseObject;
        [SerializeField] Sprite eraserSprite, spawnSprite, boldSprite, penSprite;
        [SerializeField] SpriteRenderer cursor;

        [Header("Item selected settings")]
        [SerializeField] GameObject container;
        [SerializeField] Image splashImage;
        public MouseType mouseType { get; set; }
        public Sprite cursorSprite { get => cursor.sprite; set => cursor.sprite = value; }
        private Tile tileRef;
        public Vector2 mousePosition { get; private set; }
        private bool isSpawn { get => mouseType == MouseType.Spawn; }
        private Vector3 worldPosition;
        private void OnEnable()
        {
            EditorEvent.instance.onEditorEnter += OnMouseEnabled;
            EditorEvent.instance.onPreviewModeEnter += OnMouseDisabled;
            EditorEvent.instance.onEditorExit += OnMouseDisabled;
            EditorEvent.instance.onPreviewModeExit += OnMouseEnabled;
        }
        private void OnDisable()
        {
            EditorEvent.instance.onPreviewModeEnter -= OnMouseDisabled;
            EditorEvent.instance.onPreviewModeExit -= OnMouseEnabled;
            EditorEvent.instance.onEditorExit -= OnMouseDisabled;
            EditorEvent.instance.onEditorEnter -= OnMouseEnabled;
        }
        private void Start()
        {
            instance = this;
        }

        private void OnMouseEnabled()
        {
            cursor.enabled = true;
            mouseType = MouseType.Pen;
            MapEditorInput.instance.EnableAndSetCallbacks(this);
        }
        private void OnMouseDisabled()
        {
            cursor.enabled = false;
            cursorSprite = null;
            tileRef = null;
            mouseType = MouseType.Pen;
            container.SetActive(false);
            MapEditorInput.instance.Disable();
        }
        public void Button_SetSpawn()
        {
            mouseType = MouseType.Spawn;
            //GameEvent.instance.EditorInventoryActiveChanged();
            cursorSprite = spawnSprite;
            tileRef = null;
        }
        public void Button_SetEraser()
        {
            mouseType = MouseType.Eraser;
            //GameEvent.instance.EditorInventoryActiveChanged();
            container.SetActive(false);
            cursorSprite = eraserSprite;
            tileRef = null;
        }
        public void Button_SetPen()
        {
            mouseType = MouseType.Pen;
            cursorSprite = penSprite;
        }
        public void SetAsset(Sprite sprite, Tile tile)
        {
            mouseType = MouseType.Pen;
            cursorSprite = penSprite;
            container.SetActive(true);
            splashImage.sprite = sprite;
            tileRef = tile;
        }
        public void OnAim(InputAction.CallbackContext context)
        {
            mousePosition = context.ReadValue<Vector2>();
            worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, mainCamera.nearClipPlane * mainCamera.transform.position.y));
            worldPosition = new Vector3(worldPosition.x, worldPosition.y, worldPosition.z);
            mouseObject.transform.position = worldPosition;
            if (mouseType == MouseType.Eraser)
                EditorGrid.instance.SetEraserTileOnAim(worldPosition);
            else
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
                switch (mouseType)
                {
                    case MouseType.Eraser:
                        EditorGrid.instance.EraseTile(worldPosition);
                        break;
                    case MouseType.Pen:
                        EditorGrid.instance.SetTile(worldPosition, tileRef);
                        break;
                }
            }
        }
        public void OnOpeninventary(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                EditorEvent.instance.EditorInventoryActiveChanged();
            }
        }

        public void OnEraser(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                if (mouseType != MouseType.Eraser) Button_SetEraser();
                else Button_SetPen();
            }
        }
    }
    public enum MouseType
    {
        Spawn, Bold, Eraser, Pen
    }
}
