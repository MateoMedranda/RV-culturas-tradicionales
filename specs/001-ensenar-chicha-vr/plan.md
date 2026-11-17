# Implementation Plan: Iteración 001 - Enseñanza de chicha en realidad virtual

**Branch**: `main` | **Date**: 2026-11-17 | **Spec**: [spec.md](spec.md)

**Input**: Especificación de la primera experiencia inmersiva inclusiva de preparación de chicha.

## Summary

Entregar un corte vertical de la experiencia educativa: una estación de preparación segura y
accesible, un paso interactivo completo, contenido cultural provisional trazable y guías que se
puedan completar con el audio desactivado. Unity 6.3 LTS, URP, OpenXR y XR Interaction Toolkit
aportarán la capa de presentación; las reglas de pasos y progreso permanecerán independientes de
Unity para permitir pruebas y crecimiento posterior.

## Technical Context

**Language/Version**: C# soportado por Unity 6.3 LTS.

**Primary Dependencies**: Universal Render Pipeline, XR Plugin Management, Unity OpenXR Plugin,
Input System, XR Interaction Toolkit 3.x estable, TextMeshPro y Unity Test Framework.

**Storage**: Activos ScriptableObject para contenido estático; estado de sesión solo en memoria;
sin cuentas, backend ni persistencia de datos personales en la Iteración 001.

**Testing**: Unity Test Framework: Edit Mode para dominio y aplicación; Play Mode para flujo de
escena, interacción XR, pausa, reinicio y retroalimentación. Lista manual en visor para
accesibilidad, comodidad y rendimiento.

**Target Platform**: Desarrollo en Windows 10/11 con XR Device Simulator; despliegue provisional
en Meta Quest 3/3S mediante OpenXR, pendiente de prueba real en el visor disponible.

**Project Type**: Cliente inmersivo 3D de Unity, sin servicios remotos.

**Performance Goals**: Mantener 72 Hz (máximo 13.89 ms por cuadro) en el dispositivo autónomo
objetivo; sin asignaciones evitables por cuadro en la interacción; cero errores o advertencias
críticas durante el recorrido de validación.

**Constraints**: Uso sin conexión, idioma español, una estación de trabajo sentada o de pie,
instrucciones y resultados disponibles sin audio, sin locomoción continua, contenido cultural
sin publicar hasta contar con fuente y revisión, y recursos visuales optimizados para VR.

**Scale/Scope**: Una escena greybox, una introducción, un paso verificable, un ingrediente, un
recipiente y un utensilio interactivo; tres explicaciones culturales como máximo; una persona
usuaria por sesión.

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

| Principio | Evidencia en el diseño | Estado |
| --- | --- | --- |
| Arquitectura limpia | Domain y Application no referencian Unity; Presentation adapta XR y escenas; Infrastructure carga datos. | PASS |
| Experiencia VR segura e inclusiva | Estación compacta, sin locomoción continua, uso sentado/de pie, interacción a distancia y flujo completo sin audio. | PASS |
| Calidad verificable | Pruebas Edit Mode y Play Mode, más una guía de validación en visor. | PASS |
| Rendimiento VR | URP, iluminación horneada, materiales simples, presupuesto de 72 Hz y perfilado en dispositivo. | PASS |
| Simplicidad y contenido responsable | Un único paso y datos configurables; no se publica contenido específico sin fuente y revisión cultural. | PASS |

No hay violaciones ni excepciones de la constitución en la fase de investigación.

## Project Structure

### Documentation (this feature)

```text
specs/001-ensenar-chicha-vr/
├── plan.md              # This file ($speckit-plan command output)
├── research.md          # Phase 0 output ($speckit-plan command)
├── data-model.md        # Phase 1 output ($speckit-plan command)
├── quickstart.md        # Phase 1 output ($speckit-plan command)
├── contracts/           # Phase 1 output ($speckit-plan command)
└── tasks.md             # Phase 2 output ($speckit-tasks command - NOT created by $speckit-plan)
```

### Source Code (repository root)
<!--
  ACTION REQUIRED: Replace the placeholder tree below with the concrete layout
  for this feature. Delete unused options and expand the chosen structure with
  real paths (e.g., apps/admin, packages/something). The delivered plan must
  not include Option labels.
-->

```text
Assets/
├── _Project/
│   ├── Art/
│   │   ├── Materials/
│   │   ├── Models/
│   │   └── Textures/
│   ├── Content/
│   │   └── Experiences/
│   ├── Prefabs/
│   ├── Scenes/
│   ├── Scripts/
│   │   ├── Domain/
│   │   ├── Application/
│   │   ├── Infrastructure/
│   │   └── Presentation/
│   └── UI/
├── Tests/
│   ├── EditMode/
│   └── PlayMode/
└── XR/
    └── Input/

Packages/
ProjectSettings/
docs/
specs/
```

**Structure Decision**: Un único proyecto Unity. `Assets/_Project/Scripts/Domain` contiene
entidades y reglas C# puras; `Application` orquesta casos de uso; `Infrastructure` adapta carga
de activos y fuentes; `Presentation` contiene MonoBehaviours, XR, UI y composición de escena.
Los recursos de Blender se conservan con su procedencia y licencia fuera de la lógica de negocio.

## Complexity Tracking

No se requieren justificaciones de complejidad: la Iteración 001 usa un único proyecto y evita
servicios, paquetes específicos de fabricante y abstracciones no necesarias.
