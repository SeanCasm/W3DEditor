using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using WEditor.ScenarioInput;
namespace WEditor.UI
{
    public class GroundSelectable : ItemSelectable
    {
        private Tile groundTile;
        private void Start()
        {
            groundTile = new Tile();
            groundTile.name = assetSprite.name;
            groundTile.sprite = assetSprite;
        }
        public override void Button_SelectItem()
        {
            MouseHandler.instance.SetAsset(assetSprite, groundTile);
        }
    }
}
