# Transition - Anchored Position

**Transitions** the alpha of a **CanvasGroup** from one value to another, automatically handling the interactable and blocks raycasts values.

|**Property:** |**Function:** |
|:---|:---|
|**Duration** |The duration of the Transition. |
|**Delay** |Delay at the start of the Transition's playback. |
|**Forward Curve** |Easing Curve used when Transitioning forwards. |
|**Reverse Curve** |Easing Curve used when Transitioning in reverse. |
|**Flags** |Optional Transition Flags. Options are _Reset On Initialize_, _Reset On Play_, _Disable When Done_. |
|**Start** |The start alpha (0.0 - 1.0) of the Transition. |
|**End** |The end alpha (0.0 - 1.0) of the Transition. |
|**Canvas Group** |The Canvas Group being targeted by this Transition. |
|**Interactable** |If true, when this Transition finishes playing Forward, the Canvas Group's Interactable value will be set to true. |
|**Blocks Raycasts** |If true, when this Transition finishes playing Forward, the Canvas Group's Blocks Raycasts value will be set to true. |

## Hints
* The Start and End values are both clamped between 0.0 and 1.0.