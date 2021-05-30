using System.Linq;
using Assets.Modubil.Runtime.Wheel;
using UnityEngine;

namespace Assets.Modubil.Runtime.Suspension
{
    public class MassDistributor : MonoBehaviour
    {
        private Rigidbody _rb;
        private IWheelFriction[] _wheels;
        private ISuspension[] _suspensions;

        private void Start()
        {
            _rb = GetComponentInParent<Rigidbody>();
            _wheels = GetComponentsInChildren<IWheelFriction>();
            _suspensions = _wheels.Select(x => x.GetSuspension()).ToArray();
        }

        private void FixedUpdate()
        {
            var totalSuspensionForce = _suspensions.Sum(x => x.GetForceMagnitude());

            for (var i = 0; i < _wheels.Length; i++)
            {
                var relativeSuspensionForce = totalSuspensionForce > 0
                    ? _suspensions[i].GetForceMagnitude() / totalSuspensionForce
                    : 0;
                _wheels[i].SetSupportedMass(_rb.mass * relativeSuspensionForce);
            }
        }
    }
}