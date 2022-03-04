using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WEditor.Events;
namespace WEditor.UI
{

    public class InventoryHandler : MonoBehaviour
    {
        [SerializeField] GameObject itemPanel;
        private void OnEnable()
        {
            GameEvent.instance.onEditorInventoryOpened += OnInventaryActiveChanged;
        }
        private void OnDisable()
        {
            GameEvent.instance.onEditorInventoryOpened -= OnInventaryActiveChanged;
        }
        
        private void OnInventaryActiveChanged()
        {
            itemPanel.SetActive(!itemPanel.activeSelf);
        }
    }
}
