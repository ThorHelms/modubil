using UnityEngine;

namespace Assets.Modubil.Runtime.CollisionDetection
{
    public interface IWheelCollisionDetector
    {
        bool TryGetCollision(out Vector3 point, out Vector3 normal, out Collider collider, out Rigidbody rigidbody, out Transform transform);
    }
}