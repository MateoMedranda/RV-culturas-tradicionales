# Tareas: Iteración 001 - Enseñanza de chicha en realidad virtual

**Input**: Artefactos de diseño en specs/001-ensenar-chicha-vr/

**Prerequisites**: plan.md, spec.md, research.md, data-model.md, contratos y quickstart.md.

**Tests**: Se incluyen pruebas Edit Mode y Play Mode porque la constitución exige calidad
verificable desde el inicio.

**Organization**: Las tareas se agrupan por historia de usuario; los checkpoints de commit
preservan los seis hitos definidos en plan.md.

## Convención de rutas

El proyecto Unity se crea en la raíz del repositorio. Todo código nuevo se ubica en Assets/_Project,
las pruebas en Assets/Tests y los recursos XR compartidos en Assets/XR.

## Fase 1: Configuración compartida

**Purpose**: Crear un proyecto Unity reproducible y una base XR sin implementar lógica de producto.

- [ ] T001 Crear el proyecto Unity 6.3 LTS con URP en Packages/manifest.json y ProjectSettings/ProjectVersion.txt.
- [ ] T002 [P] Documentar instalación de Unity, módulos de destino y apertura del proyecto en docs/UNITY_SETUP.md.
- [ ] T003 Configurar paquetes estables de OpenXR, Input System y XR Interaction Toolkit en Packages/manifest.json.
- [ ] T004 Configurar proveedor OpenXR, características del destino y XR Plug-in Management en ProjectSettings/XRPackageSettings.asset.
- [ ] T005 Crear las acciones de entrada para controladores y simulador en Assets/XR/Input/Iteration001.inputactions.
- [ ] T006 Crear las carpetas y definiciones de ensamblado para Domain, Application, Infrastructure y Presentation en Assets/_Project/Scripts/.
- [ ] T007 Crear ensamblados de pruebas Edit Mode y Play Mode en Assets/Tests/EditMode/Iteration001.EditModeTests.asmdef y Assets/Tests/PlayMode/Iteration001.PlayModeTests.asmdef.
- [ ] T008 Crear la escena base y el XR Origin configurado para XR Device Simulator en Assets/_Project/Scenes/Iteration001.unity.
- [ ] T009 Confirmar que el proyecto compila, que la escena abre y registrar el commit chore: initialize Unity XR foundation con Packages/manifest.json, ProjectSettings/XRPackageSettings.asset y Assets/_Project/Scenes/Iteration001.unity.

**Checkpoint**: El Editor abre sin errores y permite simular visor y controladores.

---

## Fase 2: Fundamentos bloqueantes

**Purpose**: Implementar el modelo de datos, reglas y límites entre capas necesarios para todas las historias.

- [ ] T010 [P] Crear ExperienceDefinition, PreparationStep y CulturalElement con sus reglas de identificador, ordinal y fuente en Assets/_Project/Scripts/Domain/Content/ExperienceDefinition.cs, Assets/_Project/Scripts/Domain/Content/PreparationStep.cs y Assets/_Project/Scripts/Domain/Content/CulturalElement.cs.
- [ ] T011 [P] Crear GuidanceContent, AssetManifestEntry y sus validaciones de texto, señal visual, procedencia y optimización en Assets/_Project/Scripts/Domain/Content/GuidanceContent.cs y Assets/_Project/Scripts/Domain/Content/AssetManifestEntry.cs.
- [ ] T012 [P] Crear ExperienceSessionState y ExperienceLifecycle con los estados Intro, Activo, Pausado, Completado y Salido en Assets/_Project/Scripts/Domain/Session/ExperienceSessionState.cs y Assets/_Project/Scripts/Domain/Session/ExperienceLifecycle.cs.
- [ ] T013 Crear contratos de Application para cargar contenido y publicar resultados de interacción en Assets/_Project/Scripts/Application/Ports/IExperienceRepository.cs y Assets/_Project/Scripts/Application/Results/ExperienceResult.cs.
- [ ] T014 Crear adaptador de Infrastructure para cargar ScriptableObjects sin exponer Unity al dominio en Assets/_Project/Scripts/Infrastructure/Content/ScriptableObjectExperienceRepository.cs.
- [ ] T015 Crear ScriptableObjects de autoría que correspondan al modelo de datos en Assets/_Project/Scripts/Infrastructure/Content/ExperienceDefinitionAsset.cs, Assets/_Project/Scripts/Infrastructure/Content/PreparationStepAsset.cs y Assets/_Project/Scripts/Infrastructure/Content/CulturalElementAsset.cs.
- [ ] T016 Crear validadores de contenido que bloqueen publicación si falta fuente, revisión cultural o guía esencial visual y textual en Assets/_Project/Scripts/Application/Validation/ExperienceContentValidator.cs.
- [ ] T017 Crear pruebas Edit Mode para las reglas compartidas, incluidos “los ordinales son únicos y consecutivos”, “no presentar guía esencial sin texto y señal visual” y “no cargar contenido publicable sin fuente o revisión” en Assets/Tests/EditMode/ContentValidationTests.cs.

