using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace WEditor.Events
{
    public class EditorEvent : MonoBehaviour
    {
        public static EditorEvent instance;
        private void OnEnable()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(this);
            }
        }
        public event Action onEditorExit, onEditorEnter;
        public event Action onSrollViewEnable, onSrollViewDisable;
        public event Action onPreviewModeEnter, onPreviewModeExit;
        public event Action onPlayModeEnter, onPlayModeExit;
        public event Action onEditorInventoryActiveChanged;
        public event Action<int> onEditorInventorySelected;
        public event Action onElevatorEditing, onElevatorEditingExit;
        public event Action<Vector3Int> onElevatorPlacementFailed;
        public void ElevatorPlacementFailed(Vector3Int pos)
        {
            onElevatorPlacementFailed?.Invoke(pos);
        }
        public void ElevatorEditingExit()
        {
            onElevatorEditingExit?.Invoke();
        }
        public void ElevatorEditing()
        {
            onElevatorEditing?.Invoke();
        }
        public void EditorExit()
        {
            onEditorExit?.Invoke();
        }
        public void PlayModeEnter()
        {
            onPlayModeEnter?.Invoke();
        }
        public void PlayModeExit()
        {
            onPlayModeExit?.Invoke();
        }
        public void SrollViewDisable()
        {
            onSrollViewDisable?.Invoke();
        }
        public void SrollViewEnable()
        {
            onSrollViewEnable?.Invoke();
        }
        public void EditorEnter()
        {
            onEditorEnter?.Invoke();
        }

        public void EditorInventorySelected(int id)
        {
            onEditorInventorySelected?.Invoke(id);
        }
        public void EditorInventoryActiveChanged()
        {
            onEditorInventoryActiveChanged?.Invoke();
        }
        public void PreviewModeEnter()
        {
            onPreviewModeEnter?.Invoke();
        }
        public void PreviewModeExit()
        {
            onPreviewModeExit?.Invoke();
        }
    }
}