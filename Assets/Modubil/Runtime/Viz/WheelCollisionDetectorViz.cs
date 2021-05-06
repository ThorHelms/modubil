using Assets.Modubil.Runtime.CollisionDetection;
using UnityEngine;

namespace Assets.Modubil.Runtime.Viz
{
    public class WheelCollisionDetectorViz : MonoBehaviour
    {
        private IWheelCollisionDetector _wheelCollisionDetector;

        private void Start()
        {
            _wheelCollisionDetector = GetComponent<IWheelCollisionDetector>();

            if (_wheelCollisionDetector == null)
            {
                Debug.LogWarning($"Missing {nameof(IWheelCollisionDetector)} in {transform.name}");
            }
        }

        private void Update()
        {
            if (_wheelCollisionDetector == null)
            {
                return;
            }

            if (_wheelCollisionDetector.TryGetCollision(out var point, out var normal, out var collider, out var rb,
                out var t))
            {
                Debug.DrawRay(point, normal, Color.green);
            }
        }
    }
}