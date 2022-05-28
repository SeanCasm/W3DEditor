using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Events;

namespace WEditor.Game.Player
{
    public class Score : MonoBehaviour
    {
        public int totalScore { get; private set; } = 0;
        private void OnEnable()
        {
            GameplayEvent.instance.onScoreChanged += Add;
        }
        private void OnDisable()
        {
            GameplayEvent.instance.ScoreChanged(-totalScore);
            totalScore = 0;
            GameplayEvent.instance.onScoreChanged -= Add;
        }
        public void Add(int amount)
        {
            totalScore += amount;
        }
    }
}
