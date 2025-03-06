using Critsoft.CozyShip.Gameplay.Controllers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Critsoft.CozyShip.Gameplay.Player
{
    public class ShipMovement : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private float _acceleration = 5f;
        [SerializeField] private float _deceleration = 5f;
        [SerializeField] private float _maxSpeed = 10f;
        [Range(0f, 10f)]
        [SerializeField] private float _rotationSpeed = 2f;
        [Space]
        [SerializeField] private InputActionReference _moveInputAction;
        [SerializeField] private ShipCollisionHandler _collisionHandler;

        #endregion

        #region Fields

        private const float RotationThreshold = 0.1f;

        private bool _canMove = true;
        private float _currentSpeed = 0f;
        private float _targetRotationY = 0f;
        private float _currentRotationY = 0f;
        private Vector2 _moveDirection;

        private Vector3 _originPosition;
        private Quaternion _originRotation;
        private LevelController _levelController;
        private Tween _slowDownToZeroTween;

        #endregion

        #region Properties

        public float CurrentSpeed => _currentSpeed;

        #endregion

        #region Public Methods

        [Inject]
        public void Construct(LevelController levelController)
        {
            _levelController = levelController;
            _levelController.LevelInitiated += OnLevelInitiated;
        }

        public void EnableMovement()
        {
            _canMove = true;
        }

        public void DisableMovement()
        {
            _canMove = false;
            _moveDirection = Vector2.zero;
        }

        public void SlowDownToZero(float duration)
        {
            float currentSpeed = _currentSpeed;

            _slowDownToZeroTween = DOTween.To(() => currentSpeed, x => currentSpeed = x, 0, duration)
                .OnUpdate(() => _currentSpeed = currentSpeed)
                .OnComplete(() => DisableMovement());
        }
        
        public void ResetPosition()
        {
            _currentSpeed = 0f;
            _currentRotationY = 0f;
            _targetRotationY = 0f;

            transform.position = _originPosition;
            transform.rotation = _originRotation;
        }

        #endregion
        
        #region Private Methods

        private void Awake()
        {
            _originPosition = transform.position;
            _originRotation = transform.rotation;
        }

        private void Update()
        {
            if (!_canMove)
                return;
            
            UpdateMoveDirection();
            ApplyRotation();
            ApplyMovement();
        }

        private void OnDestroy()
        {
            if (_levelController != null)
            {
                _levelController.LevelInitiated -= OnLevelInitiated;
            }

            if (_slowDownToZeroTween != null)
            {
                _slowDownToZeroTween.Kill();
            }
        }

        private void UpdateMoveDirection()
        {
            _moveDirection = _moveInputAction.action.ReadValue<Vector2>();
        }

        private void ApplyRotation()
        {
            if (_moveDirection.sqrMagnitude > RotationThreshold)
            {
                _targetRotationY = Mathf.Atan2(_moveDirection.x, _moveDirection.y) * Mathf.Rad2Deg;
            }

            _currentRotationY = Mathf.LerpAngle(_currentRotationY, _targetRotationY, _rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, _currentRotationY, 0);
        }

        private void ApplyMovement()
        {
            float forwardInput = _moveDirection.magnitude;

            if (forwardInput > 0)
            {
                _currentSpeed += _acceleration * Time.deltaTime;
                _currentSpeed = Mathf.Clamp(_currentSpeed, 0, _maxSpeed);
            }
            else
            {
                _currentSpeed -= _deceleration * Time.deltaTime;
                if (_currentSpeed < 0) _currentSpeed = 0;
            }

            transform.position += transform.forward * _currentSpeed * Time.deltaTime;
        }

        private void OnLevelInitiated(LevelInitiatedEventArgs eventArgs)
        {
            ResetPosition();
        }

        #endregion
    }
}
