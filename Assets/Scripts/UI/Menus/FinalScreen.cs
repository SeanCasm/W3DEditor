using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using WEditor.Events;
using UnityEngine.InputSystem;
namespace WEditor.UI
{
    /// <summary>
    /// Handle the level completed screen.
    /// </summary>
    public class FinalScreen : MonoBehaviour
    {
        [SerializeField] InfoStatsUIText infoStats;
        private bool exit;
        private void OnEnable()
        {
            GameplayEvent.instance.LevelCompleted(infoStats);
            GameInput.instance.DisableAll();
        }
        private void Update()
        {
            if (Keyboard.current.anyKey.wasPressedThisFrame && !exit)
            {
                SceneHandler.instance.LoadMain();
                exit = true;
            }
        }
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
