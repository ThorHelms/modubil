using Assets.Modubil.Runtime.CollisionDetection;
using Assets.Modubil.Runtime.Power;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Assets.Modubil.Runtime.Wheel {
    public class WheelFriction : MonoBehaviour, IPoweredWheel, IRpmProvider {
        const float engineShaftToWheelRatio = 25;

        [Tooltip("A height offset for applying forces, to prevent the vehicle from rolling as much.")]
        [SerializeField] private float _applyForcesOffset;

        public float forwardFrictionCoeff = 1;

        public Vector3 velocity { get; private set; }

        public float motorTorque { get; set; }

        public float lateralFrictionMultiplier = 300;


        private Rigidbody _rb;

        private IWheelCollisionDetector _wheelCollisionDetector;

        private float _rpm;

        private IWheel _wheel;

        private void Start()
        {
            _rb = GetComponentInParent<Rigidbody>();
            _wheelCollisionDetector = GetComponent<IWheelCollisionDetector>();
            _wheel = GetComponent<IWheel>();

            if (_rb == null)
            {
                Debug.LogWarning($"Missing {nameof(Rigidbody)} in parent of {transform.name}");
            }

            if (_wheelCollisionDetector == null)
            {
                Debug.LogWarning($"Missing {nameof(IWheelCollisionDetector)} in {transform.name}");
            }

            if (_wheel == null)
            {
                Debug.LogWarning($"Missing {nameof(IWheel)} in {transform.name}");
            }
        }

        private void FixedUpdate()
        {
            if (_rb == null || _wheelCollisionDetector == null || _wheel == null)
            {
                return;
            }

            velocity = _rb.GetPointVelocity(transform.position);

            CalculateFriction();
            UpdateRpm();
        }

        private void CalculateFriction()
        {
            var isGrounded = _wheelCollisionDetector.TryGetCollision(out var point, out var normal, out var collider, out var rb, out var t);

            if (!isGrounded)
            {
                return;
            }

            var forceOffset = transform.up * _applyForcesOffset;

            var right = transform.right;
            var forward = transform.forward;

            var direction = Vector3.Cross(normal, right);
            if (motorTorque < 0)
            {
                direction *= -1;
            }

            var lateralVelocity = Vector3.Project(velocity, right);
            var forwardVelocity = Vector3.Project(velocity, forward);
            var slip = (forwardVelocity + lateralVelocity) / 2;

            var lateralFriction = Vector3.Project(right, slip).magnitude * lateralVelocity.magnitude * lateralFrictionMultiplier / Time.fixedDeltaTime;
            _rb.AddForceAtPosition(-Vector3.Project(slip, lateralVelocity).normalized * lateralFriction, point + forceOffset);

            var motorForce = Mathf.Abs(motorTorque / _wheel.GetRadius());
            var maxForwardFriction = motorForce * forwardFrictionCoeff;
            var appliedForwardFriction = Mathf.Clamp(motorForce, 0, maxForwardFriction);

            var forwardForce = direction.normalized * appliedForwardFriction * engineShaftToWheelRatio;
            _rb.AddForceAtPosition(forwardForce, point + forceOffset);
        }

        public void ApplyTorque(float torque)
        {
            motorTorque = torque;
        }

        private void UpdateRpm()
        {
            var forwardVelocity = Vector3.Dot(velocity, transform.forward);
            var rotPerSec = forwardVelocity / (2 * Mathf.PI * _wheel.GetRadius());
            _rpm = rotPerSec * 60;
        }

        public float GetRpm() => _rpm;
    }
}