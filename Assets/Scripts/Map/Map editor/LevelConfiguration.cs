using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Game;
using TMPro;
using UnityEngine.UI;
using WEditor.UI;
using WEditor.Game.Player.Guns;

namespace WEditor.Scenario.Editor
{
    public class LevelConfiguration : MonoBehaviour
    {
        [SerializeField] TMP_Dropdown dropdown;
        [SerializeField] TMP_Dropdown gunsDropdown;
        public DifficultyTier difficultyMode = DifficultyTier.Easy;
        public void Dropdown_SelectDifficult()
        {
            string difficultTier = dropdown.options[dropdown.value].text;
            Enum.TryParse(difficultTier, out difficultyMode);
            DifficultyHandler.instance.HandleDifficulty(difficultyMode);
            DataHandler.difficultyTier = difficultyMode;
        }
        public void Dropdown_SelectGuns()
        {
            string guns = gunsDropdown.options[gunsDropdown.value].text;
            int[] gunIndexes = new int[] { };
            switch (guns)
            {
                case "Knife":
                    gunIndexes = new int[] { 0 };
                    break;
                case "Pistol":
                    gunIndexes = new int[] { 1 };
                    break;
                case "Machinegun":
                    gunIndexes = new int[] { 2 };
                    break;
                case "Heavy Machinegun":
                    gunIndexes = new int[] { 3 };
                    break;
                case "All":
                    gunIndexes = new int[] { 0, 1, 2, 3 };
                    break;
            }
            DataHandler.levelGuns = gunIndexes;
        }
    }
}
public enum DifficultyTier
{
    Easy, Medium, Hard
}
