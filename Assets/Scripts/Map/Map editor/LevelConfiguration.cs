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
        [SerializeField] GunHandler gunHandler;
        [SerializeField] TMP_Dropdown dropdown;
        [SerializeField] TMP_Dropdown gunsDropdown;
        public DifficultyTier difficultyMode = DifficultyTier.Easy;
        private void OnEnable()
        {
            gunHandler.initialAvailableGuns = new int[] { 0, 1, 2, 3 };
        }
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
            print(guns);
            switch (guns)
            {
                case "Knife":
                    gunHandler.initialAvailableGuns = new int[] { 0 };
                    break;
                case "Pistol":
                    gunHandler.initialAvailableGuns = new int[] { 1 };
                    break;
                case "Machinegun":
                    gunHandler.initialAvailableGuns = new int[] { 2 };
                    break;
                case "Heavy Machinegun":
                    gunHandler.initialAvailableGuns = new int[] { 3 };
                    break;
                case "All":
                    gunHandler.initialAvailableGuns = new int[] { 0, 1, 2, 3 };
                    break;
            }
        }
    }
}
public enum DifficultyTier
{
    Easy, Medium, Hard
}
