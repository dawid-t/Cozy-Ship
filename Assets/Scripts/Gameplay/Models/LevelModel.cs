using UnityEngine;
using System;

namespace Critsoft.CozyShip.Gameplay.Models
{
    public class LevelModel
    {
        #region Events

        public event Action AllLevelsCompleted;
        public event Action<int, PlayerStatsEventArgs> LevelCompleted;
        public event Action<LevelInitiatedEventArgs> LevelInitiated;

        #endregion

        #region Fields

        private bool _isLevelComplete;
        private int _currentLevelIndex;

        private LevelDataSO _currentLevelData;
        private LevelDataSO[] _levels;

        #endregion

        #region Properties

        public bool IsLevelComplete => _isLevelComplete;
        public int CurrentLevelIndex => _currentLevelIndex;
        
        public int LevelId => _currentLevelData.LevelId;
        public int PointsLimit => _currentLevelData.PointsLimit;
        public float SceneScrollSpeed => _currentLevelData.SceneScrollSpeed;

        public int CoinMinSpawnTime => _currentLevelData.CoinMinSpawnTime;
        public int CoinMaxSpawnTime => _currentLevelData.CoinMaxSpawnTime;
        public GameObject[] CoinsToSpawn => _currentLevelData.CoinsToSpawn;
        
        public int ObstacleMinSpawnTime => _currentLevelData.ObstacleMinSpawnTime;
        public int ObstacleMaxSpawnTime => _currentLevelData.ObstacleMaxSpawnTime;
        public GameObject[] ObstaclesToSpawn => _currentLevelData.ObstaclesToSpawn;

        #endregion

        public void Initialize(LevelDataSO[] levels)
        {
            _levels = levels;
            _isLevelComplete = false;
            _currentLevelIndex = 0;
            _currentLevelData = _levels[_currentLevelIndex];

            LevelInitiatedEventArgs eventArgs = GetLevelInitiatedEventArgs();
            LevelInitiated?.Invoke(eventArgs);
        }

        #region Public Methods

        public void CompleteLevel(PlayerStatsEventArgs eventArgs)
        {
            _isLevelComplete = true;
            LevelCompleted?.Invoke(LevelId, eventArgs);
        }

        public bool LoadNextLevel()
        {
            if (_currentLevelIndex < _levels.Length - 1)
            {
                _isLevelComplete = false;
                _currentLevelIndex++;
                _currentLevelData = _levels[_currentLevelIndex];

                LevelInitiatedEventArgs eventArgs = GetLevelInitiatedEventArgs();
                LevelInitiated?.Invoke(eventArgs);
                return true;
            }
            else
            {
                AllLevelsCompleted?.Invoke();
                return false;
            }
        }

        #endregion

        #region Private Methods

        private LevelInitiatedEventArgs GetLevelInitiatedEventArgs()
        {
            LevelInitiatedEventArgs eventArgs = new LevelInitiatedEventArgs(
                LevelId,
                PointsLimit,
                SceneScrollSpeed,
                CoinMinSpawnTime,
                CoinMaxSpawnTime,
                CoinsToSpawn,
                ObstacleMinSpawnTime,
                ObstacleMaxSpawnTime,
                ObstaclesToSpawn);

            return eventArgs;
        }

        #endregion
    }
}
