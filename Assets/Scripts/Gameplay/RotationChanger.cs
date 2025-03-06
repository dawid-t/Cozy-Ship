using UnityEngine;

namespace Critsoft.CozyShip.Gameplay
{
    public class RotationChanger : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private bool _freezeX;
        [SerializeField] private bool _freezeY;
        [SerializeField] private bool _freezeZ;
        [Space]
        [SerializeField] private Vector3 _minRotation;
        [SerializeField] private Vector3 _maxRotation;

        #endregion

        #region Fields

        private Quaternion _originRotation;

        #endregion

        #region Private methods

        private void OnEnable()
        {
            _originRotation = transform.localRotation;

            float x = _freezeX ? 0 : Random.Range(_minRotation.x, _maxRotation.x);
            float y = _freezeY ? 0 : Random.Range(_minRotation.y, _maxRotation.y);
            float z = _freezeZ ? 0 : Random.Range(_minRotation.z, _maxRotation.z);

            transform.rotation = Quaternion.Euler(x, y, z);
        }

        private void OnDisable()
        {
            transform.rotation = _originRotation;
        }

        #endregion
    }
}
