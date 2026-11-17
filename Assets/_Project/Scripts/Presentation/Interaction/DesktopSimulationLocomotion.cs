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
        private float yaw;

        public void Configure(Transform configuredRig, Transform configuredView)
        {
            rig = configuredRig;
            view = configuredView;
            if (view == null) return;
            pitch = NormalizeAngle(view.eulerAngles.x);
            yaw = view.eulerAngles.y;
        }

        private void Update()
        {
            // XR Device Simulator reports a virtual device as active in the Editor. The desktop
            // fallback must remain available there, but never run in a real device build.
            if (view == null || (!UnityEngine.Application.isEditor && XRSettings.isDeviceActive)) return;
            var keyboard = Keyboard.current;
            if (keyboard == null) return;

            var forward = Vector3.ProjectOnPlane(view.forward, Vector3.up).normalized;
            var right = Vector3.ProjectOnPlane(view.right, Vector3.up).normalized;
            var direction = Vector3.zero;
            if (keyboard.wKey.isPressed) direction += forward;
            if (keyboard.sKey.isPressed) direction -= forward;
            if (keyboard.aKey.isPressed) direction -= right;
            if (keyboard.dKey.isPressed) direction += right;
            if (direction.sqrMagnitude > 0f)
                view.position += direction.normalized * moveSpeed * Time.deltaTime;

            var mouse = Mouse.current;
            if (mouse == null) return;
            if (mouse.rightButton.isPressed)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                var delta = mouse.delta.ReadValue() * lookSensitivity;
                yaw += delta.x;
                pitch = Mathf.Clamp(pitch - delta.y, -45f, 45f);
                view.rotation = Quaternion.Euler(pitch, yaw, 0f);
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        private static float NormalizeAngle(float value) => value > 180f ? value - 360f : value;
    }
}
