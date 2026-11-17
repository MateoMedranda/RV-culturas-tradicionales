# Contrato de interacción en tiempo de ejecución

## Propósito

Este contrato separa Presentation de Application. La capa XR traduce gestos, controladores,
simulador y UI a comandos de aplicación; la aplicación devuelve resultados de dominio que
Presentation representa con objetos, texto, iconos, sonido opcional y háptica opcional.

## Comandos hacia Application

| Comando | Origen posible | Precondiciones | Resultado esperado |
| --- | --- | --- | --- |
| StartExperience | Botón de inicio | Sesión en Intro, experiencia válida | Presentar primer paso y guía esencial. |
| AttemptInteraction | Objeto XR o simulador | Sesión Activa, objetivo identificado | Aceptar o rechazar sin saltar pasos. |
| PauseExperience | Botón de pausa | Sesión Activa | Bloquear avance y mostrar estado pausado. |
| ResumeExperience | Botón de reanudar | Sesión Pausada | Restaurar el mismo paso activo. |
| RestartExperience | Botón de reinicio | Sesión abierta o finalizada | Reiniciar en Intro y limpiar progreso temporal. |
| RequestGuidance | Botón o UI | Sesión con paso activo | Repetir instrucción sin cambiar progreso. |
| ExitExperience | Botón de salida | Cualquier sesión abierta | Marcar sesión como Salida y limpiar estado temporal. |

## Resultados hacia Presentation

| Resultado | Datos mínimos | Representación obligatoria |
| --- | --- | --- |
| ExperienceStarted | experiencia, paso actual, guía | Texto e indicación visual del paso. |
| InteractionAccepted | objetivo, paso completado, siguiente paso o cierre | Texto e icono de éxito; sonido o háptica opcionales. |
| InteractionRejected | objetivo, paso activo, guía de corrección | Texto e icono de corrección; el color no puede ser la única señal. |
| ExperiencePaused | paso actual | Indicador visual de pausa y bloqueo de acciones. |
| GuidancePresented | guía repetida | Texto y señal visual legibles. |
| ExperienceCompleted | resumen de pasos | Resumen visual y mensaje de preservación cultural. |
| ExperienceExited | estado de salida | Retorno a una vista segura de inicio. |

## Reglas del contrato

- Presentation no decide si una acción es correcta; delega esa regla a Application.
- Application no recibe referencias a GameObject, MonoBehaviour, controlador ni visor.
- Toda respuesta necesaria para avanzar incluye equivalencia textual y visual.
- La interacción a distancia y la interacción directa emiten el mismo AttemptInteraction.
- Los adaptadores pueden añadir audio o háptica, pero no cambiar el resultado de dominio.
