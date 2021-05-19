﻿using Assets.Modubil.Runtime.CollisionDetection;
using Assets.Modubil.Runtime.Power;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Assets.Modubil.Runtime.Wheel {
    public class WheelFriction : MonoBehaviour, IPoweredWheel, IRpmProvider {
        [Tooltip("A height offset for applying forces, to prevent the vehicle from rolling as much. Should be between 0 and 1. 0 means hit height, 1 means center of mass height.")]
        [SerializeField] private float _applyForcesOffset;
        [SerializeField] private bool _applyForcesAtCom;
        [SerializeField] private float _lateralFrictionMultiplier = 300; // How much weight this wheel should try to turn with
        [SerializeField] private bool _debugLog;

        private Rigidbody _rb;
        private IWheel _wheel;
        private IWheelCollisionDetector _wheelCollisionDetector;

        private float _rpm;
        private Vector3 velocity;
        private float motorTorque;

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

            var lateralM = _lateralFrictionMultiplier;
            var lateralA = Vector3.Project(right, slip).magnitude * lateralVelocity.magnitude * -Vector3.Project(slip, lateralVelocity).normalized;
            var lateralF = lateralM * lateralA / Time.fixedDeltaTime;

            var localPoint = transform.InverseTransformPoint(point);
            var localCom = transform.InverseTransformPoint(_rb.worldCenterOfMass);
            var forceY = Mathf.Lerp(localPoint.y, localCom.y, _applyForcesOffset);
            var localForcePoint = new Vector3(localPoint.x, forceY, localPoint.z);
            var forcePoint = transform.TransformPoint(localForcePoint);

            if (_debugLog)
            {
                Debug.Log($"Lat A for {transform.name}: {lateralA}");
                Debug.DrawRay(forcePoint, lateralF, Color.magenta, 1);
            }

            var forceToAdd = lateralF;

            var motorForce = Mathf.Abs(motorTorque / _wheel.GetRadius());
            var maxForwardFriction = motorForce;
            var appliedForwardFriction = Mathf.Clamp(motorForce, 0, maxForwardFriction);

            var forwardForce = direction.normalized * appliedForwardFriction;

            forceToAdd += forwardForce;

            if (_applyForcesAtCom)
            {
                _rb.AddForce(forceToAdd);
            }
            else
            {
                _rb.AddForceAtPosition(forceToAdd, forcePoint);
            }
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