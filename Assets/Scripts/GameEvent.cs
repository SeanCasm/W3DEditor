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
        public event Action<bool> onEditorInventoryActiveChanged;
        public event Action<int> onEditorInventorySelected;
        public event Action<int> onLivesChanged, onAmmoChanged, onScoreChanged, onHealthChanged, onArmourhChanged;
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
        public void HealthChanged(int amount)
        {
            if (onHealthChanged != null)
            {
                onHealthChanged(amount);
            }
        }
        public void ArmourhChanged(int amount)
        {
            if (onArmourhChanged != null)
            {
                onArmourhChanged(amount);
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