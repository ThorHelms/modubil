using Assets.Modubil.Runtime.Suspension;
using UnityEngine;

namespace Assets.Modubil.Runtime.Wheel
{
    public class VisualWheelSync : MonoBehaviour
    {
        [SerializeField] private Transform _visualWheel;

        private ISuspensionDistance _suspensionDistance;
        private IRpmProvider _rpmProvider;

        private Vector3 _initialPosition;

        private void Start()
        {
            _suspensionDistance = GetComponentInParent<ISuspensionDistance>();
            _rpmProvider = GetComponentInParent<IRpmProvider>();
            _initialPosition = transform.localPosition;
        }

        private void Update()
        {
            UpdateSuspension();
            UpdateRotation();
        }

        private void UpdateSuspension()
        {
            if (_suspensionDistance == null)
            {
                return;
            }

            var suspensionDistance = _suspensionDistance.GetSuspensionDistance();

            transform.localPosition = _initialPosition + transform.up * suspensionDistance;
        }

        private void UpdateRotation()
        {
            if (_rpmProvider == null || _visualWheel == null)
            {
                return;
            }

            var rpm = _rpmProvider.GetRpm();
            var rotateThisTick = rpm / 60 * Time.deltaTime * 360;

            _visualWheel.Rotate(Vector3.right, rotateThisTick);
        }
    }
}