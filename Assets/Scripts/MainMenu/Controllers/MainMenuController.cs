using UnityEngine;
using Zenject;
using Critsoft.CozyShip.MainMenu.Models;
using Critsoft.CozyShip.MainMenu.Views;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using Critsoft.CozyShip.Gameplay;
using TMPro;

namespace Critsoft.CozyShip.MainMenu.Controllers
{
    public class MainMenuController : MonoBehaviour
    {
        #region Events

        public event Action<bool> SettingsOpened;
        public event Action<List<GameResult>> ScoreboardUpdated;
        public event Action<TMP_FontAsset> FontChanged;

        #endregion

        #region Fields

        private MainMenuModel _model;
        private MainMenuView _view;
        private GameResultsStorage _resultsStorage;

        #endregion

        #region Public Methods

        [Inject]
        public void Construct(MainMenuModel model, MainMenuView view, GameResultsStorage resultsStorage)
        {
            _model = model;
            _view = view;
            _resultsStorage = resultsStorage;

            _model.SettingsOpened += OnSettingsOpened;
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

        public void ChangeMusicVolume(float volume)
        {
            AmbientAudioManager.Volume = volume;
            PlayerPrefs.SetFloat(GameConfig.MusicVolumeKey, volume);
            PlayerPrefs.Save();
        }

        public void ChangeSFXVolume(float volume)
        {
            SFXAudioManager.Volume = volume;
            PlayerPrefs.SetFloat(GameConfig.SFXVolumeKey, volume);
            PlayerPrefs.Save();
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        public void InitializeFonts(TMP_FontAsset[] fonts)
        {
            _model.InitializeFonts(fonts);
        }

        public void ChangeFont(int index)
        {
            _model.SetFont(index);
        }

        #endregion

        #region Private Methods

        private void Awake()
        {
            LoadScoreboard();
        }

        private void OnDestroy()
        {
            if (_model != null)
            {
                _model.SettingsOpened -= OnSettingsOpened;
                _model.FontChanged -= OnFontChanged;
            }
        }

        private void LoadScoreboard()
        {
            List<GameResult> results = _resultsStorage.LoadResults();
            ScoreboardUpdated?.Invoke(results);
        }

        private void OnSettingsOpened(bool isOpen)
        {
            SettingsOpened.Invoke(isOpen);
        }

        private void OnFontChanged(TMP_FontAsset font)
        {
            FontChanged?.Invoke(font);
        }

        #endregion
    }
}
