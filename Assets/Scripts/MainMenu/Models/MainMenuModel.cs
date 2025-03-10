using System;
using TMPro;

namespace Critsoft.CozyShip.MainMenu.Models
{
    public class MainMenuModel
    {
        #region Events

        public event Action<bool> SettingsOpened;
        public event Action<float> MusicVolumeChanged;
        public event Action<float> SFXVolumeChanged;
        public event Action<int, TMP_FontAsset> FontChanged;

        #endregion

        #region Fields

        private bool _isSettingsOpen;
        private TMP_FontAsset _currentFont;
        private TMP_FontAsset[] _availableFonts;
        private float _musicVolume;
        private float _sfxVolume;
        private int _fontIndex;

        #endregion

        #region Public Methods

        public void ToggleSettings()
        {
            _isSettingsOpen = !_isSettingsOpen;
            SettingsOpened?.Invoke(_isSettingsOpen);
        }

        public void InitializeSettings(float musicVolume, float sfxVolume, int fontIndex, TMP_FontAsset[] fonts)
        {
            _availableFonts = fonts;
            _musicVolume = musicVolume;
            _sfxVolume = sfxVolume;
            _fontIndex = fontIndex;

            SetMusicVolume(_musicVolume);
            SetSFXVolume(_sfxVolume);
            SetFont(_fontIndex);
        }

        public void SetMusicVolume(float volume)
        {
            _musicVolume = volume;
            MusicVolumeChanged?.Invoke(volume);
        }

        public void SetSFXVolume(float volume)
        {
            _sfxVolume = volume;
            SFXVolumeChanged?.Invoke(volume);
        }

        public void SetFont(int fontIndex)
        {
            if (_availableFonts != null && fontIndex >= 0 && fontIndex < _availableFonts.Length)
            {
                _currentFont = _availableFonts[fontIndex];
                FontChanged?.Invoke(fontIndex, _currentFont);
            }
        }

        #endregion
    }
}
