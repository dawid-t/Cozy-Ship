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
        [SerializeField] private AudioClip _coinAudioClip;
        [SerializeField] private AudioClip _obstacleAudioClip;

        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlaySound(SFXType type)
        {
            _audioSource.PlayOneShot(GetAudioClip(type));
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
    }
}
