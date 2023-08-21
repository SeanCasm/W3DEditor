using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using WEditor.Events;
namespace WEditor.Game.Player
{
    /// <summary>
    /// Store player stats.
    /// </summary>
    public class Stats : MonoBehaviour
    {
        private LevelStats levelStats = new(0, 0, 0);
        private void OnEnable()
        {
            GameplayEvent.instance.onScoreChanged += AddScore;
            GameplayEvent.instance.onKillsChanged += AddKill;
            GameplayEvent.instance.onTeasuresChanged += AddTeasure;
        }
        private void OnDisable()
        {
            DataHandler.levelStats = levelStats;
            GameplayEvent.instance.ScoreChanged(-levelStats.totalScore);
            levelStats = new(0, 0, 0);
            GameplayEvent.instance.onScoreChanged -= AddScore;
            GameplayEvent.instance.onKillsChanged -= AddKill;
            GameplayEvent.instance.onTeasuresChanged -= AddTeasure;
        }

        private void AddTeasure() => levelStats.totalTeasures++;
        private void AddKill() => levelStats.totalKills++;
        private void AddScore(int amount) => levelStats.totalScore += amount;
    }
    public struct LevelStats
    {
        public LevelStats(int totalScore, int totalKills, int totalTeasures)
        {
            this.totalScore = totalScore;
            this.totalKills = totalKills;
            this.totalTeasures = totalTeasures;
        }
        public int totalScore { get; set; }
        public int totalKills { get; set; }
        public int totalTeasures { get; set; }
    }
}
