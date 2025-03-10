using Critsoft.CozyShip.Gameplay.Controllers;
using UnityEngine;
using Zenject;

namespace Critsoft.CozyShip.Gameplay
{
    public enum SFXType
    {
        Coin,
        Obstacle
    }

    [RequireComponent(typeof(AudioSource))]
    public class SFXAudioManager : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private AudioSource _audioSource;
        [Space]
        [SerializeField] private AudioClip _coinAudioClip;
        [SerializeField] private AudioClip _obstacleAudioClip;

        #endregion

        #region Fields

        private GameplayController _gameplayController;

        #endregion

        #region Properties

        public float Volume => _audioSource.volume;

        #endregion

        #region Public Methods

        [Inject]
        public void Construct(GameplayController gameplayController)
        {
            _gameplayController = gameplayController;
            _gameplayController.SFXVolumeChanged += OnVolumeChanged;
        }

        public void PlaySound(SFXType type)
        {
            _audioSource.PlayOneShot(GetAudioClip(type));
        }

        #endregion

        #region Private Methods

        private void OnDestroy()
        {
            if (_gameplayController != null)
            {
                _gameplayController.SFXVolumeChanged -= OnVolumeChanged;
            }
        }

        private AudioClip GetAudioClip(SFXType type)
        {
            switch (type)
            {
                case SFXType.Coin:
                    return _coinAudioClip;

                case SFXType.Obstacle:
                default:
                    return _obstacleAudioClip;
            }
        }

        private void OnVolumeChanged(float volume)
        {
            _audioSource.volume = volume;
        }

        #endregion
    }
}
