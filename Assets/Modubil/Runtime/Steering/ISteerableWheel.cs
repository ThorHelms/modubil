using UnityEngine;

namespace Assets.Modubil.Runtime.Steering
{
    public interface ISteerableWheel
    {
        void ResetSteering();
        void SteerTowards(Vector3 turningPoint);
        float GetTurningRadius();
        float GetTurningAngle();
    }
}