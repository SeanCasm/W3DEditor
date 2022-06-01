using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Events;
using TMPro;
namespace WEditor.Game.Player
{
    /// <summary>
    /// Store player stats.
    /// </summary>
    public class Stats : MonoBehaviour
    {
        private LevelStats levelStats = new LevelStats(0, 0, 0);
        private void OnEnable()
        {
            GameplayEvent.instance.onScoreChanged += AddScore;
            GameplayEvent.instance.onKillsChanged += AddKill;
            GameplayEvent.instance.onTeasuresChanged += AddTeasure;
            GameplayEvent.instance.onLevelCompeted += WriteLevelStats;
        }
        private void OnDisable()
        {
            GameplayEvent.instance.ScoreChanged(-levelStats.totalScore);
            levelStats = new LevelStats(0, 0, 0);
            GameplayEvent.instance.onScoreChanged -= AddScore;
            GameplayEvent.instance.onKillsChanged -= AddKill;
            GameplayEvent.instance.onTeasuresChanged -= AddTeasure;
            GameplayEvent.instance.onLevelCompeted -= WriteLevelStats;
        }
        /// <summary>
        /// Write the player stats in the text component of the level ending UI.
        /// </summary>
        /// <param name="infoStatsUIText"></param>
        private void WriteLevelStats(InfoStatsUIText infoStatsUIText)
        {
            infoStatsUIText.ScoreText = levelStats.totalScore.ToString();
            infoStatsUIText.KillsText = levelStats.totalKills.ToString();
            infoStatsUIText.TeasuresText = levelStats.totalTeasures.ToString();
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
