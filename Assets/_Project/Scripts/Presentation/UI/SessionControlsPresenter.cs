using UnityEngine;
using RvCulturas.Presentation.Bootstrap;

namespace RvCulturas.Presentation.UI
{
    public sealed class SessionControlsPresenter : MonoBehaviour
    {
        [SerializeField] private ExperienceBootstrapper bootstrapper;
        [SerializeField] private TextMesh stateText;

        public void Configure(ExperienceBootstrapper value) => bootstrapper = value;

        private void Update()
        {
            if (bootstrapper == null) return;
            if (Input.GetKeyDown(KeyCode.Space)) bootstrapper.StartExperience();
            if (Input.GetKeyDown(KeyCode.P)) bootstrapper.TogglePause();
            if (Input.GetKeyDown(KeyCode.R)) bootstrapper.RestartExperience();
            if (Input.GetKeyDown(KeyCode.G)) bootstrapper.RepeatGuidance();
            if (stateText != null) stateText.text = "Espacio: iniciar | P: pausa | G: repetir | R: reiniciar";
        }
    }
}
