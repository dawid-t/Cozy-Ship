using UnityEngine;
using DG.Tweening;

namespace Critsoft.CozyShip.Gameplay
{
    public class CoinAnimation : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private float _floatHeight = 0.5f;
        [SerializeField] private float _floatDuration = 1.5f;
        [SerializeField] private float _pulseScale = 0.5f;
        [SerializeField] private float _pulseDuration = 1f;

        #endregion

        #region Private methods

        private void OnEnable()
        {
            AnimateFloating();
            AnimatePulsing();
        }

        private void OnDisable()
        {
            DOTween.Kill(transform);
        }

        private void AnimateFloating()
        {
            transform.DOLocalMoveY(transform.localPosition.y + _floatHeight, _floatDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }

        private void AnimatePulsing()
        {
            transform.DOScale(Vector3.one + Vector3.one * _pulseScale, _pulseDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }

        #endregion
    }
}
