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
        [SerializeField] Animator playerStatusUIAnimator;

        private void OnEnable()
        {
            GameplayEvent.instance.onAmmoChanged += OnAmmoChanged;
            GameplayEvent.instance.onScoreChanged += OnScoreChanged;
            GameplayEvent.instance.onLivesChanged += OnLivesChanged;
            GameplayEvent.instance.onHealthChanged += OnHealthChanged;
        }
        private void OnDisable()
        {
            GameplayEvent.instance.onAmmoChanged -= OnAmmoChanged;
            GameplayEvent.instance.onScoreChanged -= OnScoreChanged;
            GameplayEvent.instance.onLivesChanged -= OnLivesChanged;
            GameplayEvent.instance.onHealthChanged -= OnHealthChanged;
        }
        private void OnAmmoChanged(int amount)
        {
            ammo.text = "Ammo <br>" + amount;
        }
        private void OnLivesChanged(int amount)
        {
            lives.text = "Lives <br>" + amount;
            int healthTier = amount / 16;
            playerStatusUIAnimator.SetInteger("hurtTier", healthTier);

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

