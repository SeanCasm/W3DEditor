using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Game;
using TMPro;
using UnityEngine.UI;
using WEditor.UI;

namespace WEditor.Scenario.Editor
{
    public class LevelConfiguration : MonoBehaviour
    {
        [SerializeField] TMP_Dropdown dropdown;
        public DifficultyTier difficultyMode = DifficultyTier.Easy;
        public void Dropdown_SelectDifficult()
        {
            string difficultTier = dropdown.options[dropdown.value].text;
            Enum.TryParse(difficultTier, out difficultyMode);
            DifficultyHandler.instance.HandleDifficulty(difficultyMode);
            DataHandler.difficultyTier = difficultyMode;
        }
    }
}
public enum DifficultyTier
{
    Easy, Medium, Hard
}
