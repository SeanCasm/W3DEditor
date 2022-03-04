using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using WEditor.Scenario;
using WEditor.Events;
using static WInput;
namespace WEditor.ScenarioInput
{
    public class MouseHandler : MonoBehaviour, IMapEditorActions
    {
        public static MouseHandler instance;
        [SerializeField] Camera mainCamera;
        [Header("Scroll settings")]
        [SerializeField] Transform virtualCamera;
        private SpriteRenderer cursor;
        private WInput wInput;
        public Sprite cursorSprite { get => cursor.sprite; set => cursor.sprite = value; }
        private Tile tileRef;
        private Vector2 mousePosition, worldPosition;
        private void Awake()
        {
            if (!instance) instance = this;
            else Destroy(this);
            wInput = new WInput();
            wInput.MapEditor.Enable();
            wInput.MapEditor.SetCallbacks(this);
            cursor = GetComponent<SpriteRenderer>();

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
            wInput.MapEditor.Enable();
        }
        private void OnMouseDisabled()
        {
            wInput.MapEditor.Disable();
        }
        public void SetAsset(Sprite sprite, Tile tile)
        {
            cursorSprite = sprite;
            tileRef = tile;
        }
        public void OnAim(InputAction.CallbackContext context)
        {
            mousePosition = context.ReadValue<Vector2>();
            worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, virtualCamera.position.z * -1));
            transform.position = worldPosition;
            EditorGrid.instance.SetPreviewTileOnAim(worldPosition);
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                if (tileRef != null) EditorGrid.instance.SetTile(worldPosition, tileRef);
            }
        }

        public void OnOpeninventary(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                GameEvent.instance.EditorInventoryOpened();
            }
        }
    }
}
