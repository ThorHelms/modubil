namespace Assets.Modubil.Runtime.Power
{
    public interface IMotor
    {
        /// <summary>
        /// Set the throttle, between -1 and 1.
        /// </summary>
        /// <param name="throttle">Throttle between -1 and 1. -1 means full power in reverse, 1 means full power forward, 0 means no power.</param>
        void SetThrottle(float throttle);
    }
}