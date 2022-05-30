using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using WEditor.Events;
using UnityEngine.UI;

namespace WEditor.Game.UI
{
    /// <summary>
    /// A class that handle with the HUD in editor mode and play mode.
    /// </summary>
    public class HUD : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI lives, ammo, score, health;
        [SerializeField] Animator playerStatusUIAnimator;
        [SerializeField] Image goldenKey, platinumKey;
        private void OnEnable()
        {
            EditorEvent.instance.onPreviewModeExit += OnPreviewModeExit;
            GameplayEvent.instance.onAmmoChanged += OnAmmoChanged;
            GameplayEvent.instance.onScoreChanged += OnScoreChanged;
            GameplayEvent.instance.onLivesChanged += OnLivesChanged;
            GameplayEvent.instance.onHealthChanged += OnHealthChanged;
            GameplayEvent.instance.onKeyPickedUp += OnKeyPickedUp;
        }
        private void OnDisable()
        {
            EditorEvent.instance.onPreviewModeExit -= OnPreviewModeExit;
            GameplayEvent.instance.onAmmoChanged -= OnAmmoChanged;
            GameplayEvent.instance.onScoreChanged -= OnScoreChanged;
            GameplayEvent.instance.onLivesChanged -= OnLivesChanged;
            GameplayEvent.instance.onHealthChanged -= OnHealthChanged;
            GameplayEvent.instance.onKeyPickedUp -= OnKeyPickedUp;
        }
        private void OnPreviewModeExit()
        {
            goldenKey.color = platinumKey.color = Color.black;
        }
        private void OnKeyPickedUp(int key)
        {
            if (key == 0)
                goldenKey.color = Color.white;
            else
                platinumKey.color = Color.white;
        }
        private void OnAmmoChanged(string amount)
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