**Checkpoint**: El modelo compartido se valida con Edit Mode y ninguna capa de dominio referencia Unity.

---

## Fase 3: Historia de usuario 1 - Completar la preparación guiada (Priority: P1) MVP

**Goal**: Permitir iniciar el recorrido, realizar un paso correcto y recibir confirmación, sin que una acción incorrecta cambie el progreso.

**Independent Test**: Con un único paso de prueba, la persona inicia, usa el objetivo correcto, llega al cierre y recibe corrección al usar un objetivo distinto.

- [ ] T018 [P] [US1] Escribir pruebas Edit Mode para inicio, avance único, rechazo de objetivo incorrecto y cierre en Assets/Tests/EditMode/GuidedPreparationUseCaseTests.cs.
- [ ] T019 [P] [US1] Crear la ExperienceDefinition de borrador con “mínimo un paso; los ordinales son únicos y consecutivos” en Assets/_Project/Content/Experiences/Iteration001Experience.asset.
- [ ] T020 [P] [US1] Crear prefabs greybox de ingrediente, recipiente y utensilio con escala, pivote y colisionador revisables en Assets/_Project/Prefabs/Ingredients/Ingredient_Greybox.prefab, Assets/_Project/Prefabs/Utensils/Bowl_Greybox.prefab y Assets/_Project/Prefabs/Utensils/Utensil_Greybox.prefab.
- [ ] T021 [US1] Construir la estación de trabajo compacta, usable sentado o de pie, con los tres prefabs en Assets/_Project/Scenes/Iteration001.unity.
- [ ] T022 [US1] Registrar el manifiesto de los tres recursos como Borrador, con procedencia y optimización pendientes, en Assets/_Project/Content/Experiences/Iteration001AssetManifest.asset.
- [ ] T023 [US1] Registrar el commit feat: add iteration 001 greybox content con Assets/_Project/Content/Experiences/Iteration001Experience.asset, Assets/_Project/Content/Experiences/Iteration001AssetManifest.asset y Assets/_Project/Prefabs/.
- [ ] T024 [US1] Implementar StartExperienceUseCase para transicionar de Intro a Activo y publicar la guía del primer paso en Assets/_Project/Scripts/Application/UseCases/StartExperienceUseCase.cs.
- [ ] T025 [US1] Implementar AttemptInteractionUseCase para aceptar solo el objetivo, tipo de acción y condición del paso activo en Assets/_Project/Scripts/Application/UseCases/AttemptInteractionUseCase.cs.
- [ ] T026 [US1] Implementar los resultados InteractionAccepted e InteractionRejected sin referencias a GameObject o MonoBehaviour en Assets/_Project/Scripts/Application/Results/InteractionResults.cs.
- [ ] T027 [US1] Hacer pasar las pruebas de secuencia y rechazo creadas en Assets/Tests/EditMode/GuidedPreparationUseCaseTests.cs.
- [ ] T028 [US1] Registrar el commit feat: implement guided preparation domain con Assets/_Project/Scripts/Domain/ y Assets/_Project/Scripts/Application/.
- [ ] T029 [US1] Crear el adaptador XR que traduzca selección directa o a distancia en AttemptInteraction en Assets/_Project/Scripts/Presentation/Interaction/XrInteractionAdapter.cs.
- [ ] T030 [US1] Crear un componente de objetivo interactivo que contenga targetId y expectedAction para los prefabs en Assets/_Project/Scripts/Presentation/Interaction/InteractionTargetPresenter.cs.
- [ ] T031 [US1] Crear el bootstrapper que conecte escena, repositorio y casos de uso en Assets/_Project/Scripts/Presentation/Bootstrap/ExperienceBootstrapper.cs.
- [ ] T032 [US1] Conectar XrInteractionAdapter, InteractionTargetPresenter y ExperienceBootstrapper a Assets/_Project/Scenes/Iteration001.unity sin incluir reglas de negocio en la escena.
- [ ] T033 [US1] Escribir y hacer pasar la prueba Play Mode de inicio, acción correcta y acción incorrecta en Assets/Tests/PlayMode/GuidedPreparationFlowTests.cs.
- [ ] T034 [US1] Registrar el commit feat: add XR preparation interaction con Assets/_Project/Scripts/Presentation/Interaction/, Assets/_Project/Scripts/Presentation/Bootstrap/ y Assets/Tests/PlayMode/GuidedPreparationFlowTests.cs.

