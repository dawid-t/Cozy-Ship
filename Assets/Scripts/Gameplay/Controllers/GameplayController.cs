using System;
using UnityEngine;
using Zenject;
using Critsoft.CozyShip.Gameplay.Models;
using Critsoft.CozyShip.Gameplay.Views;
using UnityEngine.InputSystem;
using Critsoft.CozyShip.Gameplay.Player;

namespace Critsoft.CozyShip.Gameplay.Controllers
{
    public class GameplayController : MonoBehaviour
    {
        #region Events

        public event Action<PlayerStatsEventArgs> PointsLimitReached;
        public event Action<int> PointsUpdated;
        public event Action<int> PointsLimitUpdated;
        public event Action<int> CollisionsUpdated;
        public event Action<float> TimeUpdated;
        public event Action<bool> PauseStateChanged;
        public event Action<LevelInitiatedEventArgs> LevelInitiated;

        #endregion

        #region Serialized Fields

        [SerializeField] private InputActionReference _pauseInputAction;

        #endregion

        #region Fields

        private GameplayModel _model;
        private GameplayView _view;
        private LevelController _levelController;
        private ShipCollisionHandler _shipCollisionHandler;

        #endregion

        #region Public Methods

        [Inject]
        public void Construct(GameplayModel model, GameplayView view, LevelController levelController, ShipCollisionHandler shipCollisionHandler)
        {
            _model = model;
            _view = view;
            _levelController = levelController;
            _shipCollisionHandler = shipCollisionHandler;

            _model.PointsLimitReached += OnPointsLimitReached;
            _model.PointsUpdated += OnPointsUpdated;
            _model.PointsLimitUpdated += OnPointsLimitUpdated;
            _model.CollisionsUpdated += OnCollisionsUpdated;
            _model.TimeUpdated += OnTimeUpdated;
            _model.PauseStateChanged += OnPauseStateChanged;

            _levelController.LevelInitiated += OnLevelInitiated;
            _shipCollisionHandler.CollisionOccurred += RegisterCollision;
        }

        public void AddPoint()
        {
            _model.Points++;
        }

        public void AddPoints(int additionalPoints)
        {
            _model.Points += additionalPoints;
        }

        public void ResetPoints()
        {
            _model.Points = 0;
        }

        public void UpdatePointsLimit()
        {
            _model.PointsLimit = _levelController.GetPointsLimit();
        }

        public void RegisterCollision()
        {
            _model.Collisions++;
            ResetPoints();
        }

        public void TogglePause()
        {
            _model.IsPaused = !_model.IsPaused;
            Time.timeScale = _model.IsPaused ? 0 : 1;
            PauseStateChanged?.Invoke(_model.IsPaused);
        }

        public void RestartLevel()
        {
            Time.timeScale = 1;
            _model.ResetGameplay();
            UnityEngine.SceneManagement.SceneManager.LoadScene(SceneNames.Gameplay);
        }

        public void ExitToMenu()
        {
            Time.timeScale = 1;
            UnityEngine.SceneManagement.SceneManager.LoadScene(SceneNames.MainMenu);
        }

        public void ExitToDesktop()
        {
            Application.Quit();
        }

        #endregion

        #region Private Methods

        private void Start()
        {
            InitHUDValues();
        }

        private void OnEnable()
        {
            _pauseInputAction.action.performed += OnPausePerformed;
            _pauseInputAction.action.Enable();
        }

        private void OnDisable()
        {
            _pauseInputAction.action.performed -= OnPausePerformed;
            _pauseInputAction.action.Disable();
        }

        private void Update()
        {
            _model.ElapsedTime += Time.deltaTime;
        }

        private void OnDestroy()
        {
            if (_model != null)
            {
                _model.PointsLimitReached -= OnPointsLimitReached;
                _model.PointsUpdated -= OnPointsUpdated;
                _model.PointsLimitUpdated -= OnPointsLimitUpdated;
                _model.CollisionsUpdated -= OnCollisionsUpdated;
                _model.TimeUpdated -= OnTimeUpdated;
                _model.PauseStateChanged -= OnPauseStateChanged;
            }

            if (_levelController != null)
            {
                _levelController.LevelInitiated -= OnLevelInitiated;
            }

            if (_shipCollisionHandler != null)
            {
                _shipCollisionHandler.CollisionOccurred -= RegisterCollision;
            }
        }

        private void InitHUDValues()
        {
            UpdatePointsLimit();
            _model.Points = 0;
            _model.Collisions = 0;
            _model.ElapsedTime = 0f;
        }

        private void OnPausePerformed(InputAction.CallbackContext context)
        {
            TogglePause();
        }

        private void OnPointsLimitReached(PlayerStatsEventArgs eventArgs)
        {
            PointsLimitReached?.Invoke(eventArgs);
        }

        private void OnPointsUpdated(int points)
        {
            PointsUpdated?.Invoke(points);
        }

        private void OnPointsLimitUpdated(int points)
        {
            PointsLimitUpdated?.Invoke(points);
        }

        private void OnCollisionsUpdated(int collisions)
        {
            CollisionsUpdated?.Invoke(collisions);
        }

        private void OnTimeUpdated(float time)
        {
            TimeUpdated?.Invoke(time);
        }
        
        private void OnPauseStateChanged(bool isPaused)
        {
            PauseStateChanged?.Invoke(isPaused);
        }

        private void OnLevelInitiated(LevelInitiatedEventArgs eventArgs)
        {
            ResetGameplay(eventArgs);
            LevelInitiated?.Invoke(eventArgs);
        }

        private void ResetGameplay(LevelInitiatedEventArgs eventArgs)
        {
            _model.ResetGameplay();
            UpdatePointsLimit();
        }

        #endregion
    }
}
