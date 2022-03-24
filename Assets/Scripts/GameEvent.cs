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
        public event Action onSrollViewEnable, onSrollViewDisable;
        public event Action onPreviewModeEnter, onPreviewModeExit;
        public event Action<bool> onEditorInventoryActiveChanged;
        public event Action<int> onEditorInventorySelected;
        public event Action<int> onLivesChanged, onAmmoChanged, onScoreChanged, onHealthChanged, onArmourhChanged;
        public event Action onCreate;
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
        public void Create()
        {
            if (onCreate != null)
            {
                onCreate();
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