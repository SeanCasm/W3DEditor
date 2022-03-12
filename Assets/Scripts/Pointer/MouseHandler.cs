using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using WEditor.Scenario;
using WEditor.Events;
using Cinemachine;
using static WInput;
namespace WEditor.ScenarioInput
{
    public class MouseHandler : MonoBehaviour, IMapEditorActions
    {
        public static MouseHandler instance;
        [SerializeField] Camera mainCamera;
        [SerializeField] GameObject itemPanel;
        [SerializeField] Sprite eraserSprite;
        [SerializeField] Sprite spawnSprite;
        [SerializeField] Tile spawnTile;
        private SpriteRenderer cursor;
        public bool isEraser { get; set; }//reference in editor
        public Sprite cursorSprite { get => cursor.sprite; set => cursor.sprite = value; }
        private Tile tileRef;
        private Vector2 mousePosition;
        private Vector3 worldPosition;
        private void Start()
        {
            if (!instance) instance = this;
            else Destroy(this);

            GameInput.instance.EnableMapEditorInputsAndSetCallbacks(this);

            cursor = GetComponent<SpriteRenderer>();
        }
        private void OnEnable()
        {
            GameEvent.instance.onPreviewModeEnter += OnMouseDisabled;
            GameEvent.instance.onPreviewModeExit += OnMouseEnabled;
        }
        private void OnDisable()
        {
            GameEvent.instance.onPreviewModeEnter -= OnMouseDisabled;
            GameEvent.instance.onPreviewModeExit -= OnMouseEnabled;
        }
        private void OnMouseEnabled()
        {
            cursor.enabled=true;
            GameInput.instance.ChangeActiveMapEditorInputs(true);
        }
        private void OnMouseDisabled()
        {
            cursor.enabled=false;
            GameInput.instance.ChangeActiveMapEditorInputs(false);
        }
        public void Button_SetSpawn()
        {
            cursorSprite = spawnSprite;
            tileRef = spawnTile;
        }
        public void SetAsset(Sprite sprite, Tile tile)
        {
            cursorSprite = sprite;
            tileRef = tile;
        }
        public void OnAim(InputAction.CallbackContext context)
        {
            mousePosition = context.ReadValue<Vector2>();
            worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, mainCamera.nearClipPlane*20));
            worldPosition = new Vector3(worldPosition.x, worldPosition.y, worldPosition.z);
            transform.position = worldPosition;
            EditorGrid.instance.SetPreviewTileOnAim(worldPosition);
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            if (context.started && tileRef != null)
            {
                if (!isEraser)
                {
                    EditorGrid.instance.SetTile(worldPosition, tileRef);
                }
                else
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
