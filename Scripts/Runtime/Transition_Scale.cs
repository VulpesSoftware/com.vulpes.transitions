using UnityEngine;

namespace Vulpes.Transitions
{
    /// <summary>
    /// Transitions the scale of a <see cref="Transform"/> from one scale to another.
    /// </summary>
    [AddComponentMenu("Vulpes/Transitions/Transition Scale")]
    public sealed class Transition_Scale : Transition<Vector3>
    {
        [SerializeField] private Transform targetTransform = default;

        public override Vector3 Current => targetTransform.localScale;

        public override void Initialize()
        {
            if (targetTransform == null)
            {
                targetTransform = transform;
            }
        }

        protected override void OnTransitionUpdate(in float time)
            => targetTransform.localScale = Vector3.LerpUnclamped(start, end, time);
    }
}