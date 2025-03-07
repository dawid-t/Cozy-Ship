using UnityEngine;

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

        [SerializeField] private AudioClip _coinAudioClip;
        [SerializeField] private AudioClip _obstacleAudioClip;

        #endregion

        #region Fields

        private AudioSource _audioSource;

        #endregion

        #region Properties

        public static float Volume { get; set; } = 0.3f;

        #endregion

        #region Public Methods

        public void PlaySound(SFXType type)
        {
            _audioSource.PlayOneShot(GetAudioClip(type), Volume);
        }

        #endregion

        #region Private Methods

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            float savedVolume = PlayerPrefs.GetFloat(GameConfig.SFXVolumeKey, Volume);
            Volume = savedVolume;
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

        #endregion
    }
}
