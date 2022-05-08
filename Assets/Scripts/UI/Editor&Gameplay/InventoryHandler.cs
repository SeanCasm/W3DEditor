using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Events;
using WEditor.Input;
namespace WEditor.UI
{

    public class InventoryHandler : MonoBehaviour
    {
        [SerializeField] GameObject itemPanel;
        private void OnEnable()
        {
            EditorEvent.instance.onEditorInventoryActiveChanged += OnInventaryActiveChanged;
        }
        private void OnDisable()
        {
            EditorEvent.instance.onEditorInventoryActiveChanged -= OnInventaryActiveChanged;
        }

        private void OnInventaryActiveChanged()
        {
            bool enable = !itemPanel.activeSelf;
            itemPanel.SetActive(enable);

            if (enable)
            {
                EditorCameraInput.instance.Disable();
                MapEditorInput.instance.OnInventoryEnable();
                return;
            }
            MapEditorInput.instance.OnInventoryDisable();
            EditorCameraInput.instance.Enable();
        }
    }
}
