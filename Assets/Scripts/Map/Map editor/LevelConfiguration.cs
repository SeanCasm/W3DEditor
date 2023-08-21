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
        [SerializeField] TMP_Dropdown gunsDropdown, musicDropdown;
        private void OnEnable()
        {
            SetLevelMusic();
            SetInitialGuns();
        }
        private void SetInitialGuns()
        {
            gunsDropdown.value = DataHandler.levelGuns;
        }
        private void SetLevelMusic()
        {
            if (DataHandler.levelMusicTheme == "Get Them Before They Get You")
            {
                musicDropdown.value = 0;
            }
            else
            {
                for (int i = 0; i < musicDropdown.options.Count; i++)
                {
                    if (musicDropdown.options[i].text == DataHandler.levelMusicTheme)
                        musicDropdown.value = i;
                }
            }
        }
        public void Dropdown_LevelMusic()
        {
            string musicName = musicDropdown.options[musicDropdown.value].text;
            DataHandler.levelMusicTheme = musicName;
            Music.current.SetAnotherTheme(musicName);
        }
        public void Dropdown_SelectGuns()
        {
            DataHandler.levelGuns = gunsDropdown.value;
        }
    }
}