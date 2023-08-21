using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace WEditor.Events
{
    public class GameplayEvent : MonoBehaviour
    {
        public static GameplayEvent instance;
        public event Action<int> onKeyPickedUp;
        public event Action<string> onAmmoChanged;
        public event Action onDeath, onReset;
        public event Action onTeasuresChanged, onKillsChanged;
        public event Action<int> onScoreChanged, onHealthChanged;
        public event Action<List<KeyType>> onInteracted;
        private void OnEnable() => instance = this;
        public void OnReset()
        {
            onReset?.Invoke();
        }
        public void OnDeath()
        {
            onDeath?.Invoke();
        }
        public void Interacted(List<KeyType> keyTrigger)
        {
            onInteracted?.Invoke(keyTrigger);
        }
        public void TeasuresChanged()
        {
            onTeasuresChanged?.Invoke();
        }
        public void KillsChanged()
        {
            onKillsChanged?.Invoke();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">0: golden, 1: platinum</param>
        public void KeyPickedUp(int key)
        {
            onKeyPickedUp?.Invoke(key);
        }
        public void HealthChanged(int amount)
        {
            onHealthChanged?.Invoke(amount);
        }

        public void AmmoChanged(string amount)
        {
            onAmmoChanged?.Invoke(amount);
        }
        public void ScoreChanged(int amount)
        {
            onScoreChanged?.Invoke(amount);
        }
    }
}