# Feature Specification: Enseñanza de chicha en realidad virtual

**Feature Branch**: `main`

**Created**: 2026-11-16

**Status**: Draft

**Input**: User description: "Crear una aplicación de realidad virtual que enseñe paso a paso a
preparar chicha, un producto ecuatoriano, mediante modelos tridimensionales y una experiencia
inmersiva."

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Completar la preparación guiada (Priority: P1)

Una persona visitante inicia el recorrido, conoce los elementos necesarios y realiza, en orden,
las acciones de una preparación tradicional de chicha hasta completar la experiencia.

**Why this priority**: Es el valor educativo central: transformar la observación pasiva en una
práctica guiada que ayude a recordar la secuencia y apreciar el proceso.

**Independent Test**: Una persona puede iniciar desde cero, seguir todas las indicaciones,
completar cada acción requerida y recibir una confirmación de finalización sin apoyo externo.

**Acceptance Scenarios**:

1. **Given** que la persona inicia un recorrido nuevo, **When** acepta la introducción,
   **Then** ve el primer paso, sus elementos requeridos y una instrucción comprensible.
2. **Given** que la persona realiza correctamente la acción del paso actual, **When** termina la
   acción, **Then** recibe retroalimentación y se habilita el siguiente paso.
3. **Given** que la persona intenta realizar una acción fuera de secuencia, **When** interactúa
   con el elemento, **Then** recibe una guía respetuosa que le indica qué paso debe completar.
4. **Given** que la persona completa todos los pasos, **When** finaliza el último, **Then** ve un
   resumen de la preparación y un mensaje sobre el valor de preservar la tradición.

---

### User Story 2 - Comprender el contexto cultural (Priority: P2)

Una persona visitante consulta explicaciones breves de los ingredientes, utensilios y prácticas
presentados para entender que la preparación se enmarca en una tradición cultural ecuatoriana.

**Why this priority**: El producto busca concientizar, no solo enseñar una secuencia de acciones.

**Independent Test**: Durante un recorrido, una persona abre las explicaciones de al menos tres
elementos y puede regresar al paso que estaba realizando sin perder el progreso.

**Acceptance Scenarios**:

1. **Given** que la persona observa un elemento explicable, **When** solicita información,
   **Then** recibe una explicación breve, clara y atribuible a una fuente registrada.
2. **Given** que la persona está leyendo o escuchando una explicación, **When** la cierra,
   **Then** vuelve al mismo paso con los elementos necesarios disponibles.

---

### User Story 3 - Repetir el aprendizaje con comodidad (Priority: P3)

Una persona visitante ajusta la presentación de las guías o reinicia el recorrido para repetir los
pasos a su propio ritmo.

**Why this priority**: Las personas aprenden con ritmos y necesidades de acceso distintos.

**Independent Test**: Una persona cambia una opción de guía, pausa o reinicia el recorrido y
puede continuar o volver al inicio sin estados confusos.

**Acceptance Scenarios**:

1. **Given** que el recorrido está en curso, **When** la persona lo pausa, **Then** las acciones
   no progresan hasta que decide reanudarlo.
2. **Given** que la persona necesita repetir el aprendizaje, **When** elige reiniciar,
   **Then** el recorrido vuelve a la introducción y no conserva progreso anterior.

---

### User Story 4 - Aprender sin depender del audio (Priority: P2)

Una persona con discapacidad auditiva completa el recorrido mediante instrucciones, demostraciones
y retroalimentación visual y textual, sin depender de una señal sonora para comprender o avanzar.

**Why this priority**: El prototipo prioriza a adolescentes y jóvenes con discapacidad auditiva
para reducir una barrera frecuente en materiales educativos basados en narración y sonido.

**Independent Test**: Una persona puede completar el recorrido con el audio desactivado y recibe
información suficiente para distinguir instrucciones, progreso y resultados de sus acciones.

**Acceptance Scenarios**:

1. **Given** que el audio está desactivado, **When** se presenta una instrucción esencial,
   **Then** la persona recibe una alternativa visual y textual que permite realizar el paso.
2. **Given** que una acción se completa o requiere corrección, **When** se muestra la
   retroalimentación, **Then** esta comunica el resultado mediante texto o símbolo además de
   cualquier sonido o color.

### Edge Cases

