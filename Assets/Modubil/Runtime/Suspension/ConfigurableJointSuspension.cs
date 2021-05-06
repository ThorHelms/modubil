using UnityEngine;

namespace Assets.Modubil.Runtime.Suspension
{
    public class ConfigurableJointSuspension : MonoBehaviour, ISuspension
    {
        private ConfigurableJoint _joint;

        private void Start()
        {
            _joint = GetComponentInParent<ConfigurableJoint>();
        }

        public float GetForceMagnitude()
        {
            if (_joint == null)
            {
                return 0;
            }

            var jointDeltaPos = _joint.transform.localPosition - _joint.targetPosition;

            return new Vector3(
                jointDeltaPos.x * _joint.xDrive.positionSpring,
                jointDeltaPos.y * _joint.yDrive.positionSpring,
                jointDeltaPos.z * _joint.zDrive.positionSpring
            ).magnitude;
        }
    }
}