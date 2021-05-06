using Assets.Modubil.Runtime.Steering;
using UnityEngine;

namespace Assets.Modubil.Runtime.Addons {
    public class MaxSteerAngleVsSpeed : MonoBehaviour
    {
        [Tooltip("The relative steering angle based on the speed (KM/H)")]
        [SerializeField] AnimationCurve curve = AnimationCurve.Linear(0, 1, 250, 0.5f);

        private ISteering _steering;
        private Rigidbody _rigidbody;
        private float _initialMinTurningRadius;

        private void Start()
        {
            _rigidbody = GetComponentInParent<Rigidbody>();
            _steering = _rigidbody.transform.GetComponentInChildren<ISteering>();
            _initialMinTurningRadius = _steering?.GetMinTurningRadius() ?? 0;

            if (_rigidbody == null)
            {
                Debug.LogWarning($"Missing {nameof(Rigidbody)} in parent of {transform.name}");
            }

            if (_steering == null)
            {
                Debug.LogWarning($"Missing {nameof(ISteering)} in children of {transform.name}");
            }
        }


        void Update()
        {
            if (_rigidbody == null || _steering == null)
            {
                return;
            }

            var range = GetMaxSteerAtSpeed(_rigidbody.velocity.magnitude * 3.6f);
            _steering.SetMinTurningRadius(range * _initialMinTurningRadius);
        }

        public float GetMaxSteerAtSpeed(float speed) {
            return Mathf.Clamp(curve.Evaluate(speed), 0, 1);
        }
    }
}
