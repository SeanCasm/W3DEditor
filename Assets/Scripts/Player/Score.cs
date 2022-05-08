using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Events;

namespace WEditor.Game.Player
{
    public class Score : MonoBehaviour
    {
        public int totalScore { get; private set; }
        private void OnEnable()
        {
            GameplayEvent.instance.onScoreChanged += Add;
        }
        public void Add(int amount)
        {
            totalScore += amount;
        }
    }
}
