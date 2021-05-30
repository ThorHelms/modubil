using Assets.Modubil.Runtime.Suspension;

namespace Assets.Modubil.Runtime.Wheel
{
    public interface IWheelFriction
    {
        void SetSupportedMass(float mass);
        ISuspension GetSuspension();
    }
}