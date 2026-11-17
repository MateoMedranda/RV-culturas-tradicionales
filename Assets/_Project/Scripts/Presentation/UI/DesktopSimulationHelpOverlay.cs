using UnityEngine;

namespace RvCulturas.Presentation.UI
{
    /// <summary>Ayuda visible únicamente para la prueba en Game View sin visor XR.</summary>
    public sealed class DesktopSimulationHelpOverlay : MonoBehaviour
    {
        private GUIStyle titleStyle;
        private GUIStyle bodyStyle;

        private void OnGUI()
        {
            EnsureStyles();
            var width = Mathf.Min(Screen.width - 32, 1060);
            GUI.Box(new Rect(16, 16, width, 76), GUIContent.none);
            GUI.Label(new Rect(32, 25, width - 32, 28), "SIMULADOR DE ESCRITORIO — RECORRIDO DE PRÁCTICA", titleStyle);
            GUI.Label(new Rect(32, 55, width - 32, 28), "W A S D: mover · clic derecho + ratón: mirar · clic izquierdo: seleccionar · Espacio: iniciar · P: pausa · G: repetir · R: reiniciar · C: cerrar ficha", bodyStyle);
        }

        private void EnsureStyles()
        {
            if (titleStyle != null) return;
            titleStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 18,
                fontStyle = FontStyle.Bold,
                normal = { textColor = new Color(1f, 0.8f, 0.25f) }
            };
            bodyStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 15,
                wordWrap = true,
                normal = { textColor = Color.white }
            };
        }
    }
}
