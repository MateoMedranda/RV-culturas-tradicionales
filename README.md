# RV Culturas Tradicionales

Aplicación educativa inmersiva para concientizar sobre tradiciones culinarias del Ecuador.
La primera experiencia guía a cada visitante, paso a paso, en la preparación de una versión
tradicional de chicha y explica el valor cultural de sus ingredientes, herramientas y prácticas.

## Estado

El repositorio se encuentra en la fase de definición. Aún no contiene un proyecto de ejecución;
contiene la constitución, la especificación funcional y la documentación que guiarán su creación.

## Alcance de la primera experiencia

- Aprendizaje guiado y manipulable de la preparación de chicha.
- Entorno culturalmente respetuoso y modelos tridimensionales de ingredientes, utensilios y
  espacios relevantes.
- Instrucciones claras, retroalimentación inmediata, controles de confort y alternativas
  accesibles.
- Contenido revisable mediante fuentes y validación cultural antes de su publicación.

No forman parte de esta primera entrega la venta de productos, perfiles de usuario, contenido de
recetas adicionales ni una recreación exhaustiva de todas las variantes de chicha.

## Estructura del repositorio

| Ruta | Propósito |
| --- | --- |
| `.agents/skills/` | Habilidades de Spec Kit disponibles para el equipo. |
| `.specify/` | Configuración, plantillas, flujo de trabajo y constitución del proyecto. |
| `specs/001-ensenar-chicha-vr/` | Especificación funcional y checklist de calidad de la primera experiencia. |
| `docs/` | Documentación del producto, arquitectura propuesta, repositorio y fuentes. |
| `README.md` | Punto de entrada de la documentación. |

## Flujo de trabajo

1. Revisar la [constitución](.specify/memory/constitution.md) y la
   [especificación](specs/001-ensenar-chicha-vr/spec.md).
2. Ejecutar `$speckit-plan` para decidir el diseño técnico y validar el alcance.
3. Ejecutar `$speckit-tasks` para convertir el plan en tareas implementables.
4. Ejecutar `$speckit-implement` únicamente después de aprobar el plan y las tareas.

## Documentación relacionada

- [Visión del producto](docs/PRODUCT_OVERVIEW.md)
- [Arquitectura propuesta](docs/ARCHITECTURE.md)
- [Guía del repositorio](docs/REPOSITORY_GUIDE.md)
- [Fuentes y validación cultural](docs/SOURCES.md)

## Principios

Las decisiones del proyecto se rigen por la arquitectura limpia, seguridad y accesibilidad en
experiencias inmersivas, calidad verificable, rendimiento y representación cultural responsable.
La fuente normativa es la [constitución](.specify/memory/constitution.md).
