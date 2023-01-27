# Transition Group

The **Transition Group** component can be used to chain together a series of **Transitions** and essentially treat them as though they are one single **Transition**.

|**Property:** |**Function:** |
|:---|:---|
|**Duration** |The calculated duration of the Transition Group. |
|**Delay** |Delay at the start of the Transition's playback. |
|**Forward Curve** |Unused. |
|**Reverse Curve** |Unused. |
|**Flags** |Optional Transition Flags. Options are _Reset On Initialize_, _Reset On Play_, _Disable When Done_. |


## Hints
* The duration of the Transition Group displayed in the inspector is equal to the sum of the duration and delay value of the longest Transition in the group.