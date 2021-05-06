using Assets.Modubil.Runtime.Power;
using Assets.Modubil.Runtime.Steering;
using UnityEngine;

namespace Assets.Modubil.Samples.Scripts {
    public class DemoInputSource : MonoBehaviour {
        [Tooltip("Should point to a vehicle that has an IMotor and ISteering component, either on itself or as a child component somewhere.")]
        [SerializeField] private GameObject _vehicle;
        private IMotor _motor;
        private ISteering _steering;

        private void Start()
        {
            if (_vehicle != null)
            {
                _motor = _vehicle.GetComponentInChildren<IMotor>();
                _steering = _vehicle.GetComponentInChildren<ISteering>();
            }
        }

        private void Update(){
            _motor?.SetThrottle(Input.GetAxis("Vertical"));
            _steering?.SetSteering(Input.GetAxis("Horizontal"));
        }
    }
}
