using UnityEngine;

namespace Critsoft.CozyShip
{
    [RequireComponent(typeof(AudioSource))]
    public class AmbientAudioManager : MonoBehaviour
    {
        private static AmbientAudioManager _instance;
        private AudioSource _audioSource;

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
        }

        public void PlayMusic(AudioClip clip)
        {
            _audioSource.clip = clip;
            _audioSource.loop = true;
            _audioSource.Play();
        }

        public void StopMusic()
        {
            _audioSource.Stop();
        }
    }
}
