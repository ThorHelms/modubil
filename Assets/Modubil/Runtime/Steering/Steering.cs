using UnityEngine;

namespace Assets.Modubil.Runtime.Steering {
    public class Steering : MonoBehaviour, ISteering {
        [SerializeField] private float _minTurningRadius = 8;
        [SerializeField] private Transform _neutralSteeringMarker;

        private float _angle;
        private ISteerableWheel[] _wheels;

        [SerializeField] private GameObject _steeringMarker;

        private void Start()
        {
            var rb = GetComponentInParent<Rigidbody>();
            _wheels = rb.transform.GetComponentsInChildren<ISteerableWheel>();
        }

        private void Update()
        {
            var neutralPosition = _neutralSteeringMarker == null ? transform.position : _neutralSteeringMarker.position;

            if (Mathf.Approximately(_angle, 0))
            {
                foreach (var wheel in _wheels)
                {
                    wheel.ResetSteering();
                }

                if (_steeringMarker != null)
                {
                    _steeringMarker.transform.position = neutralPosition;
                }
            }
            else
            {
                var dist = Mathf.Tan((_angle + 2) * Mathf.PI / 4) * _minTurningRadius;
                var turningPoint = transform.right * dist * -1 + neutralPosition;

                if (_steeringMarker != null)
                {
                    _steeringMarker.transform.position = turningPoint;
                }
                
                foreach (var wheel in _wheels)
                {
                    wheel.SteerTowards(turningPoint);
                }
            }
        }

        public void SetSteering(float steeringValue)
        {
            _angle = steeringValue;
        }

        public float GetMinTurningRadius() => _minTurningRadius;

        public void SetMinTurningRadius(float minTurningRadius)
        {
            _minTurningRadius = minTurningRadius;
        }
    }
}
