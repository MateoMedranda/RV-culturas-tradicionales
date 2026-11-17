# RV Culturas Tradicionales Constitution

## Core Principles

### I. Arquitectura limpia y dependencias dirigidas
El código MUST separar presentación/Unity, aplicación, dominio e infraestructura. El dominio
MUST permanecer independiente de `MonoBehaviour`, escenas, SDKs de hardware y APIs de Unity;
las dependencias MUST apuntar hacia el dominio mediante interfaces y adaptadores. Cada módulo
MUST tener una responsabilidad explícita y cohesionada. Esta separación permite sustituir
dispositivos, fuentes de datos y UI sin reescribir la lógica del producto.

### II. Experiencia VR segura, accesible y centrada en la persona
Toda interacción MUST ser usable con los controles VR objetivos y ofrecer retroalimentación
visual, sonora o háptica cuando corresponda. Las funcionalidades MUST evitar movimiento de
cámara impuesto, aceleración brusca y otras fuentes prevenibles de incomodidad; cualquier
excepción MUST documentar su justificación y alternativa de confort. Los textos, contraste,
tamaño de objetivos y modos alternativos MUST considerarse requisitos de experiencia, no
retoques finales.

### III. Calidad verificable desde el inicio
Las reglas de dominio, casos de uso y conversiones de datos MUST contar con pruebas
automatizadas antes de declararse terminados. Los flujos que conecten entrada VR, aplicación,
persistencia y presentación MUST disponer de pruebas de integración o de reproducción
documentada cuando una prueba automatizada no sea viable. Toda corrección de defecto MUST
incluir una prueba de regresión o una justificación registrada de por qué no aplica.

### IV. Rendimiento VR como requisito funcional
Cada funcionalidad MUST definir y respetar su presupuesto de rendimiento para la plataforma
objetivo: tasa de fotogramas, tiempo de CPU/GPU, memoria y asignaciones por cuadro. El código
de actualización por cuadro MUST evitar asignaciones evitables, búsquedas repetidas y trabajo
que pueda realizarse fuera del bucle de renderizado. Las regresiones observables MUST medirse
con el perfilador de Unity y corregirse antes de integrar cambios que afecten la experiencia.

### V. Simplicidad, extensibilidad y contenido responsable
La solución MUST preferir el diseño más pequeño que satisfaga la especificación; patrones,
abstracciones y paquetes nuevos requieren una necesidad concreta y documentada. Los sistemas
reutilizables MUST configurarse mediante datos o interfaces en lugar de bifurcar lógica por
escena. El contenido sobre culturas tradicionales MUST registrar sus fuentes, respetar el
contexto cultural y evitar representaciones estereotipadas; las afirmaciones sensibles MUST
ser revisables por sus fuentes o asesores pertinentes.

## Technical Constraints

El producto MUST desarrollarse en Unity con una versión LTS acordada y paquetes fijados en el
control de versiones. La interacción VR MUST basarse en una capa de abstracción compatible con
el conjunto de dispositivos objetivo, de modo que el código de dominio no dependa de un visor
concreto. Las escenas MUST funcionar como composición de dependencias, no como repositorios de
lógica de negocio. Los datos de contenido MUST validarse al cargarse y conservar una estructura
que permita añadir culturas, recorridos y recursos sin modificar los casos de uso existentes.

## Development Workflow

Todo trabajo MUST comenzar con una especificación de usuario, criterios de aceptación medibles
y una evaluación de impacto en arquitectura, rendimiento VR y contenido cultural. Antes de
fusionar un cambio, se MUST revisar la separación de capas, ejecutar las pruebas aplicables y
verificar el flujo VR relevante en el editor o dispositivo objetivo. Las decisiones que cambien
contratos, dependencias, presupuestos de rendimiento o representación cultural MUST quedar
documentadas en el plan de la funcionalidad. Los cambios pequeños e independientes son
preferibles; la deuda técnica identificada MUST registrarse como una tarea explícita.

## Governance

Esta constitución prevalece sobre prácticas, planes y tareas que la contradigan. Cualquier
enmienda MUST documentar el motivo, el impacto en código y artefactos existentes, y actualizar
su versión conforme a versionado semántico: MAJOR para redefiniciones incompatibles, MINOR para
nuevos principios o requisitos materiales y PATCH para aclaraciones no semánticas. Cada revisión
de especificación, plan, tarea y cambio de código MUST comprobar el cumplimiento de estos
principios; las excepciones requieren una justificación escrita, alcance limitado y fecha de
revisión.

**Version**: 1.0.0 | **Ratified**: 2026-11-16 | **Last Amended**: 2026-11-16
