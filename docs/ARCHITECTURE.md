# Arquitectura propuesta

## Objetivo

La aplicación se construirá con una estructura limpia y escalable. La lógica educativa y las
reglas de la preparación deben evolucionar sin quedar ligadas a escenas, visores, controles o
recursos visuales específicos.

## Capas

| Capa | Responsabilidad | No debe depender de |
| --- | --- | --- |
| Dominio | Secuencia de preparación, reglas de progreso y conceptos culturales. | Escenas, motores, dispositivos o recursos visuales. |
| Aplicación | Casos de uso: iniciar recorrido, avanzar paso, validar acción y cerrar sesión. | Detalles de entrada, persistencia o presentación. |
| Infraestructura | Carga de contenido, almacenamiento, telemetría autorizada y adaptadores externos. | Lógica de presentación concreta. |
| Presentación | Entorno inmersivo, interacción, guías, audio, texto y retroalimentación. | Reglas de negocio duplicadas. |

## Reglas de diseño

- Las escenas ensamblan dependencias; no contienen reglas educativas ni secuencias de negocio.
- Los controles y dispositivos se comunican con la aplicación mediante adaptadores e interfaces.
- El contenido cultural se modela como datos validados, no como condiciones dispersas en el
  código. Esto permite añadir experiencias sin duplicar la lógica del recorrido.
- Cada interacción debe tener retroalimentación y una alternativa comprensible para personas que
  no puedan usar una modalidad sensorial específica.
- Los cambios que afecten la experiencia se verifican con pruebas de reglas y con recorridos de
  integración en el entorno objetivo.

## Rendimiento y comodidad

La planificación definirá presupuestos de tasa de fotogramas, memoria y tiempos de respuesta
para el equipo de destino. La experiencia evitará desplazamientos de cámara impuestos y usará
alternativas de confort cuando la navegación sea necesaria. Estos criterios forman parte de la
aceptación de cada funcionalidad, no de una optimización posterior.

## Recursos 3D

Los modelos tridimensionales se crearán o incorporarán de forma independiente de la lógica del
recorrido. Cada recurso deberá conservar su fuente, licencia, versión y validación cultural.
Las convenciones de exportación, optimización y organización se decidirán durante la fase de
planificación, según la plataforma objetivo.
