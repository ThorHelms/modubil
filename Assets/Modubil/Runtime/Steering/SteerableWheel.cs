using UnityEngine;

namespace Assets.Modubil.Runtime.Steering
{
    public class SteerableWheel : MonoBehaviour, ISteerableWheel
    {
        private Vector3? _turningPoint { get; set; }

        private void FixedUpdate()
        {
            CalculateSteering();
        }

        public void SteerTowards(Vector3 turningPoint)
        {
            _turningPoint = turningPoint;
        }

        public float GetTurningRadius()
        {
            if (_turningPoint == null)
            {
                return float.MaxValue;
            }

            return (transform.position - _turningPoint.Value).magnitude;
        }

        public float GetTurningAngle()
        {
            return transform.localEulerAngles.y;
        }

        public void ResetSteering()
        {
            _turningPoint = null;
        }

        private void CalculateSteering()
        {
            if (_turningPoint == null)
            {
                transform.localEulerAngles = new Vector3(
                    transform.localEulerAngles.x,
                    0,
                    transform.localEulerAngles.z);
            }
            else
            {
                var localEulerAngle = transform.localEulerAngles;
                transform.LookAt(_turningPoint.Value);
                var steerAngle = transform.localEulerAngles.y % 180 - 90;
                transform.localEulerAngles = new Vector3(
                    localEulerAngle.x,
                    steerAngle,
                    localEulerAngle.z
                );
            }
        }
    }
}