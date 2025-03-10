using UnityEngine;
using Zenject;
using Critsoft.CozyShip.MainMenu.Models;
using Critsoft.CozyShip.MainMenu.Views;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using TMPro;

namespace Critsoft.CozyShip.MainMenu.Controllers
{
    public class MainMenuController : MonoBehaviour
    {
        #region Events

        public event Action<List<GameResult>> ScoreboardUpdated;
        public event Action<bool> SettingsOpened;
        public event Action<float> MusicVolumeChanged;
        public event Action<float> SFXVolumeChanged;
        public event Action<int, TMP_FontAsset> FontChanged;

        #endregion

        #region Fields

        private MainMenuModel _model;
        private MainMenuView _view;
        private GameResultsStorage _resultsStorage;
        private TMP_FontAsset[] _availableFonts;

        #endregion

        #region Public Methods

        [Inject]
        public void Construct(MainMenuModel model, MainMenuView view, GameResultsStorage resultsStorage,
            [Inject(Id = GameConfig.AvailableFontsId)] TMP_FontAsset[] availableFonts)
        {
            _model = model;
            _view = view;
            _resultsStorage = resultsStorage;
            _availableFonts = availableFonts;

            _model.SettingsOpened += OnSettingsOpened;
            _model.MusicVolumeChanged += OnMusicVolumeChanged;
            _model.SFXVolumeChanged += OnSFXVolumeChanged;
            _model.FontChanged += OnFontChanged;
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

        public void ChangeMusicVolume(float volume)
        {
            _model.SetMusicVolume(volume);
            PlayerPrefs.SetFloat(GameConfig.MusicVolumeKey, volume);
            PlayerPrefs.Save();
        }

        public void ChangeSFXVolume(float volume)
        {
            _model.SetSFXVolume(volume);
            PlayerPrefs.SetFloat(GameConfig.SFXVolumeKey, volume);
            PlayerPrefs.Save();
        }

        public void ChangeFont(int fontIndex)
        {
            _model.SetFont(fontIndex);
            PlayerPrefs.SetInt(GameConfig.SelectedFontIndexKey, fontIndex);
            PlayerPrefs.Save();
        }

        #endregion

        #region Private Methods

        private void Awake()
        {
            LoadSettings();
            LoadScoreboard();
        }

        private void OnDestroy()
        {
            _model.SettingsOpened -= OnSettingsOpened;
            _model.MusicVolumeChanged -= OnMusicVolumeChanged;
            _model.SFXVolumeChanged -= OnSFXVolumeChanged;
            _model.FontChanged -= OnFontChanged;
        }

        private void LoadSettings()
        {
            float musicVolume = PlayerPrefs.GetFloat(GameConfig.MusicVolumeKey, GameConfig.DefaultMusicVolume);
            float sfxVolume = PlayerPrefs.GetFloat(GameConfig.SFXVolumeKey, GameConfig.DefaultSFXVolume);
            int fontIndex = PlayerPrefs.GetInt(GameConfig.SelectedFontIndexKey, GameConfig.DefaultFontIndex);

            _model.InitializeSettings(musicVolume, sfxVolume, fontIndex, _availableFonts);
        }

        private void LoadScoreboard()
        {
            List<GameResult> results = _resultsStorage.LoadResults();
            ScoreboardUpdated?.Invoke(results);
        }

        private void OnSettingsOpened(bool isOpen)
        {
            SettingsOpened?.Invoke(isOpen);
        }

        private void OnMusicVolumeChanged(float volume)
        {
            MusicVolumeChanged?.Invoke(volume);
        }

        private void OnSFXVolumeChanged(float volume)
        {
            SFXVolumeChanged?.Invoke(volume);
        }

        private void OnFontChanged(int fontIndex, TMP_FontAsset font)
        {
            FontChanged?.Invoke(fontIndex, font);
        }

        #endregion
    }
}
