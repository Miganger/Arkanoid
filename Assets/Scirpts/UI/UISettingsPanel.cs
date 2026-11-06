using Saving;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UISettingsPanel : MonoBehaviour
    {
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private Slider musicSlider;

        private float sfxVolume = 0;
        private float musicVolume = 0;

        public void Open()
        {
            sfxVolume = SaveSystem.Data.SFXVolume;
            musicVolume = SaveSystem.Data.MusicVolume;

            sfxSlider.value = sfxVolume;
            musicSlider.value = musicVolume;

            gameObject.SetActive(true);
        }
        public void Close()
        {
            gameObject.SetActive(false);
        }

        public void OnSFXValueChange(float value)
        {
            sfxVolume = value;
        }
        public void OnMusicValueChange(float value)
        {
            musicVolume = value;
        }

        public void OnSave()
        {
            SaveSystem.Data.SFXVolume = sfxVolume;
            SaveSystem.Data.MusicVolume = musicVolume;
            SaveSystem.Save();

            Close();
        }
    }
}