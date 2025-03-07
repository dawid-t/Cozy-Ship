using UnityEngine;
using System;

namespace Critsoft.CozyShip.Gameplay.Models
{
    public class GameplayModel
    {
        #region Events

        public event Action<PlayerStatsEventArgs> PointsLimitReached;
        public event Action<int> PointsUpdated;
        public event Action<int> PointsLimitUpdated;
        public event Action<int> CollisionsUpdated;
        public event Action<float> TimeUpdated;
        public event Action<bool> PauseStateChanged;

        #endregion

        #region Fields

        private int _points = 0;
        private int _pointsLimit = 1;
        private int _collisions = 0;
        private float _elapsedTime = 0f;
        private bool _isPaused = false;

        private int _allPoints = 0;
        private int _allCollisions = 0;
        private float _totalElapsedTime = 0f;

        #endregion

        #region Properties

        public int Points
        {
            get => _points;
            set
            {
                _points = Mathf.Clamp(value, 0, _pointsLimit);
                PointsUpdated?.Invoke(_points);
                if (_points >= _pointsLimit)
                {
                    UpdateTotalStats(_points, _collisions, _elapsedTime);
                    PointsLimitReached?.Invoke(GetPlayerStatsEventArgs());
                }
            }
        }

        public int PointsLimit
        {
            get => _pointsLimit;
            set
            {
                _pointsLimit = Mathf.Max(1, value);
                PointsLimitUpdated?.Invoke(_pointsLimit);
            }
        }

        public int Collisions
        {
            get => _collisions;
            set
            {
                _collisions = value;
                CollisionsUpdated?.Invoke(_collisions);
            }
        }

        public float ElapsedTime
        {
            get => _elapsedTime;
            set
            {
                _elapsedTime = value;
                TimeUpdated?.Invoke(_elapsedTime);
            }
        }

        public bool IsPaused
        {
            get => _isPaused;
            set
            {
                _isPaused = value;
                PauseStateChanged?.Invoke(_isPaused);
            }
        }

        public int AllPoints => _allPoints;
        public int AllCollisions => _allCollisions;
        public float TotalElapsedTime => _totalElapsedTime;

        #endregion

        #region Public Methods

        public void ResetGameplay()
        {
            _points = 0;
            _collisions = 0;
            _elapsedTime = 0;
            _isPaused = false;

            PointsUpdated?.Invoke(_points);
            CollisionsUpdated?.Invoke(_collisions);
            TimeUpdated?.Invoke(_elapsedTime);
            PauseStateChanged?.Invoke(_isPaused);
        }
        
        #endregion

        #region Private Methods

        private PlayerStatsEventArgs GetPlayerStatsEventArgs()
        {
            return new PlayerStatsEventArgs(PointsLimit, Collisions, ElapsedTime);
        }

        private void UpdateTotalStats(int additionalPoints, int additionalCollisions, float additionalElapsedTime)
        {
            _allPoints += additionalPoints;
            _allCollisions += additionalCollisions;
            _totalElapsedTime += additionalElapsedTime;
        }

        #endregion
    }
}
