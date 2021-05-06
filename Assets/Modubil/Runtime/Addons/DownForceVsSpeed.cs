using UnityEngine;

namespace Assets.Modubil.Runtime.Addons { 
    public class DownForceVsSpeed : MonoBehaviour
    {
        [Tooltip("The down force based on the speed (KMPH)")]
        [SerializeField] AnimationCurve curve = AnimationCurve.Linear(0, 0, 250, 2500);

        private Rigidbody _rigidbody;

        private void Start()
        {
            _rigidbody = GetComponentInParent<Rigidbody>();
        }

        private void Update() {
            var downForce = GetDownForceAtSpeed(_rigidbody.velocity.magnitude * 3.6f);
            _rigidbody.AddForce(-Vector3.up * downForce);
        }

        private float GetDownForceAtSpeed(float speed) {
            return curve.Evaluate(speed);
        }
    }
}

