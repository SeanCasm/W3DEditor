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
        public event Action<int> onEditorInventorySelected, onLivesChanged, onAmmoChanged, onScoreChanged, onHealthChanged;
        public void HealthChanged(int amount)
        {
            if (onHealthChanged != null)
            {
                onHealthChanged(amount);
            }
        }
        public void LivesChanged(int amount)
        {
            if (onLivesChanged != null)
            {
                onLivesChanged(amount);
            }
        }
        public void AmmoChanged(int amount)
        {
            if (onAmmoChanged != null)
            {
                onAmmoChanged(amount);
            }
        }
        public void ScoreChanged(int amount)
        {
            if (onScoreChanged != null)
            {
                onScoreChanged(amount);
            }
        }
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
