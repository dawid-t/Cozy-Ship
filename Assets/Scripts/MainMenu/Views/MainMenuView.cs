using Critsoft.CozyShip.MainMenu.Controllers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Critsoft.CozyShip.MainMenu.Views
{
    public class MainMenuView : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private Canvas _menuCanvas;
        [SerializeField] private Canvas _settingsCanvas;
        [Space]
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _settingsBackButton;

        #endregion

        #region Fields

        private MainMenuController _controller;

        #endregion

        #region Public Methods

        [Inject]
        public void Construct(MainMenuController controller)
        {
            _controller = controller;

            _controller.SettingsOpened += OnSettingsOpened;
            
            _startButton.onClick.AddListener(_controller.StartGame);
            _settingsButton.onClick.AddListener(_controller.ToggleSettings);
            _exitButton.onClick.AddListener(_controller.ExitGame);
            _settingsBackButton.onClick.AddListener(_controller.ToggleSettings);
        }

        public void ToggleSettings(bool isOpen)
        {
            _menuCanvas.enabled = !isOpen;
            _settingsCanvas.enabled = isOpen;
            
            EventSystem.current.SetSelectedGameObject(null);

            (isOpen ? _settingsBackButton : _settingsButton).Select();
        }

        #endregion

        #region Private Methods
        
        private void OnDestroy()
        {
            if (_controller != null)
            {
                _controller.SettingsOpened -= OnSettingsOpened;
            }

            _startButton.onClick.RemoveListener(_controller.StartGame);
            _settingsButton.onClick.RemoveListener(_controller.ToggleSettings);
            _exitButton.onClick.RemoveListener(_controller.ExitGame);
            _settingsBackButton.onClick.RemoveListener(_controller.ToggleSettings);
        }
        
        private void OnSettingsOpened(bool isOpen)
        {
            ToggleSettings(isOpen);
        }

        #endregion
    }
}
