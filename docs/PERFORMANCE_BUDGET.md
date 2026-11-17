# Presupuesto de rendimiento — Iteración 001

## Objetivo de la experiencia

La iteración está diseñada para una estación compacta de una sola mesa y tres objetos
interactivos. El objetivo de aceptación en el visor es **72 Hz** (13,89 ms por cuadro),
en uso sentado o de pie y sin locomoción continua ni movimiento de cámara impuesto.

## Límites iniciales

| Recurso | Límite de la iteración | Estado actual |
|---|---:|---|
| Objetos interactivos | 3 | Ingrediente, recipiente y utensilio greybox |
| Luces dinámicas | 1 direccional | Escena base |
| Texturas externas | 0 | Greyboxes con materiales simples |
| Audio obligatorio | 0 | Guía textual y señales visuales |
| Locomoción continua | 0 | No incluida |

## Criterios de medición pendientes

La validación V-005 debe ejecutarse en el visor OpenXR objetivo con una Development
Build. Se registrarán frecuencia, tiempos de CPU/GPU, memoria, asignaciones y
observaciones de comodidad. No se declara cumplimiento del presupuesto hasta que
esa evidencia exista.
