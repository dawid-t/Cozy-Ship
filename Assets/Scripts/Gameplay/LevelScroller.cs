using Critsoft.CozyShip.Gameplay.Controllers;
using System;
using UnityEngine;
using Zenject;

namespace Critsoft.CozyShip.Gameplay
{
    public class LevelScroller : MonoBehaviour
    {
        #region Events

        public event Action<Transform> PositionReseted;

        #endregion

        #region Serialized Fields

        [SerializeField] private float _scrollSpeed = 5f;
        [SerializeField] private float _resetPositionZ = -210f;
        [SerializeField] private float _startPositionZ = 210f;
        [SerializeField] private Transform[] _scrollableObjects;

        #endregion

        #region Fields

        private LevelController _levelController;

        #endregion

        #region Public methods

        [Inject]
        public void Construct(LevelController levelController)
        {
            _levelController = levelController;
            _levelController.LevelInitiated += OnLevelInitiated;
        }

        #endregion

        #region Private methods

        private void Update()
        {
            ScrollObjects();
        }

        private void OnDestroy()
        {
            if (_levelController != null)
            {
                _levelController.LevelInitiated -= OnLevelInitiated;
            }
        }

        private void ScrollObjects()
        {
            for (int i = 0; i < _scrollableObjects.Length; i++)
            {
                Transform scrollableObject = _scrollableObjects[i];
                scrollableObject.position += Vector3.back * _scrollSpeed * Time.deltaTime;

                if (scrollableObject.position.z <= _resetPositionZ)
                {
                    scrollableObject.position = new Vector3(scrollableObject.position.x, scrollableObject.position.y, _startPositionZ);
                    PositionReseted?.Invoke(scrollableObject);
                }
            }
        }

        private void OnLevelInitiated(LevelInitiatedEventArgs eventArgs)
        {
            _scrollSpeed = eventArgs.SceneScrollSpeed;
        }

        #endregion
    }
}
