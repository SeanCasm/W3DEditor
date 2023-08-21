using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using WEditor.Events;
using WEditor.Input;
using WEditor.Game.Scriptables;
namespace WEditor.UI
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] GameObject itemGroup;
        [SerializeField] ScenarioScriptable spriteAssets;
        [SerializeField] int id;
        private List<GameObject> contentRef = new List<GameObject>();
        private bool isInScrollView;
        private void Start()
        {
            foreach (var sprite in spriteAssets.SpritesCollection)
            {
                GameObject assetObject = new GameObject();
                Image assetImage = assetObject.AddComponent<Image>();
                Button button = assetObject.AddComponent<Button>();
                assetImage.sprite = sprite;
                button.onClick.AddListener(() => { SetItemSpriteToMousePointer(sprite); });

                contentRef.Add(assetObject);
                assetObject.SetActive(false);

                Instantiate(assetObject);
            }
        }
        private void OnEnable()
        {
            EditorEvent.instance.onEditorInventorySelected += OnSelected;
        }
        private void OnDisable()
        {
            EditorEvent.instance.onEditorInventorySelected -= OnSelected;
        }
        public void Button_OnSelectedItem()
        {
            EditorEvent.instance.EditorInventorySelected(id);
        }
        public void SetItemSpriteToMousePointer(Sprite sprite)
        {
            Tile itemTile = ScriptableObject.CreateInstance("Tile") as Tile;
            itemTile.name = sprite.name;
            itemTile.sprite = sprite;
            MouseHandler.instance.SetAsset(sprite, itemTile);
        }
        private void OnSelected(int id)
        {
            if (this.id == id && !isInScrollView)
            {
                foreach (var item in contentRef)
                {
                    item.gameObject.SetActive(true);
                    item.transform.SetParent(itemGroup.transform);
                    item.transform.localScale = Vector2.one;
                }
                isInScrollView = true;
            }
            else if (this.id != id)
            {
                foreach (var item in contentRef)
                {
                    item.gameObject.SetActive(false);
                    item.transform.SetParent(null);
                }
                isInScrollView = false;
            }
        }
    }
}