**Checkpoint**: La experiencia P1 funciona en XR Device Simulator y puede demostrarse con un solo paso.

---

## Fase 4: Historia de usuario 2 - Comprender el contexto cultural (Priority: P2)

**Goal**: Permitir consultar explicaciones de elementos sin perder el paso activo y sin mostrar contenido no validado como definitivo.

**Independent Test**: Durante el paso activo, la persona abre tres fichas de contexto y vuelve al mismo paso con el progreso intacto.

- [ ] T035 [P] [US2] Escribir pruebas Edit Mode para bloquear explicaciones sin sourceReference y conservar el estado de sesión al solicitar contexto en Assets/Tests/EditMode/CulturalContentTests.cs.
- [ ] T036 [US2] Añadir hasta tres CulturalElement con explicación accesible, fuente, estado Borrador y referencias de recurso en Assets/_Project/Content/Experiences/Iteration001Experience.asset.
- [ ] T037 [US2] Implementar RequestCulturalContextUseCase para devolver la explicación sin cambiar currentStepOrdinal en Assets/_Project/Scripts/Application/UseCases/RequestCulturalContextUseCase.cs.
- [ ] T038 [US2] Crear panel de contexto cultural con texto, fuente y cierre explícito en Assets/_Project/Scripts/Presentation/UI/CulturalContextPanelPresenter.cs y Assets/_Project/UI/CulturalContextPanel.prefab.
- [ ] T039 [US2] Integrar el panel con los objetivos de la escena y hacer pasar las pruebas en Assets/Tests/EditMode/CulturalContentTests.cs y Assets/Tests/PlayMode/CulturalContextFlowTests.cs.

**Checkpoint**: La consulta de contexto no altera la secuencia ni presenta como validado contenido en Borrador.

---

## Fase 5: Historia de usuario 4 - Aprender sin depender del audio (Priority: P2)

**Goal**: Hacer que instrucciones, progreso, aciertos y correcciones sean comprensibles con el audio desactivado.

**Independent Test**: Con el audio silenciado, la persona identifica el objeto, realiza el paso, comprende éxito o corrección y llega al cierre.

- [ ] T040 [P] [US4] Escribir pruebas Play Mode que desactiven audio y verifiquen texto y señal visual en inicio, éxito y corrección en Assets/Tests/PlayMode/AudioIndependentGuidanceTests.cs.
- [ ] T041 [US4] Implementar AccessibleGuidancePresenter con texto obligatorio, icono opcional y repetición de guía en Assets/_Project/Scripts/Presentation/UI/AccessibleGuidancePresenter.cs.
- [ ] T042 [US4] Implementar AccessibleFeedbackPresenter para representar éxito y corrección con texto e icono además de color o sonido en Assets/_Project/Scripts/Presentation/UI/AccessibleFeedbackPresenter.cs.
- [ ] T043 [US4] Crear el panel de guía en campo de visión con fondo opaco, tamaño legible y control de repetición en Assets/_Project/UI/AccessibleGuidanceCanvas.prefab.
- [ ] T044 [US4] Integrar AccessibleGuidancePresenter y AccessibleFeedbackPresenter con los resultados del contrato runtime-interaction en Assets/_Project/Scenes/Iteration001.unity.
- [ ] T045 [US4] Hacer pasar Assets/Tests/PlayMode/AudioIndependentGuidanceTests.cs y registrar el commit feat: add accessible learning guidance con Assets/_Project/Scripts/Presentation/UI/ y Assets/_Project/UI/AccessibleGuidanceCanvas.prefab.

**Checkpoint**: P1, P2 y P4 se pueden demostrar sin audio y sin usar color como única señal.

---

## Fase 6: Historia de usuario 3 - Repetir el aprendizaje con comodidad (Priority: P3)

**Goal**: Permitir pausar, reanudar, repetir la guía y reiniciar sin producir estados ambiguos ni pasos duplicados.

**Independent Test**: La persona pausa el paso activo, no puede avanzar mientras está pausado, repite la guía, reanuda y reinicia al estado Intro.

