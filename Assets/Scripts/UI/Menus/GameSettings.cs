using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;
using WEditor.CameraUtils;
using WEditor.Game.Player;
using WEditor.Input;

namespace WEditor
{
    public class GameSettings : MonoBehaviour
    {
        [Range(0, 5)]
        [SerializeField] float defaultAimSensibility;
        [Range(0.0001f, 1)]
        [SerializeField] float defaultFxVolume, defaultMusicVolume;
        [SerializeField] Slider aimSlider, fxSlider, musicSlider;
        [SerializeField] Toggle fullScreen;
        [SerializeField] AudioMixerGroup fxGroup, musicGroup;
        [SerializeField] AudioSource sampleFxSource, sampleMusicSource;
        private void Start()
        {
            Screen.SetResolution(800, 600, FullScreenMode.Windowed);
            Application.targetFrameRate = 60;
            aimSlider.value = PlayerPrefs.HasKey("aim") ? PlayerPrefs.GetFloat("aim") : defaultAimSensibility;
            fxSlider.value = PlayerPrefs.HasKey("fx") ? PlayerPrefs.GetFloat("fx") : defaultFxVolume;
            musicSlider.value = PlayerPrefs.HasKey("music") ? PlayerPrefs.GetFloat("music") : defaultMusicVolume;
            SetFX(fxSlider.value);
            SetMusic(musicSlider.value);
            EditorCamera.currentSpeed = PlayerPrefs.GetFloat("camSpeed");
            PlayerController.currentRotationSpeed = PlayerPrefs.HasKey("aim") ? PlayerPrefs.GetFloat("aim") : defaultAimSensibility;
            LoadFullscreenData();
        }
        private void LoadFullscreenData()
        {
            bool fs = false;
            if (PlayerPrefs.HasKey("fullscreen"))
            {
                fs = PlayerPrefs.GetInt("fullscreen") == 0;
                SetFullscreen(fs);
                Screen.SetResolution(800, 600, fs);
            }
            else
            {
                Screen.SetResolution(800, 600, false);
                SetFullscreen(false);
            }
            print(fs);
            fullScreen.isOn = fs;
        }
        private void SetMusic(float amount)
        {
            float correctValue = Mathf.Log10(amount) * 20;
            musicGroup.audioMixer.SetFloat("music", correctValue);
        }
        private void SetFX(float amount)
        {
            float correctValue = Mathf.Log10(fxSlider.value) * 20;
            fxGroup.audioMixer.SetFloat("fx", correctValue);
        }
        /// <summary>
        /// Set the mouse aim sensibility.
        /// </summary>
        /// <param name="aim">amount of sensibility</param>
        public void SetAimSensibility(float aim)
        {
            PlayerController.currentRotationSpeed = aim;
            PlayerPrefs.SetFloat("aim", aim);
        }
        public void SetFullscreen(bool value)
        {
            Screen.fullScreen = value;
            PlayerPrefs.SetInt("fullscreen", value ? 0 : 1);
        }
        /// <summary>
        /// Set the editor camera speed.
        /// </summary>
        /// <param name="speed">amount of speed</param>/
        public void SetCameraSpeed(float speed)
        {
            EditorCamera.currentSpeed = speed;
            PlayerPrefs.SetFloat("camSpeed", speed);
        }
        public void SetSoundEffectsVolume(float amount)
        {
            SetFX(amount);
            PlayerPrefs.SetFloat("fx", amount);
            if (!sampleFxSource.isPlaying)
                sampleFxSource.Play();
        }
        public void SetMusicVolume(float amount)
        {
            SetMusic(amount);
            PlayerPrefs.SetFloat("music", amount);
            if (!sampleMusicSource.isPlaying)
                sampleMusicSource.Play();
        }
    }
}
