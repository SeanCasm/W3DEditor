using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace WEditor.Events
{
    public class GameEvent : MonoBehaviour
    {
        public static GameEvent instance;
        private void Awake()
        {
            instance = this;
        }

        public event Action onPreviewModeEnter;
        public event Action onPreviewModeExit;
        public event Action<bool> onEditorInventoryActiveChanged;
        public event Action<int> onEditorInventorySelected;
        public void EditorInventorySelected(int id)
        {
            if (onEditorInventorySelected != null)
            {
                onEditorInventorySelected(id);
            }
        }
        public void EditorInventoryActiveChanged(bool enable)
        {
            if (onEditorInventoryActiveChanged != null)
            {
                onEditorInventoryActiveChanged(enable);
            }
        }
        public void PreviewModeEnter()
        {
            if (onPreviewModeEnter != null)
            {
                onPreviewModeEnter();
            }
        }
        public void PreviewModeExit()
        {
            if (onPreviewModeExit != null)
            {
                onPreviewModeExit();
            }
        }

    }
}
