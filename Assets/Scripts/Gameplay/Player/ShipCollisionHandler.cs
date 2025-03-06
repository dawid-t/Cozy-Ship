using System;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using Critsoft.CozyShip.Gameplay.Controllers;
using Zenject;

namespace Critsoft.CozyShip.Gameplay.Player
{
    public class ShipCollisionHandler : MonoBehaviour
    {
        #region Events

        public event Action CollisionOccurred;

        #endregion

        #region Serialized Fields

        [Header("Ship References")]
        [SerializeField] private ShipMovement _shipMovement;
        [SerializeField] private Transform _shipTransform;
        [SerializeField] private Renderer _shipRenderer;
        [SerializeField] private Collider _collider;
        [SerializeField] private ShipFoamEffect _shipFoamEffect;

        [Header("Respawn Settings")]
        [SerializeField] private float _respawnTime = 2.5f;
        [SerializeField] private float _movementBlockTime = 0.5f;
        [SerializeField] private float _blinkInterval = 0.2f;
        [SerializeField] private float _scaleDuration = 0.5f;
        [SerializeField] private float _slowDownDuration = 1f;

        [Header("Coins Drop Settings")]
        [SerializeField] private int _maxCoinsDropNumber = 10;
        [SerializeField] private float _coinScatterRadius = 1;
        [SerializeField] private float _coinLifeTime = 5;
        [SerializeField] private float _coinStartBlinkTime = 2;

        #endregion

        #region Fields

        private bool _isRespawning;
        private bool _canUpdateCurrentCoinsNumber = true;
        private int _currentCoinsNumber;
        private WaitForSeconds _blinkWaitForSeconds;
        private GameplayController _gameplayController;
        private CollectibleCoinPool _coinPool;

        #endregion

        [Inject]
        public void Construct(GameplayController gameplayController, CollectibleCoinPool coinPool)
        {
            _gameplayController = gameplayController;
            _coinPool = coinPool;

            _gameplayController.PointsUpdated += OnPointsUpdated;
        }

        #region Private Methods

        private void Awake()
        {
            _blinkWaitForSeconds = new WaitForSeconds(_blinkInterval);
        }

        private void OnDestroy()
        {
            if (_gameplayController != null)
            {
                _gameplayController.PointsUpdated -= OnPointsUpdated;
            }
            DOTween.Kill(_shipTransform);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isRespawning)
                return;

            if (other.CompareTag(Tags.Obstacle))
            {
                DestroyShip();
                CollisionOccurred?.Invoke();
            }
        }

        private void DestroyShip()
        {
            _canUpdateCurrentCoinsNumber = false;
            _isRespawning = true;
            _collider.enabled = false;

            // Stop ship movement
            _shipMovement.DisableMovement();
            _shipMovement.SlowDownToZero(_slowDownDuration);
            _shipFoamEffect.ToggleFoam(false);

            // Scale ship down
            _shipTransform.DOScale(Vector3.zero, _scaleDuration)
                .OnComplete(() =>
                {
                    ScatterCoins();
                    StartCoroutine(RespawnShip());
                });
        }
        
        private IEnumerator RespawnShip()
        {
            _shipMovement.ResetPosition();
            _shipTransform.DOScale(Vector3.one, _scaleDuration);

            // Blink effect
            float elapsedTime = 0f;
            while (elapsedTime < _respawnTime)
            {
                _shipRenderer.enabled = !_shipRenderer.enabled;
                yield return _blinkWaitForSeconds;
                elapsedTime += _blinkInterval;

                if (elapsedTime > _movementBlockTime)
                {
                    _shipMovement.EnableMovement();
                    _shipFoamEffect.ToggleFoam(true);
                }
            }

            _shipRenderer.enabled = true;
            _collider.enabled = true;
            _isRespawning = false;
            _canUpdateCurrentCoinsNumber = true;
        }

        private void ScatterCoins()
        {
            int coinsToScatter = Mathf.Clamp(_currentCoinsNumber, 0, _maxCoinsDropNumber);
            if (coinsToScatter <= 0)
                return;
            
            for (int i = 0; i < coinsToScatter; i++)
            {
                Vector3 scatterPosition = GetRandomScatterPosition();
                CollectibleCoin coin = _coinPool.Spawn(scatterPosition);

                StartCoroutine(StartDisappearing(coin));
            }

            // Local methods
            Vector3 GetRandomScatterPosition()
            {
                float angle = UnityEngine.Random.Range(0f, 360f) * Mathf.Deg2Rad;
                
                float minRadius = 0.5f;
                float radius = UnityEngine.Random.Range(minRadius, _coinScatterRadius);

                Vector3 offset = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
                return transform.position + offset;
            }

            IEnumerator StartDisappearing(CollectibleCoin coin)
            {
                if (coin == null)
                    yield break;

                yield return new WaitForSeconds(_coinStartBlinkTime);

                float elapsedTime = 0f;
                float blinkMinInterval = 0.1f;
                float blinkMaxInterval = 0.5f;
                float blinkInterval = blinkMaxInterval;
                float blinkTime = _coinLifeTime - _coinStartBlinkTime;
                
                while (elapsedTime < blinkTime)
                {
                    coin.Renderer.enabled = !coin.Renderer.enabled;
                    yield return new WaitForSeconds(blinkInterval);

                    elapsedTime += blinkInterval;
                    blinkInterval = Mathf.Lerp(blinkMaxInterval, blinkMinInterval, elapsedTime / blinkTime);
                }

                if (coin != null)
                {
                    _coinPool.Despawn(coin);
                }
            }
        }
        
        private void OnPointsUpdated(int points)
        {
            if (_canUpdateCurrentCoinsNumber)
            {
                _currentCoinsNumber = points;
            }
        }

        #endregion
    }
}
