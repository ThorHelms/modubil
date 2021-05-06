using UnityEngine;

namespace Assets.Modubil.Samples.Scenes.Tests
{
    public class DebugJointForce : MonoBehaviour
    {
        private ConfigurableJoint _joint;

        // Start is called before the first frame update
        void Start()
        {
            _joint = GetComponent<ConfigurableJoint>();
        }

        // Update is called once per frame
        void Update()
        {
            if (_joint == null)
            {
                return;
            }

            var forces = _joint.currentForce;
            Debug.DrawRay(transform.position, forces, Color.magenta);
            Debug.DrawRay(transform.position, transform.up * forces.magnitude, Color.red);
            Debug.Log($"joint forces: ({forces.x:N2}, {forces.y:N2}, {forces.z:N2})");
        }
    }
}
