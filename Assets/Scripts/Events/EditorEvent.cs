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
        public void EditorExit()
        {
            if (onEditorExit != null)
            {
                onEditorExit();
            }
        }
        public void PlayModeEnter()
        {
            if (onPlayModeEnter != null)
            {
                onPlayModeEnter();
            }
        }
        public void PlayModeExit()
        {
            if (onPlayModeExit != null)
            {
                onPlayModeExit();
            }
        }
        public void SrollViewDisable()
        {
            if (onSrollViewDisable != null)
            {
                onSrollViewDisable();
            }
        }
        public void SrollViewEnable()
        {
            if (onSrollViewEnable != null)
            {
                onSrollViewEnable();
            }
        }
        public void EditorEnter()
        {
            if (onEditorEnter != null)
            {
                onEditorEnter();
            }
        }
       
        public void EditorInventorySelected(int id)
        {
            if (onEditorInventorySelected != null)
            {
                onEditorInventorySelected(id);
            }
        }
        public void EditorInventoryActiveChanged()
        {
            if (onEditorInventoryActiveChanged != null)
            {
                onEditorInventoryActiveChanged();
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