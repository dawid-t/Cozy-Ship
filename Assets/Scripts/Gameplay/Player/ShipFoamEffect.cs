using UnityEngine;

namespace Critsoft.CozyShip.Gameplay.Player
{
    public class ShipFoamEffect : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private ParticleSystem _foamParticles;
        [SerializeField] private ShipMovement _shipMovement;
        [SerializeField] private float _speedThreshold = 1f;
        [SerializeField] private int _rateOverDistanceValue = 20;

        #endregion

        #region Fields

        private bool _isEmmisionEnabled = true;
        private ParticleSystem.EmissionModule _emission;

        #endregion

        #region Public methods

        public void ToggleFoam(bool enable)
        {
            _isEmmisionEnabled = enable;
            if (!enable)
            {
                _emission.rateOverDistance = 0;
            }
        }

        #endregion

        #region Private methods

        private void Awake()
        {
            _emission = _foamParticles.emission;
        }

        private void Update()
        {
            if (_isEmmisionEnabled)
            {
                _emission.rateOverDistance = _shipMovement.CurrentSpeed > _speedThreshold ? _rateOverDistanceValue : 0;
            }
        }

        #endregion
    }
}
