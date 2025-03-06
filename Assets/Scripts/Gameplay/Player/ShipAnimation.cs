using UnityEngine;
using DG.Tweening;

namespace Critsoft.CozyShip.Gameplay.Player
{
    public class ShipAnimation : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private float _maxRotationOffset = 15f;
        [SerializeField] private float _maxHeightOffset = 1f;
        [SerializeField] private float _maxSwayOffset = 1f;
        [Space]
        [SerializeField] private float _minDuration = 1.5f;
        [SerializeField] private float _maxDuration = 3f;

        #endregion

        #region Public methods

        public void PlayAnimations()
        {
            StopAnimations();

            AnimateRotation();
            AnimateHeight();
            AnimateSway();
        }

        public void StopAnimations()
        {
            DOTween.Kill(transform);
        }

        #endregion

        #region Private methods

        private void OnEnable()
        {
            PlayAnimations();
        }

        private void OnDisable()
        {
            StopAnimations();
        }

        private void AnimateRotation()
        {
            float randomDuration = Random.Range(_minDuration, _maxDuration);
            float randomRotation = Random.Range(-_maxRotationOffset, _maxRotationOffset);

            transform.DOLocalRotate(new Vector3(0, 0, randomRotation), randomDuration)
                .SetEase(Ease.InOutSine)
                .OnComplete(AnimateRotation);
        }

        private void AnimateHeight()
        {
            float randomDuration = Random.Range(_minDuration, _maxDuration);
            float randomHeight = Random.Range(0, _maxHeightOffset);

            transform.DOLocalMoveY(randomHeight, randomDuration / 2)
                .SetEase(Ease.InOutSine)
                .SetLoops(2, LoopType.Yoyo)
                .OnComplete(AnimateHeight);
        }

        private void AnimateSway()
        {
            float randomDuration = Random.Range(_minDuration, _maxDuration);
            float randomSway = Random.Range(-_maxSwayOffset, _maxSwayOffset);

            transform.DOLocalMoveX(randomSway, randomDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(2, LoopType.Yoyo)
                .OnComplete(AnimateSway);
        }

        #endregion
    }
}
