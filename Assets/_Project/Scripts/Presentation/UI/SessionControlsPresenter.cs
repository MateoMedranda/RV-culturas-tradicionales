using UnityEngine;
using UnityEngine.InputSystem;
using RvCulturas.Presentation.Bootstrap;

namespace RvCulturas.Presentation.UI
{
    public sealed class SessionControlsPresenter : MonoBehaviour
    {
        [SerializeField] private ExperienceBootstrapper bootstrapper;
        [SerializeField] private TextMesh stateText;
        [SerializeField] private InputActionAsset inputActions;
        private InputAction startAction;
        private InputAction pauseAction;
        private InputAction restartAction;
        private InputAction repeatAction;
        private InputAction closeContextAction;

        public void Configure(ExperienceBootstrapper value, InputActionAsset actions = null)
        {
            bootstrapper = value;
            if (actions != null) inputActions = actions;
        }

        private void OnEnable()
        {
            BindActions();
        }

        private void Start()
        {
            if (stateText != null) stateText.text = "Espacio: iniciar | P: pausa | G: repetir | R: reiniciar | C: cerrar ficha";
        }

        private void OnDisable()
        {
            UnbindActions();
        }

        private void BindActions()
        {
            if (inputActions == null) return;
            var map = inputActions.FindActionMap("Experience", false);
            if (map == null) return;
            startAction = map.FindAction("Start", false);
            pauseAction = map.FindAction("Pause", false);
            restartAction = map.FindAction("Restart", false);
            repeatAction = map.FindAction("RepeatGuidance", false);
            closeContextAction = map.FindAction("CloseContext", false);
            if (startAction != null) startAction.performed += StartPerformed;
            if (pauseAction != null) pauseAction.performed += PausePerformed;
            if (restartAction != null) restartAction.performed += RestartPerformed;
            if (repeatAction != null) repeatAction.performed += RepeatPerformed;
            if (closeContextAction != null) closeContextAction.performed += CloseContextPerformed;
            inputActions.Enable();
        }

        private void UnbindActions()
        {
            if (startAction != null) startAction.performed -= StartPerformed;
            if (pauseAction != null) pauseAction.performed -= PausePerformed;
            if (restartAction != null) restartAction.performed -= RestartPerformed;
            if (repeatAction != null) repeatAction.performed -= RepeatPerformed;
            if (closeContextAction != null) closeContextAction.performed -= CloseContextPerformed;
        }

        private void StartPerformed(InputAction.CallbackContext _) => bootstrapper?.StartExperience();
        private void PausePerformed(InputAction.CallbackContext _) => bootstrapper?.TogglePause();
        private void RestartPerformed(InputAction.CallbackContext _) => bootstrapper?.RestartExperience();
        private void RepeatPerformed(InputAction.CallbackContext _) => bootstrapper?.RepeatGuidance();
        private void CloseContextPerformed(InputAction.CallbackContext _) => bootstrapper?.CloseCulturalContext();
    }
}
