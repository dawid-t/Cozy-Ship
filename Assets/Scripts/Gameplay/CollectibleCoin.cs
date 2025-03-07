using System;
using UnityEngine;
using Zenject;

namespace Critsoft.CozyShip.Gameplay
{
    public class CollectibleCoin : MonoBehaviour
    {
        public class Factory : PlaceholderFactory<CollectibleCoin>
        { }

        #region Serialized Fields

        [SerializeField] private CoinType _coinType;
        [SerializeField] private int _coinValue = 1;
        [SerializeField] private Renderer _renderer;

        #endregion

        #region Fields

        private const int MinCoinValue = 1;
        
        private Action<int> _coinCollected;
        private SFXAudioManager _sfxAudioManager;

        #endregion

        #region Properties

        public CoinType CoinType => _coinType;
        public Renderer Renderer => _renderer;

        #endregion

        #region Public Methods

        [Inject]
        public void Construct([Inject(Id = GameConfig.CoinCollectedId)] Action<int> coinCollected, SFXAudioManager sfxAudioManager)
        {
            _coinCollected = coinCollected;
            _sfxAudioManager = sfxAudioManager;
        }

        #endregion

        #region Private Methods

        private void OnEnable()
        {
            if (_coinValue < MinCoinValue)
            {
                _coinValue = MinCoinValue;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Tags.Player))
            {
                _sfxAudioManager.PlaySound(SFXType.Coin);
                _coinCollected?.Invoke(_coinValue);
                gameObject.SetActive(false);
            }
        }

        #endregion
    }
}
