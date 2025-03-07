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
        [SerializeField] private TMP_Text _scoreboardText;
        [Space]
        [SerializeField] private Slider _musicVolumeSlider;
        [SerializeField] private Slider _sfxVolumeSlider;
        [SerializeField] private Button _settingsBackButton;
        [SerializeField] private TMP_Dropdown _fontDropdown;
        [SerializeField] private TMP_Text[] _allTexts;

        #endregion

        #region Fields

        private MainMenuController _controller;
        private TMP_FontAsset[] _availableFonts;

        #endregion

        #region Public Methods

        [Inject]
        public void Construct(MainMenuController controller, [Inject(Id = GameConfig.AvailableFontsId)] TMP_FontAsset[] fonts)
        {
            _controller = controller;
            _availableFonts = fonts;

            _controller.SettingsOpened += OnSettingsOpened;
            _controller.ScoreboardUpdated += UpdateScoreboard;
            _controller.FontChanged += UpdateFont;

            _startButton.onClick.AddListener(_controller.StartGame);
            _settingsButton.onClick.AddListener(_controller.ToggleSettings);
            _exitButton.onClick.AddListener(_controller.ExitGame);
            _settingsBackButton.onClick.AddListener(_controller.ToggleSettings);

            _musicVolumeSlider.onValueChanged.AddListener(_controller.ChangeMusicVolume);
            _sfxVolumeSlider.onValueChanged.AddListener(_controller.ChangeSFXVolume);
            _fontDropdown.onValueChanged.AddListener(_controller.ChangeFont);

            _controller.InitializeFonts(_availableFonts);
        }

        public void ToggleSettings(bool isOpen)
        {
            _menuCanvas.enabled = !isOpen;
            _settingsCanvas.enabled = isOpen;
            
            EventSystem.current.SetSelectedGameObject(null);

            (isOpen ? _settingsBackButton : _settingsButton).Select();
        }

        public void UpdateFont(TMP_FontAsset newFont)
        {
            foreach (var textElement in _allTexts)
            {
                textElement.font = newFont;
            }
        }

        #endregion

        #region Private Methods

        private void OnDestroy()
        {
            if (_controller != null)
            {
                _controller.SettingsOpened -= OnSettingsOpened;
                _controller.ScoreboardUpdated -= UpdateScoreboard;
                _controller.FontChanged -= UpdateFont;
            }

            _startButton.onClick.RemoveListener(_controller.StartGame);
            _settingsButton.onClick.RemoveListener(_controller.ToggleSettings);
            _exitButton.onClick.RemoveListener(_controller.ExitGame);
            _settingsBackButton.onClick.RemoveListener(_controller.ToggleSettings);

            _musicVolumeSlider.onValueChanged.RemoveListener(_controller.ChangeMusicVolume);
            _sfxVolumeSlider.onValueChanged.RemoveListener(_controller.ChangeSFXVolume);
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
