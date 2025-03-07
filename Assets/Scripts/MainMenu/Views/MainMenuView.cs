using Critsoft.CozyShip.MainMenu.Controllers;
using System.Collections.Generic;
using TMPro;
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
        [SerializeField] private TMP_Text _scoreboardText;

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
            _controller.ScoreboardUpdated += UpdateScoreboard;

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
                _controller.ScoreboardUpdated -= UpdateScoreboard;
            }

            _startButton.onClick.RemoveListener(_controller.StartGame);
            _settingsButton.onClick.RemoveListener(_controller.ToggleSettings);
            _exitButton.onClick.RemoveListener(_controller.ExitGame);
            _settingsBackButton.onClick.RemoveListener(_controller.ToggleSettings);
        }

        private void UpdateScoreboard(List<GameResult> results)
        {
            _scoreboardText.text = "";
            for (int i = 0; i < results.Count; i++)
            {
                _scoreboardText.text += $"#{i + 1} - {results[i].AllCollisions} wrecks - {results[i].TotalElapsedTime:F1}s\n";
            }
        }

        private void OnSettingsOpened(bool isOpen)
        {
            ToggleSettings(isOpen);
        }

        #endregion
    }
}
