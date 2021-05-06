using UnityEngine;

namespace Assets.Modubil.Runtime.Power {
    public class Motor : MonoBehaviour, IMotor {
        [Tooltip("The maximum torque that the motor generates")]
        [SerializeField] private float _maxTorque = 10000;

        [SerializeField] private float _maxReverseInput = -.5f;

        private float _throttle = 0;

        private IDrivetrain _drivetrain;

        private void Start()
        {
            _drivetrain = GetComponentInChildren<IDrivetrain>();

            if (_drivetrain == null)
            {
                Debug.LogWarning($"Missing {nameof(IDrivetrain)} in children of {transform.name}");
            }
        }

        private void FixedUpdate() {
            if (_drivetrain == null)
            {
                return;
            }

            _drivetrain.ApplyMotorTorque(_throttle * _maxTorque);
        }

        public void SetThrottle(float throttle)
        {
            _throttle = Mathf.Clamp(throttle, _maxReverseInput, 1);
        }
    }
}
