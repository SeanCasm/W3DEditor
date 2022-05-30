using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
namespace WEditor.Events
{
    public class GameplayEvent : MonoBehaviour
    {
        public static GameplayEvent instance;
        public event Action<string> onAmmoChanged;
        public event Action<int> onLivesChanged, onScoreChanged, onHealthChanged, onArmourhChanged;

        private void OnEnable()
        {
            instance = this;
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
        public void AmmoChanged(string amount)
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
    }
}


