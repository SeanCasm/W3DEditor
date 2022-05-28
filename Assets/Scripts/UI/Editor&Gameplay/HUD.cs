using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using WEditor.Events;
namespace WEditor.Game.UI
{
    /// <summary>
    /// A class that handle with the HUD in editor mode and play mode.
    /// </summary>
    public class HUD : MonoBehaviour
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
            int currentScore = int.Parse(score.text.Split(" ")[1]) + amount;
            score.text = "Score\n " + currentScore;
        }
        private void OnHealthChanged(int amount)
        {
            health.text = "Health <br>" + amount;
        }
    }
}

