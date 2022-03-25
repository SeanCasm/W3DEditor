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
        [SerializeField] Sprite eraserSprite;
        [SerializeField] Sprite spawnSprite;
        [SerializeField] GameObject spawnPrefab;
        private SpriteRenderer cursor;
        public bool isEraser { get; set; }//reference in editor
        private bool isSpawn { get; set; }
        public Sprite cursorSprite { get => cursor.sprite; set => cursor.sprite = value; }
        private Tile tileRef;
        private Vector2 mousePosition;
        private Vector3 worldPosition;
        private void OnEnable()
        {
            GameEvent.instance.onCreate += OnInit;
            GameEvent.instance.onPreviewModeEnter += OnMouseDisabled;
            GameEvent.instance.onPreviewModeExit += OnMouseEnabled;
        }
        private void OnDisable()
        {
            GameEvent.instance.onCreate -= OnInit;
            GameEvent.instance.onPreviewModeEnter -= OnMouseDisabled;
            GameEvent.instance.onPreviewModeExit -= OnMouseEnabled;
        }
        private void Start()
        {
            if (!instance) instance = this;
            else Destroy(this);

            cursor = GetComponent<SpriteRenderer>();
        }
        private void OnInit()
        {
            MapEditorInput.instance.EnableAndSetCallbacks(this);
        }

        private void OnMouseEnabled()
        {
            cursor.enabled = true;
            MapEditorInput.instance.ChangeInputActiveState(true);
        }
        private void OnMouseDisabled()
        {
            cursor.enabled = false;
            MapEditorInput.instance.ChangeInputActiveState(false);

        }
        public void Button_SetSpawn()
        {
            isSpawn = true;
            MapEditorInput.instance.ChangeInputOnInventory(true);
            spawnSprite = spawnPrefab.GetComponent<SpriteRenderer>().sprite;
            cursorSprite = spawnSprite;
        }
        public void SetAsset(Sprite sprite, Tile tile)
        {
            isSpawn = false;
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
                    EditorGrid.instance.SetSpawnObject(worldPosition, spawnPrefab);
                    return;
                }

                if (!isEraser && tileRef != null)
                {
                    EditorGrid.instance.SetTile(worldPosition, tileRef);
                }
                else if (tileRef != null)
                {
                    EditorGrid.instance.EraseTile(worldPosition);
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
                isEraser = !isEraser;
                cursor.sprite = isEraser ? cursorSprite : null;
            }
        }
    }
}
