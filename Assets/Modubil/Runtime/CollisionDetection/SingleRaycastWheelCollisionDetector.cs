using Assets.Modubil.Runtime.Wheel;
using UnityEngine;

namespace Assets.Modubil.Runtime.CollisionDetection
{
    public class SingleRaycastWheelCollisionDetector : MonoBehaviour, IWheelCollisionDetector
    {
        [SerializeField] private LayerMask _raycastLayers;

        private IWheel _wheel;

        private bool _rayHit = false;
        private RaycastHit _hit;

        private void Start()
        {
            _wheel = GetComponentInParent<IWheel>();
        }

        private void FixedUpdate()
        {
            if (_wheel == null)
            {
                _rayHit = false;
                return;
            }

            _rayHit = Physics.Raycast(transform.position, transform.up * -1, out _hit, _wheel.GetRadius(), _raycastLayers);
        }

        public bool TryGetCollision(out Vector3 point, out Vector3 normal, out Collider c, out Rigidbody rb,
            out Transform t)
        {
            point = _hit.point;
            normal = transform.up;
            c = _hit.collider;
            rb = _hit.rigidbody;
            t = _hit.transform;

            return _rayHit;
        }
    }
}