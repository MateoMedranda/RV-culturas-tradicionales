# Cocina interactiva: ChichaKitchen

Escena nueva: `Assets/_Project/Scenes/ChichaKitchen.unity`. La escena Iteration001 y sus cambios manuales se conservan. No es necesario regenerar Iteration001 para usar la cocina.

## Ejecutar

1. Abre ChichaKitchen y pulsa Play.
2. Pulsa Comenzar o Espacio.
3. WASD o flechas para caminar, botón derecho mantenido y ratón para mirar desde la cabeza.
4. Apunta y pulsa clic izquierdo o E. Alternativa sin puntería: teclas 1–6.
5. Sigue maíz, agua, panela, especias, mezclar, servir. Una selección incorrecta conserva el progreso.
6. P pausa/reanuda, R reinicia, F1 abre accesibilidad. Se puede ampliar texto, cambiar altura sentado/de pie y silenciar confirmaciones.

No hay límite de tiempo ni penalización por errores. Las indicaciones esenciales son textuales y el amarillo de selección es complementario al nombre. Las colisiones están en suelo, paredes, mesa y objetos; no existe movimiento de cámara impuesto.

## Implementación

- Importador glTFast 6.21.0 fijado en manifest/lock para los recursos locales.
- `KitchenSceneBuilder`: composición reproducible, materiales URP persistentes y assets de seis pasos. El menú `RV Culturas > Build Chicha Kitchen` reconstruye **esta escena**; guarda antes cualquier personalización de ChichaKitchen porque se reemplaza su composición.
- `KitchenLessonPresenter`: adapta los casos de uso y el estado de dominio existentes. No cambia las reglas de Iteration001.
- `KitchenPlayer`: primera persona con CharacterController y representación de manos; usa poses XR cuando hay dispositivo activo.
- `KitchenTarget`: identificación, límites visuales y destinos de animación.
- Las acciones son selecciones asistidas con desplazamiento animado al destino, no una simulación de agarre físico libre ni de líquidos.

## Contenido y recursos

El recorrido de maíz, agua, panela y especias es ilustrativo. No establece una receta ecuatoriana universal ni instrucciones reales de fermentación, cocción o consumo. Estado de contenido: Draft, pendiente de validación cultural. Se debe acordar comunidad, variante e ingredientes antes de presentar el contenido como enseñanza validada.

- Maíz: Decorative Corn 3D Scan — Arkify 3D. CC BY 4.0. Fuente: https://sketchfab.com/3d-models/decorative-corn-3d-scan-3d6b09c1a2e243dd9fd27240d252eb31. Se seleccionó una parte y se simplificó su material.
- Manos: Free Pack - VR Hands (Rigged) — PolyOne Studio. CC BY 4.0. Fuente: https://sketchfab.com/3d-models/free-pack-vr-hands-rigged-b3b8309e59c748e3956531c8f57dd233. Malla horneada y separada en dos manos para representación; sin animación individual de dedos.
- Estante: Wooden Shelf With Spices — volvor. CC BY 4.0. Fuente: https://sketchfab.com/3d-models/wooden-shelf-with-spices-f922f299aacf4c1791fb3eb983c6de9c.
- Fruta decorativa: Lowpoly Fruits & Vegetables — Loïc Norgeot. CC BY 4.0. Fuente: https://sketchfab.com/3d-models/lowpoly-fruits-vegetables-d3be8fed96eb48be88b47bbe8d2951e1.
- Textura local de panela: procedencia/licencia no suministrada; pendiente de registrar antes de distribución.
- El vaso peruano de Assets/Models/vaso no se incorpora: es CC BY-NC y no se presenta como utensilio ecuatoriano.

Las licencias originales se conservan junto a los modelos. Estos créditos deben acompañar cualquier distribución; no basta con mantenerlos únicamente en el repositorio.

## Aceptación y límites de validación

| Prueba | Resultado esperado |
| --- | --- |
| Abrir Scene y Game | Materiales con color, sin magenta; cocina y seis objetos identificables |
| Iniciar | Paso 1; texto completo y sin superposición |
| Seleccionar 6 antes de 1 | No avanza; corrección textual |
| Completar 1–6 | Animaciones, guía consecutiva y cierre |
| Pausar e intentar acción | No cambia el paso |
| Reiniciar | Introducción, objetos en su posición original, mezcla oculta |
| Caminar contra mesa/pared | No atraviesa superficies |
| Mirar con ratón | Giro desde la cámara, sin órbita externa |
| Apuntar a objeto cercano | Caja amarilla y nombre legible |
| Silenciar | Se puede completar con información visual |
| Texto extra grande y vista sentada | Instrucciones completas, objetos alcanzables por selección asistida |
| Visor y mandos | Poses de cabeza/manos y selección con gatillo; requiere validación física |

La accesibilidad no se certifica con una captura: falta prueba con personas usuarias, visor físico y perfiles de baja visión/movilidad. La interfaz XR fija y el seguimiento de mandos son una base de prueba. En mandos: gatillo selecciona/inicia, botón primario derecho pausa/reanuda, secundario derecho cambia tamaño y primario izquierdo cambia altura. Estas asignaciones requieren verificación con el dispositivo objetivo. No hay lector de pantalla, locución ni seguimiento óptico de dedos.

Presupuesto objetivo provisional: 72 Hz en visor, sin búsquedas globales por cuadro ni creación de geometría en Update. Medir CPU/GPU, memoria y estabilidad con el visor objetivo antes de aceptar rendimiento; aún no se ha medido en hardware XR.

## Verificación realizada (18 de noviembre de 2026)

- Unity MCP: inspección de recursos, instalación del importador, construcción de escena y ejecución en el editor 6000.6.4f1.
- EditMode: 12/12 pruebas correctas (incluidas KitchenLessonTests).
- PlayMode, KitchenPlayableFlowTests: 2/2 correctas. Incluyen las seis acciones con animaciones, rechazo de selección incorrecta, pausa, cierre, reinicio, presencia de ambas mallas de manos y bloqueo de movimiento al intentar atravesar la mesa.
- Capturas de Game revisadas para ajustar tipografía, encuadre e iluminación. No sustituyen una prueba de confort ni accesibilidad con personas usuarias.
- Visor físico, seguimiento de mandos, accesibilidad XR y rendimiento en hardware: pendientes.
