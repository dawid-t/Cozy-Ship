using Critsoft.CozyShip.MainMenu.Controllers;
using UnityEngine;
using Zenject;

namespace Critsoft.CozyShip
{
    [RequireComponent(typeof(AudioSource))]
    public class AmbientAudioManager : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private AudioSource _audioSource;

        #endregion

        #region Fields

        private static AmbientAudioManager _instance;
        private MainMenuController _mainMenuController;

        #endregion

        #region Properties

        public float Volume => _audioSource.volume;

        #endregion

        #region Public Methods

        [Inject]
        public void Construct(MainMenuController mainMenuController)
        {
            _mainMenuController = mainMenuController;
            _mainMenuController.MusicVolumeChanged += OnVolumeChanged;
        }

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
        }

        private void OnDestroy()
        {
            if (_mainMenuController != null)
            {
                _mainMenuController.MusicVolumeChanged -= OnVolumeChanged;
            }
        }

        private void OnVolumeChanged(float volume)
        {
            _audioSource.volume = volume;
        }

        #endregion
    }
}
