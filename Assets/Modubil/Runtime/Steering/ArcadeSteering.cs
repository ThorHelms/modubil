using UnityEngine;

namespace Assets.Modubil.Runtime.Steering
{
    public class ArcadeSteering : MonoBehaviour, ISteering
    {
        [SerializeField] private float _turnMultiplier = 10f;
        [SerializeField] private GameObject _steering;

        private ISteering _childSteering;
        private Rigidbody _rigidbody;

        private void Start()
        {
            if (_steering != null)
            {
                _childSteering = _steering.GetComponent<ISteering>();
            }

            _rigidbody = GetComponentInParent<Rigidbody>();

            if (_childSteering == null)
            {
                Debug.LogWarning($"Missing {nameof(ISteering)} as a child", this);
            }

            if (_rigidbody == null)
            {
                Debug.LogWarning($"Missing {nameof(Rigidbody)} as a parent", this);
            }
        }

        public void SetSteering(float steeringValue)
        {
            _childSteering?.SetSteering(steeringValue);

            if (_rigidbody == null) return;

            _rigidbody.AddRelativeTorque(0, steeringValue * _turnMultiplier, 0);
        }

        public float GetMinTurningRadius()
        {
            return _childSteering?.GetMinTurningRadius() ?? 8; // Random static value
        }

        public void SetMinTurningRadius(float minTurningRadius)
        {
            _childSteering?.SetMinTurningRadius(minTurningRadius);
        }
    }
}