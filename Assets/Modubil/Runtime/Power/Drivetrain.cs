using UnityEngine;

namespace Assets.Modubil.Runtime.Power
{
    public class Drivetrain : MonoBehaviour, IDrivetrain
    {
        private IPoweredAxle[] _axles;

        private void Start()
        {
            _axles = GetComponentsInChildren<IPoweredAxle>();
        }

        public void ApplyMotorTorque(float torque)
        {
            // TODO: Apply torque according to how much torque each axle can receive, see code below
            foreach (var axle in _axles)
            {
                axle.ApplyTorque(torque / _axles.Length);
            }
            
            /*var frontMaxTorque = FrontAxle.GetMaxTorque();
            var backMaxTorque = BackAxle.GetMaxTorque();

            var absTorque = Mathf.Abs(torque);
            var maxTorquePerAxle = absTorque / 2;

            var frontTorque = maxTorquePerAxle;
            var backTorque = maxTorquePerAxle;

            if (frontMaxTorque < maxTorquePerAxle)
            {
                frontTorque = frontMaxTorque;
                backTorque = Mathf.Clamp(absTorque - frontMaxTorque, 0, backMaxTorque);
            }
            else if (backMaxTorque < maxTorquePerAxle)
            {
                frontTorque = Mathf.Clamp(absTorque - backMaxTorque, 0, frontMaxTorque);
                backTorque = backMaxTorque;
            }

            var sign = torque < 0 ? -1 : 1;

            FrontAxle.ApplyTorque(frontTorque * sign);
            BackAxle.ApplyTorque(backTorque * sign);*/
        }
    }
}