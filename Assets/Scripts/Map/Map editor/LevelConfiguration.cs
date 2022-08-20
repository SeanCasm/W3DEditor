using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Game;
using TMPro;

namespace WEditor.Scenario.Editor
{
    public class LevelConfiguration : MonoBehaviour
    {
        [SerializeField] TMP_Dropdown dropdown;
        [SerializeField] TMP_Dropdown gunsDropdown;
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
                    gunIndexes = new int[] { 0, 1 };
                    break;
                case "Machinegun":
                    gunIndexes = new int[] { 0, 2 };
                    break;
                case "Heavy Machinegun":
                    gunIndexes = new int[] { 0, 3 };
                    break;
                case "All":
                    gunIndexes = new int[] { 0, 1, 2, 3 };
                    break;
            }
            DataHandler.levelGuns = gunIndexes;
        }
    }
}