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
        [SerializeField] TextMeshProUGUI scoreText, killsText, teasuresText;
        private bool exit;
        private void OnEnable()
        {
            GameInput.instance.DisableAll();
            int score = DataHandler.levelStats.totalScore;
            int kills = DataHandler.levelStats.totalKills;
            int teasures = DataHandler.levelStats.totalTeasures;
            string levelName = DataHandler.currentLevelName;

            bool hasScoreSaved = PlayerPrefs.HasKey($"{levelName}-Score");
            bool hasKillsSaved = PlayerPrefs.HasKey($"{levelName}-Kills");
            bool hasTeasuresSaved = PlayerPrefs.HasKey($"{levelName}-Teasures");
            scoreText.text = $"Score: {score}";
            killsText.text = $"Kills: {kills}";
            teasuresText.text = $"Teasures: {teasures}";
            if (hasScoreSaved)
            {
                int scoreSaved = PlayerPrefs.GetInt($"{levelName}-Score");

                if (score > scoreSaved)
                {
                    PlayerPrefs.SetInt($"{levelName}-Score", score);
                    scoreText.text += " - Best: " + scoreSaved;
                }
            }
            else
            {
                PlayerPrefs.SetInt($"{levelName}-Score", score);
            }
            if (hasKillsSaved)
            {
                int killsSaved = PlayerPrefs.GetInt($"{levelName}-Kills");

                if (score > killsSaved)
                {
                    PlayerPrefs.SetInt($"{levelName}-Kills", kills);
                    killsText.text += " - Best: " + killsSaved;
                }
            }
            else
            {
                PlayerPrefs.SetInt($"{levelName}-Kills", kills);
            }
            if (hasTeasuresSaved)
            {
                int teasuresSaved = PlayerPrefs.GetInt($"{levelName}-Teasures");

                if (score > teasuresSaved)
                {
                    PlayerPrefs.SetInt($"{levelName}-Teasures", teasures);
                    teasuresText.text += " - Best: " + teasuresSaved;
                }
            }
            else
            {
                PlayerPrefs.SetInt($"{levelName}-Teasures", teasures);
            }
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