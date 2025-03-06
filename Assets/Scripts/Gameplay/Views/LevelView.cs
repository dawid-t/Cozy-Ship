using UnityEngine;
using TMPro;
using Critsoft.CozyShip.Gameplay.Controllers;
using Zenject;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace Critsoft.CozyShip.Gameplay.Views
{
    public class LevelView : MonoBehaviour
    {
        #region Serialized Fields

        [Header("Level Complete Popup")]
        [SerializeField] private Canvas _levelCompletePopup;
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private TMP_Text _pointsText;
        [SerializeField] private TMP_Text _collisionsText;
        [SerializeField] private TMP_Text _timerText;
        [SerializeField] private Button _nextLevelButton;

        #endregion

        #region Fields

        private LevelController _controller;

        #endregion

        #region Public Methods

        [Inject]
        public void Construct(LevelController controller)
        {
            _controller = controller;

            _controller.LevelCompleted += OnLevelCompleted;
            _controller.LevelInitiated += OnLevelInitiated;

            _nextLevelButton.onClick.AddListener(_controller.LoadNextLevel);
        }

        #endregion

        #region Private Methods

        private void OnDestroy()
        {
            if (_controller != null)
            {
                _controller.LevelCompleted -= OnLevelCompleted;
                _controller.LevelInitiated -= OnLevelInitiated;
            }

            DOTween.Kill(_levelCompletePopup.transform);
        }

        private void OnLevelCompleted(int levelId, PlayerStatsEventArgs eventArgs)
        {
            UpdatePopupInfo(levelId, eventArgs);
            ToggleLevelCompletePopup(true);
        }

        private void OnLevelInitiated(LevelInitiatedEventArgs eventArgs)
        {
            ToggleLevelCompletePopup(false);
        }

        private void ToggleLevelCompletePopup(bool enable)
        {
            _levelCompletePopup.transform.localScale = Vector3.zero;
            _levelCompletePopup.enabled = enable;

            EventSystem.current.SetSelectedGameObject(null);
            if (enable)
            {
                DOTween.Kill(_levelCompletePopup.transform);

                _levelCompletePopup.transform.DOScale(Vector3.one, 0.5f)
                    .SetEase(Ease.OutBack)
                    .SetUpdate(true);

                _nextLevelButton.Select();
            }
        }

        private void UpdatePopupInfo(int levelId, PlayerStatsEventArgs eventArgs)
        {
            _levelText.text = "Level " + levelId + " Completed!";

            _pointsText.text = eventArgs.Points.ToString();
            _collisionsText.text = eventArgs.Collisions.ToString();
            _timerText.text = $"{eventArgs.ElapsedTime:F1}s";
        }

        #endregion
    }
}
