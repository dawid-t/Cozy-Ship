using System;

namespace Critsoft.CozyShip.MainMenu.Models
{
    public class MainMenuModel
    {
        #region Events

        public event Action<bool> SettingsOpened;

        #endregion

        #region Fields

        private bool _isSettingsOpen;

        #endregion

        #region Public Methods

        public void ToggleSettings()
        {
            _isSettingsOpen = !_isSettingsOpen;
            SettingsOpened?.Invoke(_isSettingsOpen);
        }

        #endregion
    }
}
