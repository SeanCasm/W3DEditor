using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using WEditor.Scenario;
using static WInput;
namespace WEditor.Input
{
    public class MouseHandler : MonoBehaviour, IMapEditorActions
    {
        public static MouseHandler instance;
        [SerializeField] Camera mainCamera;
        [SerializeField] GameObject mouseRefecence;
        private SpriteRenderer cursor;
        private WInput wInput;
        public GameObject Mouse { get => mouseRefecence; }
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
            worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 10));
            transform.position = worldPosition;
            EditorGrid.instance.SetPreviewTileOnAim(worldPosition);
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                EditorGrid.instance.SetTile(worldPosition, tileRef);
            }
        }
    }

}
