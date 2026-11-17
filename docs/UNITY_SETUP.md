# Configuración de Unity

## Versión base

El proyecto se crea con el editor instalado Unity 6000.6.4f1. Mantener esta versión para la
Iteración 001 evita que se reserialicen escenas o activos durante el desarrollo.

## Paquetes

El manifiesto declara URP, Input System, OpenXR, XR Interaction Toolkit y Unity Test Framework.
Después de abrir el proyecto, esperar a que Package Manager termine de resolver los paquetes antes
de editar la escena.

## Apertura y prueba local

1. Abrir la raíz del repositorio como proyecto en Unity Hub.
2. Esperar a que no aparezcan errores de compilación.
3. Abrir Assets/_Project/Scenes/Iteration001.unity.
4. En el Editor, usar el simulador XR o el control alternativo de ratón para recorrer el paso.
5. Para visor, crear una Development Build y seguir quickstart.md.

## Convenciones

- Domain y Application no pueden depender de UnityEngine.
- La escena solo compone presentadores, prefabs y datos; no contiene reglas de negocio.
- Los recursos creados en Blender deben tener procedencia, licencia, escala y optimización
  registradas antes de ser aceptados.
