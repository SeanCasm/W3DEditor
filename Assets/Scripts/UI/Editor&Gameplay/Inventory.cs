using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using WEditor.Events;
using WEditor.ScenarioInput;
namespace WEditor.UI
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] GameObject itemGroup;
        [SerializeField] Sprite[] assets;
        [SerializeField] int id;
        private List<GameObject> contentRef = new List<GameObject>();
        private bool isInScrollView;
        private void Start()
        {
            foreach (var sprite in assets)
            {
                GameObject asset = new GameObject();
                Image assetImage = asset.AddComponent<Image>();
                Button button = asset.AddComponent<Button>();
                assetImage.sprite = sprite;
                button.onClick.AddListener(() => { SetItemSpriteToMousePointer(sprite); });

                contentRef.Add(asset);
                asset.SetActive(false);

                Instantiate(asset);
            }
        }
        private void OnEnable()
        {
            GameEvent.instance.onEditorInventorySelected += OnSelected;
        }
        private void OnDisable()
        {
            GameEvent.instance.onEditorInventorySelected -= OnSelected;
        }
        public void Button_OnSelectedItem()
        {
            GameEvent.instance.EditorInventorySelected(id);
        }
        public void SetItemSpriteToMousePointer(Sprite sprite)
        {
            Tile itemTile = new Tile();
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
