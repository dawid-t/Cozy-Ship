using UnityEngine;
using Zenject;
using Critsoft.CozyShip.MainMenu.Models;
using Critsoft.CozyShip.MainMenu.Views;
using UnityEngine.SceneManagement;
using System;

namespace Critsoft.CozyShip.MainMenu.Controllers
{
    public class MainMenuController : MonoBehaviour
    {
        #region Events

        public event Action<bool> SettingsOpened;

        #endregion

        #region Fields

        private MainMenuModel _model;
        private MainMenuView _view;

        #endregion

        #region Public Methods

        [Inject]
        public void Construct(MainMenuModel model, MainMenuView view)
        {
            _model = model;
            _view = view;

            _model.SettingsOpened += OnSettingsOpened;
        }

        public void StartGame()
        {
            SceneManager.LoadScene(SceneNames.Gameplay);
        }

        public void ToggleSettings()
        {
            _model.ToggleSettings();
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        #endregion

        #region Private Methods

        private void OnDestroy()
        {
            if (_model != null)
            {
                _model.SettingsOpened -= OnSettingsOpened;
            }
        }

        private void OnSettingsOpened(bool isOpen)
        {
            SettingsOpened.Invoke(isOpen);
        }

        #endregion
    }
}
