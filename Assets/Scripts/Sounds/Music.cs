using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

namespace WEditor.Game
{
    public class Music : MonoBehaviour
    {
        public static Music current;
        [SerializeField] AudioSource audioSource;
        [SerializeField] AudioClip tileTheme, levelTheme, editorTheme, endLevelTheme;
        [SerializeField] AudioClip[] allThemes;
        private Dictionary<string, AudioClip> music = new Dictionary<string, AudioClip>();
        private AudioClip currentLevelTheme;
        private void OnEnable()
        {
            if (!current)
            {
                current = this;
                DontDestroyOnLoad(gameObject);
                currentLevelTheme = levelTheme;
                music = allThemes.ToDictionary(k => k.name, v => v);
            }
            else
            {
                Destroy(gameObject);
            }
            int index = SceneManager.GetActiveScene().buildIndex;
            if (index == 0 || index == 1)
                SetTitleTheme();
        }

        public void SetAnotherTheme(string themeName)
        {
            AudioClip current = music[themeName];
            currentLevelTheme = current;
            audioSource.clip = current;
            audioSource.Play();
        }
        public void SetLevelEndTheme()
        {
            audioSource.clip = endLevelTheme;
            audioSource.Play();
            currentLevelTheme = levelTheme;
        }
        public void SetTitleTheme()
        {
            if (audioSource.clip.name == tileTheme.name) return;
            audioSource.clip = tileTheme;
            audioSource.Play();
            currentLevelTheme = levelTheme;
        }
        public void SetLevelTheme()
        {
            audioSource.clip = currentLevelTheme;
            audioSource.Play();
        }
        public void SetEditorTheme()
        {
            audioSource.clip = editorTheme;
            audioSource.Play();
            currentLevelTheme = levelTheme;
        }
    }
}
