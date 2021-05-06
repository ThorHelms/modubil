using UnityEngine;

namespace Assets.Modubil.Runtime.Addons{
    public class MidAirSteering : MonoBehaviour
    {
        [Header("Mid Air Steer")]
        public float midAirSteerTorque = 1500;
        public float midAirSteerInput;
        private Rigidbody _rigidbody;

        private void Start()
        {
            _rigidbody = GetComponentInParent<Rigidbody>();
        }

        private void FixedUpdate() {
            SteerMidAir();
        }

        private void SteerMidAir() {
            if (!Mathf.Approximately(midAirSteerInput, 0))
                _rigidbody.AddTorque(new Vector3(0, midAirSteerInput * midAirSteerTorque, 0));
        }
    }
}
