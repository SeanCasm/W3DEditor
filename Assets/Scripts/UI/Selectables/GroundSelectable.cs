using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using WEditor.Input;
namespace WEditor.UI
{
    public class GroundSelectable : ItemSelectable
    {
        [SerializeField] Tile groundTile;
        public override void Button_SelectItem()
        {
           MouseHandler.instance.SetAsset(assetSprite,groundTile);
        }
    }
}
