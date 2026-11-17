# Contrato de autoría de contenido

## Propósito

Define qué información debe entregar una persona autora de contenido para incorporar una
experiencia, paso o elemento cultural. Sirve como límite entre contenido validado y la aplicación.

## Paquete mínimo de una experiencia

| Entregable | Obligatorio para el prototipo | Condición de aceptación |
| --- | --- | --- |
| Identificador y título de experiencia | Sí | Estables, únicos y en español. |
| Contexto cultural breve | Sí | No hace afirmaciones específicas sin fuente. |
| Fuente y revisión cultural | Sí para publicación | Origen, fecha, responsable y estado registrados. |
| Secuencia de pasos | Sí | Orden, acción esperada, resultado y elementos requeridos. |
| Instrucción visual y textual por paso | Sí | Comprensible con audio desactivado. |
| Explicación de elementos | Hasta tres | Fuente registrada antes de mostrarla como confirmada. |
| Recursos visuales | Sí, pueden ser placeholders | Cada recurso tiene AssetManifestEntry. |
| Audio | No | Si existe, tiene equivalente visual y textual. |

## Validación de un paso

Un paso se acepta para carga cuando:

1. Tiene identificador, ordinal, título, instrucción visual y textual.
2. Declara al menos un objetivo y un tipo de acción.
3. Define retroalimentación para éxito y corrección.
4. No contradice el orden de los demás pasos.
5. Sus elementos culturales y recursos tienen referencias válidas.
6. Puede completarse sin narración ni alerta sonora.

## Estados de publicación

- **Borrador**: utilizable en desarrollo interno con placeholders claramente identificados.
- **Validado**: fuente, revisión cultural y recursos listos para prueba de aceptación.
- **Bloqueado**: falta autorización, fuente, revisión o información necesaria; no se publica.

## Cambio de contenido

Modificar textos, explicaciones o orden de pasos requiere revisar las pruebas de secuencia. Cambiar
procedencia, fuente o representación cultural requiere una nueva revisión antes de pasar a
Validado.
