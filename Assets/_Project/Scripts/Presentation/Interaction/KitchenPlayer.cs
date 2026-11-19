using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using CommonUsages = UnityEngine.XR.CommonUsages;

namespace RvCulturas.Presentation.Interaction
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class KitchenPlayer : MonoBehaviour
    {
        public Camera view;
        public Transform leftHand, rightHand;
        public bool seated;
        public bool movementLocked;
        public float sensitivity = 0.09f;
        private CharacterController body;
        private float yaw, pitch = 14;
        private bool previousTrigger;
        public bool IsXr { get; private set; }
        public bool Selected { get; private set; }
        public Ray Pointer { get; private set; }
        private void Awake() { body = GetComponent<CharacterController>(); yaw = transform.eulerAngles.y; }
        private void Update()
        {
            var head = InputDevices.GetDeviceAtXRNode(XRNode.Head);
            IsXr = XRSettings.isDeviceActive && head.isValid;
            Selected = false;
            if (IsXr)
            {
                if (head.TryGetFeatureValue(CommonUsages.devicePosition, out var hp)) view.transform.localPosition = hp + Vector3.up * (seated ? 0.4f : 0f);
                if (head.TryGetFeatureValue(CommonUsages.deviceRotation, out var hr)) view.transform.localRotation = hr;
                TrackHand(XRNode.LeftHand, leftHand);
                TrackHand(XRNode.RightHand, rightHand);
                var device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
                if (!device.isValid) device = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
                device.TryGetFeatureValue(CommonUsages.triggerButton, out var pressed);
                Selected = pressed && !previousTrigger;
                previousTrigger = pressed;
                Pointer = new Ray((device.characteristics & InputDeviceCharacteristics.Left) != 0 ? leftHand.position : rightHand.position,
                    (device.characteristics & InputDeviceCharacteristics.Left) != 0 ? leftHand.forward : rightHand.forward);
                Cursor.lockState = CursorLockMode.None;
                return;
            }
            view.transform.localPosition = new Vector3(0, seated ? 1.3f : 1.65f, 0);
            var mouse = Mouse.current;
            var key = Keyboard.current;
            if (mouse != null && mouse.rightButton.isPressed && !movementLocked)
            {
                Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false;
                var delta = mouse.delta.ReadValue() * sensitivity;
                yaw += delta.x; pitch = Mathf.Clamp(pitch - delta.y, -65, 65);
            }
            else { Cursor.lockState = CursorLockMode.None; Cursor.visible = true; }
            transform.rotation = Quaternion.Euler(0, yaw, 0);
            view.transform.localRotation = Quaternion.Euler(pitch, 0, 0);
            var move = Vector3.zero;
            if (key != null && !movementLocked)
            {
                if (key.wKey.isPressed || key.upArrowKey.isPressed) move += transform.forward;
                if (key.sKey.isPressed || key.downArrowKey.isPressed) move -= transform.forward;
                if (key.aKey.isPressed || key.leftArrowKey.isPressed) move -= transform.right;
                if (key.dKey.isPressed || key.rightArrowKey.isPressed) move += transform.right;
            }
            body.Move((Vector3.ClampMagnitude(move, 1) * 1.6f + Vector3.down * 3) * Time.deltaTime);
            leftHand.SetPositionAndRotation(view.transform.TransformPoint(new Vector3(-0.25f, -0.10f, 0.5f)), view.transform.rotation * Quaternion.Euler(0, 0, -12));
            rightHand.SetPositionAndRotation(view.transform.TransformPoint(new Vector3(0.25f, -0.10f, 0.5f)), view.transform.rotation * Quaternion.Euler(0, 0, 12));
            var point = mouse != null && Cursor.lockState != CursorLockMode.Locked ? mouse.position.ReadValue() : new Vector2(Screen.width / 2f, Screen.height / 2f);
            Pointer = view.ScreenPointToRay(point);
            Selected = !movementLocked && ((mouse != null && mouse.leftButton.wasPressedThisFrame) || (key != null && key.eKey.wasPressedThisFrame));
        }
        private void TrackHand(XRNode node, Transform hand)
        {
            var device = InputDevices.GetDeviceAtXRNode(node);
            if (device.TryGetFeatureValue(CommonUsages.devicePosition, out var p)) hand.localPosition = p;
            if (device.TryGetFeatureValue(CommonUsages.deviceRotation, out var q)) hand.localRotation = q;
        }
        private void OnDisable() { Cursor.lockState = CursorLockMode.None; Cursor.visible = true; }
    }
}
