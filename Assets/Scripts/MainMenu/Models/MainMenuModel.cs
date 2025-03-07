using System;
using TMPro;
using UnityEngine;

namespace Critsoft.CozyShip.MainMenu.Models
{
    public class MainMenuModel
    {
        #region Events

        public event Action<bool> SettingsOpened;
        public event Action<TMP_FontAsset> FontChanged;

        #endregion

        #region Fields

        private bool _isSettingsOpen;
        private TMP_FontAsset _currentFont;
        private TMP_FontAsset[] _availableFonts;

        #endregion

        #region Public Methods

        public void ToggleSettings()
        {
            _isSettingsOpen = !_isSettingsOpen;
            SettingsOpened?.Invoke(_isSettingsOpen);
        }

        public void InitializeFonts(TMP_FontAsset[] fonts)
        {
            _availableFonts = fonts;
            int savedFontIndex = PlayerPrefs.GetInt(GameConfig.SelectedFontIndexKey, 0);
            _currentFont = _availableFonts[savedFontIndex];

            FontChanged?.Invoke(_currentFont);
        }

        public void SetFont(int index)
        {
            if (index >= 0 && index < _availableFonts.Length)
            {
                _currentFont = _availableFonts[index];
                PlayerPrefs.SetInt(GameConfig.SelectedFontIndexKey, index);
                PlayerPrefs.Save();
                FontChanged?.Invoke(_currentFont);
            }
        }

        public TMP_FontAsset GetCurrentFont()
        {
            return _currentFont;
        }

        #endregion
    }
}
