using UnityEngine;

namespace RvCulturas.Presentation.Interaction
{
    public sealed class KitchenTarget : MonoBehaviour
    {
        public string targetId;
        public string displayName;
        public Transform destination;
        private Vector3 initialPosition;
        private Quaternion initialRotation;
        private Renderer[] renderers;
        private void Awake() { initialPosition = transform.position; initialRotation = transform.rotation; renderers = GetComponentsInChildren<Renderer>(); }
        public void ResetPosition() { transform.SetPositionAndRotation(initialPosition, initialRotation); }
        public void Complete() { if (destination != null) transform.SetPositionAndRotation(destination.position, destination.rotation); }
        public Bounds VisualBounds
        {
            get
            {
                var bounds = new Bounds(transform.position, Vector3.zero);
                var first = true;
                if (renderers == null) renderers = GetComponentsInChildren<Renderer>();
                foreach (var renderer in renderers)
                {
                    if (first) { bounds = renderer.bounds; first = false; }
                    else bounds.Encapsulate(renderer.bounds);
                }
                return bounds;
            }
        }
    }
}
