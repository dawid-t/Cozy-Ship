using UnityEngine;

namespace Critsoft.CozyShip.Gameplay
{
    public class BackgroundObjectAnimTrigger : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private Animator _animator;
        [SerializeField] private string _animStateName = "";
        [SerializeField] private int _animLayer = 0;
        [SerializeField] private float _animNormalizedTime = 0f;

        #endregion

        #region Fields

        private Vector3 _originAnimatorScale;

        #endregion

        #region Private methods

        private void OnEnable()
        {
            _originAnimatorScale = _animator.transform.localScale;
        }

        private void OnDisable()
        {
            _animator.enabled = false;
            _animator.transform.localScale = _originAnimatorScale;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Tags.Player) && !_animator.enabled)
            {
                _animator.enabled = true;

                if (!string.IsNullOrEmpty(_animStateName))
                {
                    _animator.Play(_animStateName, _animLayer, _animNormalizedTime);
                }
            }
        }
        
        #endregion
    }
}
