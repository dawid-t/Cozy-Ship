using UnityEngine;
using TMPro;
using Zenject;
using Critsoft.CozyShip.Gameplay.Controllers;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace Critsoft.CozyShip.Gameplay.Views
{
    public class GameplayView : MonoBehaviour
    {
        #region Serialized Fields

        [Header("HUD Elements")]
        [SerializeField] private Canvas _hudCanvas;
        [SerializeField] private TMP_Text _pointsText;
        [SerializeField] private TMP_Text _collisionsText;
        [SerializeField] private TMP_Text _timerText;

        [Header("Pause Menu")]
        [SerializeField] private Canvas _pauseMenuCanvas;
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _exitToMenuButton;
        [SerializeField] private Button _exitToDesktopButton;

        [Header("Fonts")]
        [SerializeField] private TMP_Text[] _allTexts;

        #endregion

        #region Fields

        private GameplayController _controller;
        private string _lastUpdatedPoints;
        private string _lastUpdatedPointsLimit;

        #endregion

        #region Public Methods

        [Inject]
        public void Construct(GameplayController controller)
        {
            _controller = controller;

            _controller.PointsLimitReached += OnPointsLimitReached;
            _controller.PointsUpdated += OnPointsUpdated;
            _controller.PointsLimitUpdated += OnPointsLimitUpdated;
            _controller.CollisionsUpdated += OnCollisionsUpdated;
            _controller.TimeUpdated += OnTimeUpdated;
            _controller.PauseStateChanged += OnPauseStateChanged;
            _controller.LevelInitiated += OnLevelInitiated;
            _controller.FontChanged += OnFontChanged;

            _resumeButton.onClick.AddListener(_controller.TogglePause);
            _restartButton.onClick.AddListener(_controller.RestartLevel);
            _exitToMenuButton.onClick.AddListener(_controller.ExitToMenu);
            _exitToDesktopButton.onClick.AddListener(_controller.ExitToDesktop);
        }

        #endregion

        #region Private Methods

        private void OnDestroy()
        {
            if (_controller != null)
            {
                _controller.PointsLimitReached -= OnPointsLimitReached;
                _controller.PointsUpdated -= OnPointsUpdated;
                _controller.PointsLimitUpdated -= OnPointsLimitUpdated;
                _controller.CollisionsUpdated -= OnCollisionsUpdated;
                _controller.TimeUpdated -= OnTimeUpdated;
                _controller.PauseStateChanged -= OnPauseStateChanged;
                _controller.LevelInitiated -= OnLevelInitiated;
                _controller.FontChanged -= OnFontChanged;
            }

            _resumeButton.onClick.RemoveListener(_controller.TogglePause);
            _restartButton.onClick.RemoveListener(_controller.RestartLevel);
            _exitToMenuButton.onClick.RemoveListener(_controller.ExitToMenu);
            _exitToDesktopButton.onClick.RemoveListener(_controller.ExitToDesktop);

            DOTween.Kill(_pauseMenuCanvas.transform);
        }

        private void OnPointsLimitReached(PlayerStatsEventArgs eventArgs)
        {
            _hudCanvas.enabled = false;
        }

        private void UpdatePointsText()
        {
            _pointsText.text = _lastUpdatedPoints + "/" + _lastUpdatedPointsLimit;
        }

        private void OnPointsUpdated(int points)
        {
            _lastUpdatedPoints = points.ToString();
            UpdatePointsText();
        }

        private void OnPointsLimitUpdated(int pointsLimit)
        {
            _lastUpdatedPointsLimit = pointsLimit.ToString();
            UpdatePointsText();
        }

        private void OnCollisionsUpdated(int collisions)
        {
            _collisionsText.text = collisions.ToString();
        }

        private void OnTimeUpdated(float time)
        {
            _timerText.text = $"{time:F1}s";
        }

        private void OnPauseStateChanged(bool isPaused)
        {
            TogglePauseMenu(isPaused);
        }

        private void OnLevelInitiated(LevelInitiatedEventArgs eventArgs)
        {
            _hudCanvas.enabled = true;
        }

        private void TogglePauseMenu(bool enable)
        {
            _pauseMenuCanvas.transform.localScale = Vector3.zero;
            _pauseMenuCanvas.enabled = enable;

            EventSystem.current.SetSelectedGameObject(null);
            if (enable)
            {
                DOTween.Kill(_pauseMenuCanvas.transform);

                _pauseMenuCanvas.transform.DOScale(Vector3.one, 0.5f)
                    .SetEase(Ease.OutBack)
                    .SetUpdate(true);

                _resumeButton.Select();
            }
        }

        private void OnFontChanged(int fontIndex, TMP_FontAsset newFont)
        {
            foreach (var textElement in _allTexts)
            {
                textElement.font = newFont;
            }
        }

        #endregion
    }
}
