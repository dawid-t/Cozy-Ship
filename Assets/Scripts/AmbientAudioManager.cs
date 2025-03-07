using UnityEngine;

namespace Critsoft.CozyShip
{
    [RequireComponent(typeof(AudioSource))]
    public class AmbientAudioManager : MonoBehaviour
    {
        #region Fields

        public static float _volume = 0.1f;
        private static AmbientAudioManager _instance;
        private AudioSource _audioSource;

        #endregion

        #region Properties

        public static float Volume
        {
            get => _volume;
            set
            {
                _volume = value;
                if (_instance != null)
                {
                    _instance._audioSource.volume = Volume;
                }
            }
        }

        #endregion

        #region Public Methods

        public void PlayMusic(AudioClip clip)
        {
            _audioSource.clip = clip;
            _audioSource.loop = true;
            _audioSource.volume = Volume;
            _audioSource.Play();
        }

        public void StopMusic()
        {
            _audioSource.Stop();
        }

        #endregion

        #region Private Methods

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            _audioSource = GetComponent<AudioSource>();
            
            float savedVolume = PlayerPrefs.GetFloat(GameConfig.MusicVolumeKey, Volume);
            Volume = savedVolume;

            _audioSource.volume = Volume;
        }

        #endregion
    }
}
