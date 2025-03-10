using Critsoft.CozyShip.MainMenu.Controllers;
using System.Collections.Generic;
using System.Linq;
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

        #endregion

        #region Public Methods

        [Inject]
        public void Construct(MainMenuController controller)
        {
            _controller = controller;

            _controller.SettingsOpened += ToggleSettings;
            _controller.ScoreboardUpdated += OnScoreboardUpdated;
            _controller.MusicVolumeChanged += OnMusicVolumeChanged;
            _controller.SFXVolumeChanged += OnSFXVolumeChanged;
            _controller.FontChanged += OnFontChanged;

            _startButton.onClick.AddListener(_controller.StartGame);
            _settingsButton.onClick.AddListener(_controller.ToggleSettings);
            _exitButton.onClick.AddListener(_controller.ExitGame);
            _settingsBackButton.onClick.AddListener(_controller.ToggleSettings);

            _musicVolumeSlider.onValueChanged.AddListener(_controller.ChangeMusicVolume);
            _sfxVolumeSlider.onValueChanged.AddListener(_controller.ChangeSFXVolume);
            _fontDropdown.onValueChanged.AddListener(_controller.ChangeFont);
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
            _controller.SettingsOpened -= ToggleSettings;
            _controller.ScoreboardUpdated -= OnScoreboardUpdated;
            _controller.MusicVolumeChanged -= OnMusicVolumeChanged;
            _controller.SFXVolumeChanged -= OnSFXVolumeChanged;
            _controller.FontChanged -= OnFontChanged;

            _startButton.onClick.RemoveListener(_controller.StartGame);
            _settingsButton.onClick.RemoveListener(_controller.ToggleSettings);
            _exitButton.onClick.RemoveListener(_controller.ExitGame);
            _settingsBackButton.onClick.RemoveListener(_controller.ToggleSettings);

            _musicVolumeSlider.onValueChanged.RemoveListener(_controller.ChangeMusicVolume);
            _sfxVolumeSlider.onValueChanged.RemoveListener(_controller.ChangeSFXVolume);
            _fontDropdown.onValueChanged.RemoveListener(_controller.ChangeFont);
        }

        private void OnMusicVolumeChanged(float volume)
        {
            _musicVolumeSlider.value = volume;
        }

        private void OnSFXVolumeChanged(float volume)
        {
            _sfxVolumeSlider.value = volume;
        }

        private void OnFontChanged(int fontIndex, TMP_FontAsset newFont)
        {
            foreach (var textElement in _allTexts)
            {
                textElement.font = newFont;
            }
            _fontDropdown.value = fontIndex;
        }

        private void OnScoreboardUpdated(List<GameResult> results)
        {
            _scoreboardText.text = string.Join("\n", results.Select((r, i) => $"#{i + 1} - {r.AllCollisions} wrecks - {r.TotalElapsedTime:F1}s"));
        }

        #endregion
    }
}
