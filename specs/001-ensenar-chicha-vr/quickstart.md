# Guía de validación - Iteración 001

## Propósito

Esta guía valida el recorrido de extremo a extremo una vez implementados los artefactos del plan.
No sustituye las pruebas automatizadas descritas en el modelo de datos ni los contratos de
interacción.

## Prerrequisitos

- Repositorio en la rama de trabajo acordada y sin cambios no revisados.
- Unity Hub con Unity 6.3 LTS y módulos necesarios para el dispositivo objetivo.
- Proyecto Unity creado en la raíz del repositorio conforme al plan.
- Paquetes URP, OpenXR, Input System y XR Interaction Toolkit instalados con versiones estables.
- Una ExperienceDefinition de prueba con un paso, un ingrediente, un recipiente y un utensilio.
- XR Device Simulator habilitado en el Editor; visor OpenXR disponible para la validación final.
- Recurso cultural marcado como Borrador o Validado según el contrato de autoría.

## Preparación

1. Abrir el proyecto desde Unity Hub y confirmar que no existan errores de compilación.
2. Abrir la escena de la Iteración 001.
3. Confirmar que la estación sea alcanzable sentado y de pie, o que los objetivos permitan
   interacción a distancia.
4. Activar el XR Device Simulator para la prueba en Editor.
5. Si hay visor disponible, crear una Development Build e instalarla antes de ejecutar el bloque
   de validación en dispositivo.

### Controles del simulador de escritorio

Para revisar la escena en el Editor sin visor, primero ejecuta `RV Culturas > Generate Iteration
001` después de que Unity termine de compilar. En Game View usa:

- `W`, `A`, `S`, `D`: desplazarte por la estación.
- Mantener clic derecho y mover el ratón: mirar alrededor.
- Clic izquierdo sobre un elemento: seleccionarlo mediante el rayo XR.
- `Espacio`: iniciar; `P`: pausar/reanudar; `G`: repetir guía; `R`: reiniciar; `C`: cerrar ficha cultural.

Este movimiento solo se activa sin visor para probar en escritorio; no se aplica durante la
experiencia con dispositivo XR.

## Escenarios de validación

### V-001: Inicio e instrucción accesible

1. Iniciar un recorrido nuevo.
2. Verificar que aparece la introducción y el primer paso.
3. Desactivar o silenciar el audio.
4. Confirmar que la instrucción se entiende mediante texto, icono o demostración visual.

**Resultado esperado**: La persona puede identificar qué objeto usar y qué acción realizar sin
audio.

### V-002: Acción correcta y avance

1. Realizar la acción configurada para el único paso activo.
2. Verificar la retroalimentación de éxito.
3. Confirmar que se presenta el siguiente estado o el cierre del recorrido.

**Resultado esperado**: El estado avanza una sola vez y la confirmación combina texto con señal
visual.

### V-003: Acción fuera de secuencia

1. Intentar usar un objeto no permitido o realizar un tipo de acción incorrecto.
2. Observar la guía de corrección.

**Resultado esperado**: El progreso no cambia; la guía indica el paso activo sin depender de
color o sonido.

### V-004: Pausa, reanudación, repetición y reinicio

1. Pausar el recorrido durante el paso activo.
2. Intentar interactuar; reanudarlo y solicitar repetir la guía.
3. Reiniciar el recorrido.

**Resultado esperado**: La pausa bloquea el avance, repetir no modifica el estado y reiniciar
limpia el progreso temporal.

### V-005: Perfilado y comodidad

1. Ejecutar el recorrido en el visor objetivo mediante una Development Build.
2. Registrar tasa de refresco, tiempo de CPU/GPU, memoria, asignaciones y eventos de espera de GPU.
3. Repetir sentado y de pie; comprobar alcance, ausencia de movimiento de cámara impuesto y
   legibilidad de las guías.

**Resultado esperado**: La prueba mantiene 72 Hz en el dispositivo objetivo y no muestra
regresiones de comodidad o accesibilidad. Si no existe visor, registrar V-005 como bloqueada y
no cerrar la iteración como aceptada.

## Evidencia requerida

- Resultados de pruebas Edit Mode y Play Mode.
- Capturas o vídeo de V-001 a V-004.
- Registro de prueba con audio desactivado.
- Registro de perfilado en dispositivo para V-005.
- Lista de participantes, observaciones y consentimiento apropiado para cualquier prueba con
  personas usuarias.
- Registro de fuentes y revisión cultural de los elementos mostrados.

## Criterio de salida

La Iteración 001 queda lista para aceptación solo si V-001 a V-004 pasan, V-005 pasa en visor,
no hay errores críticos, las pruebas automatizadas aplicables pasan y la evidencia cultural
requerida está disponible.
