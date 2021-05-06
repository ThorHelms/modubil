using UnityEngine;

namespace Assets.Modubil.Runtime.Power
{
    public class LockedPoweredAxle : MonoBehaviour, IPoweredAxle
    {
        private IPoweredWheel[] _wheels;

        private void Start()
        {
            _wheels = GetComponentsInChildren<IPoweredWheel>();
        }

        public void ApplyTorque(float torque)
        {
            foreach (var wheel in _wheels)
            {
                // TODO: Apply torque according to turning radius of each wheel
                // TODO: Respect the max-torque of each wheel
                wheel.ApplyTorque(torque / _wheels.Length);
            }
        }
    }
}