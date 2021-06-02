using System;
using Assets.Modubil.Runtime.CollisionDetection;
using Assets.Modubil.Runtime.Power;
using Assets.Modubil.Runtime.Suspension;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Assets.Modubil.Runtime.Wheel {
    public class WheelFriction : MonoBehaviour, IWheelFriction, IPoweredWheel, IRpmProvider {
        [Tooltip("A height offset for applying forces, to prevent the vehicle from rolling as much. Should be between 0 and 1. 0 means hit height, 1 means center of mass height.")]
        [SerializeField] private float _applyForcesOffset;
        [SerializeField] private bool _applyForcesAtCom;
        [SerializeField] private float _maxForce = 20000;
        [SerializeField] private float _epsilonPercent = 0.05f;
        [SerializeField] private bool _debugLog;
        [SerializeField] private Color _debugColor;

        private Rigidbody _rb;
        private IWheel _wheel;
        private IWheelCollisionDetector _wheelCollisionDetector;

        private float _rpm;
        private Vector3 velocity;
        private float motorTorque;
        private float _supportedMass;

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

            var right = transform.right;
            var forward = Vector3.Cross(normal, right);

            var lateralF = GetLateralForce(forward, right, point);

            if (motorTorque < 0)
            {
                forward *= -1;
            }

            var localPoint = transform.InverseTransformPoint(point);
            var localCom = transform.InverseTransformPoint(_rb.worldCenterOfMass);
            var forceY = Mathf.Lerp(localPoint.y, localCom.y, _applyForcesOffset);
            var localForcePoint = new Vector3(localPoint.x, forceY, localPoint.z);
            var forcePoint = transform.TransformPoint(localForcePoint);

            var forceToAdd = lateralF;

            var motorForce = Mathf.Abs(motorTorque / _wheel.GetRadius());
            var maxForwardFriction = motorForce;
            var appliedForwardFriction = Mathf.Clamp(motorForce, 0, maxForwardFriction);

            var forwardForce = forward.normalized * appliedForwardFriction;

            forceToAdd += forwardForce;

            if (forceToAdd.magnitude > _maxForce)
            {
                forceToAdd = forceToAdd.normalized * _maxForce;
            }

            if (_applyForcesAtCom)
            {
                _rb.AddForce(forceToAdd);
            }
            else
            {
                _rb.AddForceAtPosition(forceToAdd, forcePoint);
            }
        }

        private Vector3 GetLateralForce(Vector3 forward, Vector3 right, Vector3 point)
        {
            var lateralVelocity = Vector3.Project(velocity, right);
            var forwardVelocity = Vector3.Project(velocity, forward);

            Debug.DrawRay(point, lateralVelocity, _debugColor, 10);

            if (Mathf.Abs(forwardVelocity.magnitude) < Single.Epsilon ||
                lateralVelocity.magnitude / forwardVelocity.magnitude < _epsilonPercent)
            {
                return Vector3.zero;
            }

            var slip = (forwardVelocity + lateralVelocity) / 2;
            var lateralForceDirection = -lateralVelocity.normalized;

            var lateralA = Vector3.Project(right, slip).magnitude * lateralVelocity.magnitude * lateralVelocity.magnitude;
            var maxLateralA = lateralVelocity.magnitude;
            lateralA = Mathf.Min(lateralA, maxLateralA);
            return _supportedMass * lateralA * lateralForceDirection / Time.fixedDeltaTime;
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

        public void SetSupportedMass(float mass)
        {
            if (_debugLog)
            {
                Debug.Log($"{transform.name} has supported mass of {mass}");
            }
            _supportedMass = mass;
        }

        public ISuspension GetSuspension()
        {
            return GetComponentInParent<ISuspension>();
        }
    }
}