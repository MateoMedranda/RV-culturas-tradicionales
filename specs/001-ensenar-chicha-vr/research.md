# Investigación técnica - Iteración 001

## Alcance investigado

La Iteración 001 entrega un corte vertical de la experiencia: una estación de preparación de
chicha, un paso interactivo completo y guías que se puedan seguir con el audio desactivado. Esta
investigación convierte las decisiones técnicas iniciales en criterios verificables para el plan.

## Decisiones

### Plataforma y pila XR

**Decisión**: Usar Unity 6.3 LTS con Universal Render Pipeline, XR Plugin Management, Unity
OpenXR Plugin, Input System y una versión estable de XR Interaction Toolkit 3.x.

**Rationale**: OpenXR mantiene desacoplada la interacción del visor específico; XR Interaction
Toolkit ofrece los componentes de interacción, UI y locomoción necesarios para un prototipo
pequeño. URP permite mantener el escenario dentro de un presupuesto apto para visor autónomo.

**Alternatives considered**:

- Oculus XR Plugin: descartado porque su ruta de soporte está deprecada frente a OpenXR.
- Paquetes XR en versión preview: descartados por riesgo de cambios incompatibles durante la
  iteración.
- SDKs específicos de fabricante: pospuestos hasta que un requisito del producto los justifique.

**Fuentes**:

