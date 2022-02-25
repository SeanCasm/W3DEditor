using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace WEditor.UI
{
    public class ObjectSelectable : ItemSelectable
    {
        [SerializeField] AssetReference assetReference;
        private GameObject assetGameObject;
        void Start()
        {
            assetReference.InstantiateAsync().Completed += LoadAsset;
        }
        void LoadAsset(AsyncOperationHandle<GameObject> asset)
        {
            assetGameObject = asset.Result;
            assetSprite = assetGameObject.GetComponent<SpriteRenderer>().sprite;
            assetGameObject.SetActive(false);
        }
    }
}
