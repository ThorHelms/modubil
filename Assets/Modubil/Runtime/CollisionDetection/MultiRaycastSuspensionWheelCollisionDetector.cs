using Assets.Modubil.Runtime.Suspension;
using Assets.Modubil.Runtime.Wheel;
using UnityEngine;

namespace Assets.Modubil.Runtime.CollisionDetection
{
    public class MultiRaycastSuspensionWheelCollisionDetector : MonoBehaviour, IWheelCollisionDetector, ISuspensionDistance
    {
        [SerializeField] private int _numberRaysLength = 36;
        [SerializeField] private int _numberRaysWidth = 3;
        [SerializeField] private LayerMask _raycastLayers;
        [SerializeField] private float _suspensionTravel = 0.4f;

        private IWheel _wheel;

        private bool _foundHit;
        private Vector3 _hitPoint = Vector3.zero;
        private Vector3 _hitNormal = Vector3.zero;
        private Collider _hitCollider;
        private Rigidbody _hitRigidbody;
        private Transform _hitTransform;
        private float _suspensionDistance;

        private RaycastHit _hit;

        private void Start()
        {
            _wheel = GetComponentInParent<IWheel>();
        }

        private void FixedUpdate()
        {
            _foundHit = false;

            if (_wheel == null)
            {
                return;
            }

            var down = transform.up * -1;
            Debug.DrawRay(transform.position, down, Color.magenta);
            var minDist = _suspensionTravel;

            var radius = _wheel.GetRadius();
            var width = _wheel.GetWidth();

            for (var i = 0; i < _numberRaysLength; i++)
            {
                var x = radius * 2 * i / _numberRaysLength - radius;
                var y = Mathf.Sqrt(radius * radius - x * x);
                var localDirection = new Vector3(0, - y, x);
                var normalDirection = transform.TransformDirection(localDirection);
                //var normalDirection = Vector3.Cross(direction, transform.right).normalized;

                for (var j = 0; j < _numberRaysWidth; j++)
                {
                    var origin = transform.position + transform.up * radius / 2;

                    if (_numberRaysWidth > 1)
                    {
                        origin -= transform.right * width / 2;
                        origin += transform.right * width * j / _numberRaysWidth;
                    }

                    if (_numberRaysLength > 1)
                    {
                        origin += transform.forward * x;
                        origin += transform.up * _suspensionTravel / 2;
                        origin -= transform.up * y;
                    }

                    var rayHit = Physics.Raycast(origin, down, out _hit, minDist, _raycastLayers);
                    Debug.DrawRay(origin, down * minDist, rayHit ? Color.cyan : Color.gray);

                    if (!rayHit) continue;

                    _foundHit = true;
                    _hitPoint = _hit.point;
                    _hitNormal = normalDirection; // TODO: Is this correct?
                    _hitCollider = _hit.collider;
                    _hitRigidbody = _hit.rigidbody;
                    _hitTransform = _hit.transform;
                    minDist = _hit.distance;
                }
            }

            _suspensionDistance = _foundHit ? _suspensionTravel - minDist : 0;
        }

        public float GetSuspensionDistance() => _suspensionDistance;

        public bool TryGetCollision(out Vector3 point, out Vector3 normal, out Collider collider, out Rigidbody rigidbody,
            out Transform transform)
        {
            point = _hitPoint;
            normal = _hitNormal;
            collider = _hitCollider;
            rigidbody = _hitRigidbody;
            transform = _hitTransform;
            return _foundHit;
        }
    }
}