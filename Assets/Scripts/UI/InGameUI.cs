using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using WEditor.Events;
namespace WEditor.Game.UI
{
    public class InGameUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI lives, ammo, score, health;

        private void OnEnable()
        {
            GameEvent.instance.onAmmoChanged += OnAmmoChanged;
            GameEvent.instance.onScoreChanged += OnScoreChanged;
            GameEvent.instance.onLivesChanged += OnLivesChanged;
            GameEvent.instance.onHealthChanged += OnHealthChanged;
        }
        private void OnDisable()
        {
            GameEvent.instance.onAmmoChanged -= OnAmmoChanged;
            GameEvent.instance.onScoreChanged -= OnScoreChanged;
            GameEvent.instance.onLivesChanged -= OnLivesChanged;
            GameEvent.instance.onHealthChanged -= OnHealthChanged;
        }
        private void OnAmmoChanged(int amount)
        {
            ammo.text = "Ammo <br>" + amount;
        }
        private void OnLivesChanged(int amount)
        {
            lives.text = "Lives <br>" + amount;
        }
        private void OnScoreChanged(int amount)
        {
            score.text = "Score <br>" + amount;
        }
        private void OnHealthChanged(int amount)
        {
            health.text = "Health <br>" + amount;
        }
    }
}

