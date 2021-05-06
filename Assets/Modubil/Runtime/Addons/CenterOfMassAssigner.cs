using UnityEngine;

namespace Assets.Modubil.Runtime.Addons {
    public class CenterOfMassAssigner : MonoBehaviour
    {
        private void Start()
        {
            var rb = GetComponentInParent<Rigidbody>();

            if (rb == null)
                return;
            rb.centerOfMass = rb.transform.InverseTransformPoint(transform.position);
        }
    }
}
