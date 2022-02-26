using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using WEditor.Scenario;
using static WInput;
namespace WEditor.ScenarioInput
{
    public class MouseHandler : MonoBehaviour, IMapEditorActions
    {
        public static MouseHandler instance;
        [SerializeField] Camera mainCamera;
        [Header("Scroll settings")]
        [SerializeField] Transform virtualCamera;
        [SerializeField] float minZoom, maxZoom;
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
            wInput.Enable();
            wInput.MapEditor.SetCallbacks(this);
            cursor = GetComponent<SpriteRenderer>();
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

        public void OnZoom(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                float cameraZoom = context.ReadValue<Vector2>().normalized.y;
                Vector3 cameraPos = virtualCamera.position;
                if (cameraPos.z >= minZoom && cameraPos.z <= maxZoom)
                {
                    virtualCamera.position = new Vector3(cameraPos.x, cameraPos.y, Mathf.Clamp(cameraPos.z + cameraZoom, minZoom, maxZoom));
                }
            }
        }
    }

}
