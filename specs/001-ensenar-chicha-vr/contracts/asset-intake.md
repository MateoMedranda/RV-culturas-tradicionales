# Contrato de incorporación de recursos 3D

## Propósito

Establece el ingreso seguro y trazable de modelos creados en Blender o recibidos de terceros. Se
aplica a modelos, texturas, imágenes y sonidos que aparezcan en la Iteración 001.

## Requisitos de entrega

| Elemento | Requisito |
| --- | --- |
| Archivo fuente | Conservar el archivo original y su versión fuera de la lógica de interacción. |
| Recurso importable | Entregar un formato compatible con Unity y un nombre estable. |
| Escala | Aplicar escala coherente con 1 unidad de Unity = 1 metro. |
| Orientación | Revisar orientación, pivote y tamaño antes de crear prefabs. |
| Colisionadores | Declarar si requiere interacción directa, interacción a distancia o solo decoración. |
| Materiales | Usar materiales compatibles con URP; evitar transparencia y shaders costosos sin justificación. |
| Optimización | Registrar estado de malla, texturas, materiales y pruebas de perfilado. |
| Procedencia | Registrar autoría, licencia o permiso y revisión cultural cuando aplique. |

## Criterios de aceptación por recurso

- El recurso puede cargarse sin errores en el proyecto.
- Tiene escala, pivote y colisionador adecuados para su acción prevista.
- No introduce dependencias no aprobadas ni materiales incompatibles.
- Mantiene su referencia de procedencia y licencia.
- Si es interactivo, puede generar el identificador de objetivo definido en el modelo de datos.
- Si es decorativo, no participa en la validación del paso.

## Alcance de la Iteración 001

La primera escena admite solo los recursos necesarios para una estación greybox: un ingrediente,
un recipiente, un utensilio y decoración mínima. Modelos finales y una cocina completa se
incorporarán después de validar el flujo y el presupuesto de rendimiento.
