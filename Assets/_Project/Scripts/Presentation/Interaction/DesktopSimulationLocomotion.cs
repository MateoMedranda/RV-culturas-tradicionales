using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

namespace RvCulturas.Presentation.Interaction
{
    /// <summary>Movimiento exclusivo para comprobar la experiencia desde el Editor sin visor.</summary>
    public sealed class DesktopSimulationLocomotion : MonoBehaviour
    {
        [SerializeField] private Transform rig;
        [SerializeField] private Transform view;
        [SerializeField] private float moveSpeed = 2.4f;
        [SerializeField] private float lookSensitivity = 0.12f;
        private float pitch;

        public void Configure(Transform configuredRig, Transform configuredView)
        {
            rig = configuredRig;
            view = configuredView;
        }

        private void Update()
        {
            // XR Device Simulator reports a virtual device as active in the Editor. The desktop
            // fallback must remain available there, but never run in a real device build.
            if (rig == null || view == null || (!UnityEngine.Application.isEditor && XRSettings.isDeviceActive)) return;
            var keyboard = Keyboard.current;
            if (keyboard == null) return;

            var direction = Vector3.zero;
            if (keyboard.wKey.isPressed) direction += Vector3.forward;
            if (keyboard.sKey.isPressed) direction += Vector3.back;
            if (keyboard.aKey.isPressed) direction += Vector3.left;
            if (keyboard.dKey.isPressed) direction += Vector3.right;
            if (direction.sqrMagnitude > 0f)
                rig.Translate(direction.normalized * moveSpeed * Time.deltaTime, Space.Self);

            var mouse = Mouse.current;
            if (mouse == null || !mouse.rightButton.isPressed) return;
            var delta = mouse.delta.ReadValue() * lookSensitivity;
            rig.Rotate(Vector3.up, delta.x, Space.World);
            pitch = Mathf.Clamp(pitch - delta.y, -45f, 45f);
            view.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }
    }
}