- [ ] T046 [P] [US3] Escribir pruebas Edit Mode para transiciones Activo-Pausado-Activo, reinicio y salida en Assets/Tests/EditMode/SessionLifecycleTests.cs.
- [ ] T047 [US3] Implementar PauseExperienceUseCase, ResumeExperienceUseCase, RestartExperienceUseCase y RequestGuidanceUseCase en Assets/_Project/Scripts/Application/UseCases/SessionControlUseCases.cs.
- [ ] T048 [US3] Crear controles de pausa, reanudar, repetir y reiniciar con estado visual explícito en Assets/_Project/Scripts/Presentation/UI/SessionControlsPresenter.cs y Assets/_Project/UI/SessionControlsCanvas.prefab.
- [ ] T049 [US3] Conectar los controles a la escena sin locomoción continua ni movimiento de cámara impuesto en Assets/_Project/Scenes/Iteration001.unity.
- [ ] T050 [US3] Hacer pasar la prueba Play Mode de pausa, repetición y reinicio en Assets/Tests/PlayMode/SessionControlsFlowTests.cs.

**Checkpoint**: La persona controla el ritmo de aprendizaje sin perder claridad sobre el paso activo.

---

## Fase 7: Pulido y validación transversal

**Purpose**: Verificar el cumplimiento de rendimiento, comodidad, trazabilidad cultural y criterios de salida de la Iteración 001.

- [ ] T051 [P] Completar los campos de procedencia, permiso, revisión cultural y optimización de cada recurso en Assets/_Project/Content/Experiences/Iteration001AssetManifest.asset.
- [ ] T052 [P] Configurar iluminación horneada, materiales URP simples y presupuesto de recursos de la estación en Assets/_Project/Scenes/Iteration001.unity y docs/PERFORMANCE_BUDGET.md.
- [ ] T053 Ejecutar las pruebas Edit Mode y Play Mode, registrar resultados de V-001 a V-004 y evidencia de audio desactivado en docs/evidence/iteration001-editor-validation.md.
- [ ] T054 Ejecutar una Development Build en el visor OpenXR objetivo, medir 72 Hz, CPU, GPU, memoria, asignaciones y comodidad sentado/de pie, y registrar V-005 en docs/evidence/iteration001-headset-validation.md.
- [ ] T055 Realizar la prueba con el grupo prioritario y registrar observaciones, consentimiento apropiado y mejoras pendientes en docs/evidence/iteration001-accessibility-feedback.md.
- [ ] T056 Registrar el commit test: validate iteration 001 in headset con docs/evidence/, Assets/Tests/ y Assets/_Project/Content/Experiences/Iteration001AssetManifest.asset.

**Checkpoint**: La iteración solo se acepta cuando V-001 a V-005 pasan, la evidencia cultural está disponible y no existen errores críticos.

---

## Dependencias y orden de ejecución

- Fase 1 no tiene dependencias.
- Fase 2 depende de T001 a T008 y bloquea todas las historias.
- US1 depende de la Fase 2 y define el MVP.
- US2 y US4 dependen de la Fase 2; para integrar con el recorrido usan los resultados de US1.
- US3 depende de la Fase 2 y puede implementarse después de US1 para reutilizar el estado de sesión.
- Fase 7 depende de las historias que se deseen aceptar; T054 queda bloqueada hasta disponer de visor.

Orden recomendado: Configuración → Fundamentos → US1 → US2 y US4 → US3 → Validación transversal.

## Oportunidades de paralelismo

- Tras T001, T002 puede avanzar en paralelo con T003 a T008 si no modifica los mismos archivos.
- Tras la estructura de ensamblados, T010, T011 y T012 pueden desarrollarse en paralelo.
- En US1, T018, T019 y T020 son independientes tras completar la Fase 2.
- En US2, las pruebas T035 pueden prepararse mientras se redacta el contenido de T036.
- En US4, T040 puede prepararse mientras se crean los presenters T041 y T042 en archivos distintos.
- En la fase final, T051 y T052 pueden ejecutarse en paralelo; T053 a T055 requieren los flujos implementados.

## Estrategia de implementación

### MVP

El MVP es la Fase 3 completa: inicio, un paso validable, corrección ante error, cierre y prueba en
XR Device Simulator. Debe confirmarse con el checkpoint de US1 antes de añadir contexto o controles.

### Entrega incremental

1. Cerrar la Fundación Unity con T009.
2. Construir el MVP de US1 y cerrar los commits de greybox, dominio e interacción XR.
3. Añadir contexto cultural y accesibilidad sin audio; cerrar el commit de inclusión.
4. Añadir controles de ritmo y reinicio.
5. Aceptar la iteración solo después de validación en visor, revisión cultural y evidencia de uso.

## Notas

- Todas las tareas usan el formato obligatorio de checklist, ID, etiqueta de historia cuando aplica y rutas concretas.
- Las tareas de pruebas se incluyen por los requisitos de la constitución; su ejecución debe preceder a declarar cada checkpoint completo.
- Cada checkpoint conserva un commit descriptivo. Si falta visor, fuente cultural o validación con personas usuarias, registrar el bloqueo sin declarar aceptación.
