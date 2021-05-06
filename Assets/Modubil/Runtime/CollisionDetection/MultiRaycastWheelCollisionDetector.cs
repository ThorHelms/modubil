using Assets.Modubil.Runtime.Wheel;
using UnityEngine;

namespace Assets.Modubil.Runtime.CollisionDetection
{
    public class MultiRaycastWheelCollisionDetector : MonoBehaviour, IWheelCollisionDetector
    {
        [SerializeField] private int _numberRaysRound = 36;
        [SerializeField] private int _numberRaysWidth = 3;
        [SerializeField] private float _angleLimit = 180;
        [SerializeField] private LayerMask _raycastLayers;

        private IWheel _wheel;

        private bool _foundHit;
        private Vector3 _hitPoint = Vector3.zero;
        private Vector3 _hitNormal = Vector3.zero;
        private Collider _hitCollider = null;
        private Rigidbody _hitRigidbody = null;
        private Transform _hitTransform = null;

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

            var radius = _wheel.GetRadius();
            var width = _wheel.GetWidth();

            var minDist = radius;

            for (var i = 0; i < _numberRaysRound; i++)
            {
                var angle = 180 - _angleLimit / 2 + i * _angleLimit / _numberRaysRound;
                var direction = Quaternion.AngleAxis(angle, transform.right) * transform.up;
                for (var j = 0; j < _numberRaysWidth; j++)
                {
                    var origin = transform.position;
                    if (_numberRaysWidth > 1)
                    {
                        origin -= transform.right * width / 2;
                        origin += transform.right * width * j / _numberRaysWidth;
                    }
                    var rayHit = Physics.Raycast(origin, direction, out _hit, minDist, _raycastLayers);
                    Debug.DrawRay(origin, direction.normalized * minDist, rayHit ? Color.cyan : Color.gray);

                    if (!rayHit) continue;

                    minDist = _hit.distance;

                    _foundHit = true;
                    _hitPoint = _hit.point;
                    _hitNormal = direction.normalized * -1;
                    _hitCollider = _hit.collider;
                    _hitRigidbody = _hit.rigidbody;
                    _hitTransform = _hit.transform;
                }
            }
        }

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