- La persona intenta usar un elemento que no corresponde al paso activo.
- La persona pausa, reinicia o sale del recorrido antes de completarlo.
- Una explicación cultural no está disponible o no posee una fuente validada.
- La persona no puede utilizar una modalidad de guía sensorial y necesita una alternativa.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: El sistema MUST presentar una introducción que explique el propósito educativo y
  el contexto cultural de la experiencia antes del primer paso.
- **FR-002**: El sistema MUST definir una secuencia ordenada de pasos de preparación, con una
  instrucción clara, elementos requeridos y resultado esperado para cada paso.
- **FR-003**: La persona MUST poder realizar las acciones del paso activo y recibir una
  confirmación comprensible al completarlo correctamente.
- **FR-004**: El sistema MUST impedir que una acción fuera de secuencia avance el recorrido y
  MUST indicar a la persona el paso que debe realizar primero.
- **FR-005**: El sistema MUST permitir pausar, reanudar y reiniciar un recorrido sin producir
  estados ambiguos ni pasos duplicados.
- **FR-006**: El sistema MUST ofrecer información contextual de los ingredientes, utensilios o
  prácticas relevantes y asociar cada explicación con una fuente registrada antes de publicarla.
- **FR-007**: El sistema MUST ofrecer alternativas de guía que permitan completar el recorrido
  cuando una persona no pueda depender de una modalidad visual, auditiva o háptica.
- **FR-008**: El sistema MUST mostrar un cierre al completar todos los pasos, con un resumen de
  la secuencia y un mensaje de preservación cultural respetuoso.
- **FR-009**: El contenido MUST identificar la variante de chicha y su procedencia antes de
  presentarse como una práctica tradicional específica.
- **FR-010**: El sistema MUST conservar el progreso solo durante la sesión activa; esta primera
  versión no requiere perfiles personales ni historial de visitantes.
- **FR-011**: El sistema MUST ofrecer todas las instrucciones esenciales y la retroalimentación
  de progreso en formatos visuales y textuales que permitan completar el recorrido sin audio.
- **FR-012**: El sistema MUST permitir consultar de nuevo la instrucción del paso activo sin
  penalizar el progreso ni obligar a reiniciar el recorrido.

### Key Entities *(include if feature involves data)*

- **Recorrido de preparación**: Experiencia educativa compuesta por introducción, pasos,
  explicaciones y cierre.
- **Paso de preparación**: Unidad ordenada con una acción esperada, elementos requeridos,
  instrucciones y condiciones de avance.
- **Elemento cultural**: Ingrediente, utensilio, espacio o práctica con nombre, explicación,
  procedencia y fuente de validación.
- **Estado del recorrido**: Progreso temporal que identifica el paso activo, el estado de pausa y
  la finalización de la sesión.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: Al menos el 90% de las personas de una prueba guiada de 10 participantes completa
  todos los pasos sin asistencia humana adicional después de la introducción.
- **SC-002**: Al menos el 80% de las personas que completan el recorrido ordena correctamente al
  menos el 80% de los pasos esenciales en una comprobación posterior.
- **SC-003**: Al menos el 80% de las personas de prueba indica que las instrucciones y la
  retroalimentación fueron claras en una encuesta de cierre.
- **SC-004**: El 100% de los elementos culturales publicados posee una fuente registrada y una
  revisión de representación cultural antes de la prueba de aceptación.
- **SC-005**: Una persona puede completar el recorrido inicial en 15 minutos o menos, excluyendo
  el tiempo que dedique voluntariamente a consultar explicaciones adicionales.
- **SC-006**: Al menos el 80% de las personas del grupo prioritario que participa en la prueba
  completa el recorrido con el audio desactivado y sin asistencia humana continua.

## Assumptions

- La primera versión se centra en una variante de chicha tradicional vinculada a una comunidad
  amazónica ecuatoriana; la procedencia concreta y la secuencia se validarán antes de publicar.
- El grupo prioritario del prototipo está formado por adolescentes y jóvenes con discapacidad
  auditiva en contextos educativos o de difusión cultural; sus necesidades se validarán en las
  pruebas de uso.
- La experiencia se utiliza de forma individual y con acompañamiento opcional de un docente o
  facilitador.
- Los modelos tridimensionales y recursos audiovisuales estarán disponibles con permisos de uso
  y registro de procedencia antes de incorporarse al contenido publicado.
- El progreso se mantiene únicamente dentro de la sesión para reducir la necesidad de datos
  personales en la primera versión.
