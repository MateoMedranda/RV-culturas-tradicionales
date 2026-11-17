# Modelo de datos - Iteración 001

## Propósito

El modelo mantiene el contenido de la experiencia separado de la interacción XR. Los activos de
contenido describen qué se enseña; el estado de sesión describe qué está haciendo la persona.
Ninguna regla de secuencia depende de escenas, prefabs, visor o controlador.

## Entidades de contenido

### ExperienceDefinition

Representa una experiencia educativa publicable.

| Campo | Tipo conceptual | Reglas |
| --- | --- | --- |
| id | Identificador estable | Único; no cambia al editar textos. |
| title | Texto | Obligatorio y en español para esta iteración. |
| culturalContext | Texto breve | Obligatorio; no debe afirmar procedencia específica sin fuente validada. |
| sourceReference | Referencia de fuente | Obligatoria antes de marcar el contenido como publicable. |
| reviewStatus | Borrador, validado, bloqueado | Solo el estado validado permite publicación. |
| steps | Lista ordenada de PreparationStep | Mínimo un paso; los ordinales son únicos y consecutivos. |
| culturalElements | Lista de CulturalElement | Cada elemento usado por un paso existe una sola vez. |

### PreparationStep

Representa una acción enseñable y verificable dentro del recorrido.

| Campo | Tipo conceptual | Reglas |
| --- | --- | --- |
| id | Identificador estable | Único dentro de la experiencia. |
| ordinal | Número entero | Empieza en 1 y define el orden de avance. |
| title | Texto breve | Describe la acción sin depender del audio. |
| visualInstruction | Texto e indicación visual | Obligatoria; visible al presentar el paso. |
| requiredTargets | Lista de identificadores | Contiene los objetos requeridos para validar la acción. |
| expectedAction | Tipo de acción | Para la iteración: tomar, colocar o usar. |
| successFeedback | GuidanceContent | Debe incluir texto o icono, no solo color o sonido. |
| correctionFeedback | GuidanceContent | Indica el paso activo y cómo reintentar. |
| completionCondition | Regla de dominio | Se satisface una sola vez por sesión. |

### CulturalElement

Representa un ingrediente, utensilio, práctica o espacio con información contextual.

| Campo | Tipo conceptual | Reglas |
| --- | --- | --- |
| id | Identificador estable | Único y referenciable desde un paso. |
| category | Ingrediente, utensilio, práctica o espacio | Obligatoria. |
| displayName | Texto | Obligatorio; requiere revisión antes de publicación. |
| explanation | Texto breve | Comprensible sin narración de audio. |
| assetKey | Identificador de recurso visual | Debe referir a un recurso incluido o a un placeholder permitido. |
| sourceReference | Referencia de fuente | Obligatoria antes de publicar la explicación. |
| reviewStatus | Borrador, validado o bloqueado | Un elemento bloqueado no se muestra como contenido cultural confirmado. |

### GuidanceContent

Agrupa la información accesible de una instrucción o resultado.

| Campo | Tipo conceptual | Reglas |
| --- | --- | --- |
| text | Texto | Obligatorio para toda guía esencial. |
| iconKey | Identificador opcional | Complementa el texto; no lo reemplaza. |
| visualCue | Tipo de señal visual | Obligatorio para éxito, corrección y progreso. |
| audioCue | Tipo de señal opcional | Nunca es la única indicación. |
| repeatable | Booleano | Verdadero para instrucciones de pasos. |

### AssetManifestEntry

Registra la procedencia y condiciones de un modelo, textura, audio o imagen.

| Campo | Tipo conceptual | Reglas |
| --- | --- | --- |
| assetKey | Identificador estable | Coincide con el recurso referenciado por contenido. |
| assetType | Modelo, textura, material, audio o imagen | Obligatorio. |
| source | Autor, repositorio o creación propia | Obligatorio. |
| licenseOrPermission | Licencia o permiso | Obligatorio antes de publicación. |
| culturalReview | Estado de revisión | Obligatorio para recursos que representen elementos culturales. |
| optimizationStatus | Pendiente, aprobado o bloqueado | Aprobado antes de incluir el recurso en la build objetivo. |

## Estado de sesión

### ExperienceSessionState

Existe solo en memoria durante un recorrido.

| Campo | Tipo conceptual | Reglas |
| --- | --- | --- |
| lifecycle | Intro, activo, pausado, completado o salido | Solo una transición válida por evento. |
| currentStepOrdinal | Número o vacío | Se asigna al iniciar la experiencia y avanza solo tras una acción válida. |
| completedStepIds | Conjunto de identificadores | No admite duplicados; no se persiste entre sesiones. |
| activeGuidance | GuidanceContent | Puede repetirse sin modificar el progreso. |
| selectedTargetId | Identificador opcional | Se valida contra el paso activo. |
| accessibilityMode | Configuración de guías | Las equivalencias visuales y textuales esenciales permanecen activas. |

## Relaciones

    ExperienceDefinition 1 ── * PreparationStep
    ExperienceDefinition 1 ── * CulturalElement
    ExperienceDefinition 1 ── * AssetManifestEntry
    PreparationStep * ── * CulturalElement
    PreparationStep 1 ── 1 GuidanceContent (instrucción)
    PreparationStep 1 ── 1 GuidanceContent (éxito)
    PreparationStep 1 ── 1 GuidanceContent (corrección)
    ExperienceSessionState 1 ── 1 ExperienceDefinition durante la sesión
    ExperienceSessionState 1 ── 1 PreparationStep activo

## Transiciones de estado

    Intro --StartExperience--> Activo
    Activo --Pause--> Pausado
    Pausado --Resume--> Activo
    Activo --ValidInteraction del paso actual--> Activo con siguiente paso
    Activo --ValidInteraction del último paso--> Completado
    Intro|Activo|Pausado --Restart--> Intro
    Intro|Activo|Pausado --Exit--> Salido
    Completado|Salido --Restart--> Intro

Las acciones inválidas no cambian el estado ni el ordinal; solo producen GuidanceContent de
corrección. Una acción no puede completar más de un paso.

## Reglas de validación transversales

- No cargar una ExperienceDefinition publicable si falta fuente o revisión de contexto cultural.
- No presentar una guía esencial sin texto y señal visual.
- No aceptar interacción cuando el ciclo de vida no sea Activo.
- No avanzar si el objetivo, tipo de acción o condición no corresponden al paso activo.
- No incluir un recurso en la build de aceptación si su manifiesto no tiene procedencia, permiso
  y estado de optimización aprobado.