- [Meta: configuración manual de Unity](https://developers.meta.com/vr/documentation/unity/unity-tutorial-hello-world-manual-steps/)
- [Meta: ruta Unity OpenXR](https://developers.meta.com/vr/blog/build-faster-and-earn-more-accelerate-unity-vr-workflow/)
- [Unity: XR Interaction Toolkit](https://docs.unity3d.com/current/Manual/com.unity.xr.interaction.toolkit.html)

### Dispositivo y validación inicial

**Decisión**: Desarrollar en Windows con XR Device Simulator y preparar el despliegue para Meta
Quest 3 o 3S mediante OpenXR. La compatibilidad solo se declarará después de una prueba real en
el dispositivo disponible para el equipo.

**Rationale**: El simulador permite construir y automatizar el flujo antes de disponer del visor;
OpenXR evita atar el dominio de la aplicación a un fabricante. La prueba en visor sigue siendo
obligatoria para comprobar ergonomía, comodidad y rendimiento.

**Alternatives considered**:

- Desarrollo exclusivo para PC VR: se mantiene como alternativa si no hay Quest disponible.
- Implementar seguimiento de manos desde el inicio: descartado; los controladores estándar
  cubren la interacción de la iteración y reducen el riesgo técnico.

**Fuente**: [Unity: XR Device Simulator](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@2.0/api/UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation.XRDeviceSimulator.html)

### Accesibilidad para el grupo prioritario

**Decisión**: Todas las instrucciones, cambios de estado, aciertos y errores tendrán equivalentes
visuales y textuales. El audio puede complementar, pero nunca ser la única fuente de información.
El color se acompañará de texto o iconos; las guías permitirán pausa, repetición y tamaño legible.

**Rationale**: La Iteración 001 prioriza a adolescentes y jóvenes con discapacidad auditiva. Esta
decisión convierte la accesibilidad en el camino principal de uso y no en un añadido posterior.

**Alternatives considered**:

- Narración con subtítulos opcionales: descartada porque todavía deja el flujo principal
  dependiente del audio.
- Señales mediante color únicamente: descartadas por ambigüedad y falta de acceso para otras
  necesidades visuales.

**Fuentes**:

- [Meta: checklist de accesibilidad](https://developers.meta.com/vr/design/accessibility-checklist/)
- [Meta: accesibilidad y color](https://developers.meta.com/vr/design/accessibility/)
- [W3C: WCAG 3 para entornos inmersivos, borrador](https://www.w3.org/TR/2026/WD-wcag-3.0-20260910/)

### Comodidad y escenario

**Decisión**: El primer paso ocurrirá en una única estación de trabajo, usable sentado o de pie.
Los objetos quedarán dentro de alcance o admitirán interacción a distancia. No habrá locomoción
continua; si en el futuro se necesita movimiento, teletransporte y giro por saltos serán los
valores iniciales.

**Rationale**: Un escenario compacto reduce mareo, fatiga, dependencia del espacio físico y
complejidad de la primera implementación.

**Alternatives considered**:

- Recorrido amplio con desplazamiento continuo: pospuesto porque no aporta valor al paso inicial
  y aumenta el riesgo de incomodidad.
- Construir una cocina completa en la primera iteración: descartado; se usará un greybox
  culturalmente neutro hasta validar el flujo y los contenidos.

**Fuentes**:

- [Meta: preferencias de locomoción](https://developers.meta.com/vr/design/locomotion-user-preferences/)
- [Meta: comodidad y usabilidad](https://developers.meta.com/vr/design/locomotion-comfort-usability/)

### Arquitectura y contenido

**Decisión**: Mantener cuatro capas: Domain para pasos y reglas, Application para casos de uso,
Infrastructure para carga de contenido y adaptadores, y Presentation para Unity, XR e interfaz.
Los pasos, elementos culturales y explicaciones serán activos de datos reutilizables; el progreso
de sesión será independiente de esos datos.

**Rationale**: Esta estructura cumple la constitución y permite agregar pasos, recetas o visores
sin duplicar reglas en escenas o prefabs.

**Alternatives considered**:

- Reglas y secuencia dentro de MonoBehaviours de la escena: descartado por dificultar pruebas y
  reutilización.
- Datos duplicados por prefab: descartados porque elevan el riesgo de inconsistencias.

**Fuentes**:

- [Unity: ScriptableObject](https://docs.unity3d.com/6000.1/Documentation/Manual/class-ScriptableObject.html)
- [Unity: XR Interaction Toolkit](https://docs.unity3d.com/current/Manual/com.unity.xr.interaction.toolkit.html)

### Calidad, pruebas y rendimiento

**Decisión**: Probar reglas y transiciones en Edit Mode; probar el recorrido integrado en Play Mode;
validar en visor antes de aceptar la iteración. El presupuesto inicial para el destino autónomo
será 72 Hz, equivalente a un tiempo de cuadro máximo de 13.89 ms. El escenario usará iluminación
horneada, materiales simples y sin postprocesado no justificado.

**Rationale**: Las pruebas separan la lógica del entorno XR; la medición en dispositivo evita
concluir erróneamente a partir del rendimiento del Editor.

**Alternatives considered**:

- Solo pruebas manuales: descartadas porque no protegen la secuencia ni las transiciones.
- Optimizar al final: descartado; el rendimiento y la comodidad son requisitos del producto.

**Fuentes**:

- [Unity: Test Framework](https://docs.unity3d.com/Manual/com.unity.test-framework.html)
- [Unity: profiling en dispositivo](https://docs.unity3d.com/2022.2/Documentation/Manual/profiler-profiling-applications.html)
- [Unity: importación de modelos](https://docs.unity3d.com/es/2021.1/Manual/ImportingModelFiles.html)

## Riesgos y respuestas

| Riesgo | Respuesta planificada |
| --- | --- |
| No hay visor disponible durante el desarrollo | Construir con XR Device Simulator, pero bloquear la aceptación final hasta una prueba en visor. |
| El contenido cultural aún no está validado | Usar contenido provisional sin atribuciones específicas y no publicar hasta registrar la fuente y revisión. |
| Los modelos 3D son demasiado pesados | Usar greybox inicial, perfilar por recurso y aplicar presupuesto de polígonos, texturas y materiales durante la implementación. |
| Las guías no son claras para el grupo prioritario | Probar con audio desactivado y registrar observaciones de personas usuarias antes de cerrar la iteración. |
