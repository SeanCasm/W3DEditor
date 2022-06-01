using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using WEditor.Events;

namespace WEditor.UI
{
    /// <summary>
    /// Handle the level completed screen.
    /// </summary>
    public class EndedGame : MonoBehaviour
    {
        [SerializeField] InfoStatsUIText infoStats;
        private void OnEnable() => GameplayEvent.instance.LevelCompeted(infoStats);
    }
}
[System.Serializable]
public struct InfoStatsUIText
{
    [SerializeField] TextMeshProUGUI scoreText, killsText, teasuresText;
    public InfoStatsUIText(TextMeshProUGUI scoreText, TextMeshProUGUI killsText, TextMeshProUGUI teasuresText)
    {
        this.scoreText = scoreText;
        this.killsText = killsText;
        this.teasuresText = teasuresText;
    }
    public string ScoreText { get => scoreText.text; set => scoreText.text = value; }
    public string KillsText { get => killsText.text; set => killsText.text = value; }
    public string TeasuresText { get => teasuresText.text; set => teasuresText.text = value; }

}
