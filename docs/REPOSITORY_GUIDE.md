# Guía del repositorio

## Situación actual

Este repositorio contiene la base documental para iniciar el producto. No se han agregado
escenas, código fuente ni recursos tridimensionales; dichos elementos se incorporarán una vez
aprobado el plan de la funcionalidad.

## Artefactos de Spec Kit

- `.specify/memory/constitution.md`: reglas no negociables del proyecto.
- `.specify/templates/`: plantillas usadas por el flujo de especificación.
- `.specify/scripts/powershell/`: utilidades del flujo para Windows.
- `.agents/skills/`: comandos de Spec Kit disponibles en Codex.
- `specs/001-ensenar-chicha-vr/spec.md`: definición funcional vigente.
- `specs/001-ensenar-chicha-vr/checklists/requirements.md`: validación de calidad de esa
  definición.

## Convenciones de cambios

1. Describir el valor de una nueva funcionalidad en una especificación.
2. Revisar que la funcionalidad cumpla la constitución antes de planificarla.
3. Mantener separadas las decisiones de producto, arquitectura y tareas de implementación.
4. Incluir pruebas o evidencia de verificación con todo cambio de comportamiento.
5. Registrar las fuentes, licencias y validaciones de todo contenido cultural y recurso externo.

## Flujo local

```powershell
$speckit-plan
$speckit-tasks
$speckit-analyze
$speckit-implement
```

`$speckit-clarify` puede ejecutarse entre la especificación y el plan si cambia el alcance o si
se necesita confirmar decisiones que afecten a las personas usuarias.

## Control de versiones

Los documentos y la configuración de Spec Kit se versionan. Los directorios generados localmente
por herramientas de ejecución y las vistas temporales se excluyen mediante `.gitignore`. Antes
de enviar cambios, revisar `git status`, confirmar que no se incluyan archivos privados y crear
commits pequeños con mensajes que describan el propósito del cambio.
