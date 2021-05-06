namespace Assets.Modubil.Runtime.Steering
{
    public interface ISteering
    {
        /// <summary>
        /// Set steering-value, between -1 and 1.
        /// </summary>
        /// <param name="steeringValue">Steering value, between -1 and 1. -1 means turn left, 1 means turns right, 0 means neutral steering.</param>
        void SetSteering(float steeringValue);

        float GetMinTurningRadius();

        void SetMinTurningRadius(float minTurningRadius);
    }
}