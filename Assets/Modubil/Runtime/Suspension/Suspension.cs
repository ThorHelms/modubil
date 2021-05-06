using UnityEngine;

namespace Assets.Modubil.Runtime.Suspension
{
    public class Suspension : MonoBehaviour, ISuspension
    {
        [SerializeField] private GameObject _suspensionDistanceProvider;
        private ISuspensionDistance _suspensionDistance;
        private Rigidbody _vehicleRigidbody;

        [SerializeField] private float _suspensionForce = 30000;
        [SerializeField] private float _suspensionDamper = 4000;

        private float _suspensionForceMagnitude;
        private float _lastSuspensionDistance;

        private void Start()
        {
            if (_suspensionDistanceProvider != null)
            {
                _suspensionDistance = _suspensionDistanceProvider.GetComponent<ISuspensionDistance>();
            }

            if (_suspensionDistance == null)
            {
                _suspensionDistance = GetComponentInChildren<ISuspensionDistance>();
            }

            if (_suspensionDistance == null)
            {
                _suspensionDistance = GetComponentInParent<ISuspensionDistance>();
            }

            _vehicleRigidbody = GetComponentInParent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (_suspensionDistance == null)
            {
                return;
            }

            var suspensionDistance = _suspensionDistance.GetSuspensionDistance();
            var springVelocity = (suspensionDistance - _lastSuspensionDistance) / Time.fixedDeltaTime;
            var springForce = _suspensionForce * suspensionDistance;
            var damperForce = _suspensionDamper * springVelocity;
            _suspensionForceMagnitude = springForce + damperForce;

            if (_vehicleRigidbody != null)
            {
                _vehicleRigidbody.AddForceAtPosition(transform.up * _suspensionForceMagnitude, transform.position);
            }

            _lastSuspensionDistance = suspensionDistance;
        }

        public float GetForceMagnitude() => _suspensionForceMagnitude;
    }
}