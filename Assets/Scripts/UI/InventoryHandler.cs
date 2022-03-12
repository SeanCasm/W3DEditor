using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Events;
using WEditor.ScenarioInput;
namespace WEditor.UI
{

    public class InventoryHandler : MonoBehaviour
    {
        [SerializeField] GameObject itemPanel;
        private void OnEnable()
        {
            GameEvent.instance.onEditorInventoryActiveChanged += OnInventaryActiveChanged;
        }
        private void OnDisable()
        {
            GameEvent.instance.onEditorInventoryActiveChanged -= OnInventaryActiveChanged;
        }

        private void OnInventaryActiveChanged(bool enable)
        {
            itemPanel.SetActive(enable);

            if (enable)
            {
                MouseHandler.instance.isEraser=false;
                GameInput.instance.DisableInputsForInventoryOpened();
                return;
            }
            GameInput.instance.EnableInputsForInventoryClosed();
        }
    }
}
