using Zenject;
using Critsoft.CozyShip.Gameplay.Models;
using Critsoft.CozyShip.Gameplay.Views;
using System;
using System.Linq;
using UnityEngine;

namespace Critsoft.CozyShip.Gameplay.Controllers
{
    public class LevelController : IInitializable, IDisposable
    {
        #region Events

        public event Action<int, PlayerStatsEventArgs> LevelCompleted;
        public event Action<LevelInitiatedEventArgs> LevelInitiated;

        #endregion

        #region Fields

        private LevelModel _model;
        private LevelView _view;
        private GameplayController _gameplayController;
        private LevelsLibrarySO _levelsLibrary;

        #endregion

        #region Public Methods

        [Inject]
        public LevelController(LevelModel levelModel, LevelView levelView, GameplayController gameplayController, LevelsLibrarySO levelsLibrary)
        {
            _model = levelModel;
            _view = levelView;
            _gameplayController = gameplayController;
            _levelsLibrary = levelsLibrary;
        }

        public void Initialize()
        {
            _model.LevelCompleted += OnLevelCompleted;
            _model.LevelInitiated += OnLevelInitiated;
            _gameplayController.PointsLimitReached += OnPointsLimitReached;

            _model.Initialize(_levelsLibrary.Levels.ToArray());
        }

        public void Dispose()
        {
            _model.LevelCompleted -= OnLevelCompleted;
            _model.LevelInitiated -= OnLevelInitiated;
            _gameplayController.PointsLimitReached -= OnPointsLimitReached;
        }

        public void LoadNextLevel()
        {
            _model.LoadNextLevel();
        }
        
        public int GetPointsLimit()
        {
            return _model.PointsLimit;
        }

        #endregion

        #region Private Methods

        private void OnPointsLimitReached(PlayerStatsEventArgs eventArgs)
        {
            _model.CompleteLevel(eventArgs);
        }

        private void OnLevelCompleted(int levelId, PlayerStatsEventArgs eventArgs)
        {
            Time.timeScale = 0;
            LevelCompleted?.Invoke(levelId, eventArgs);
        }

        private void OnLevelInitiated(LevelInitiatedEventArgs eventArgs)
        {
            Time.timeScale = 1;
            LevelInitiated?.Invoke(eventArgs);
        }

        #endregion
    }
}
