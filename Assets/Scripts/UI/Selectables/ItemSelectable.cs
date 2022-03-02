using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Tilemaps;
using WEditor.ScenarioInput;
using UnityEngine.UI;

namespace WEditor.UI
{
    public class ItemSelectable : MonoBehaviour
    {
        protected Sprite assetSprite;
        private Button button;
        protected void OnEnable()
        {
            assetSprite = GetComponent<Image>().sprite;
            button = GetComponent<Button>();
            button.onClick.AddListener(Button_SelectItem);
        }
        public virtual void Button_SelectItem()
        {
           
        }
        public virtual void Button_DeselectItem()
        {

        }
    }
}

