using UnityEngine;

namespace Vulpes.Transitions
{
    /// <summary>
    /// Transitions the position of a <see cref="Transform"/> from one position to another.
    /// </summary>
    [AddComponentMenu("Vulpes/Transitions/Transition Position")]
    public sealed class Transition_Position : Transition<Vector3>
    {
        [SerializeField] private Transform targetTransform = default;
        [SerializeField] private bool useLocalSpace = false;

        public override Vector3 Current
        {
            get
            {
                return useLocalSpace ? transform.localPosition : targetTransform.position;
            }
        }

        public override void Initialize()
        {
            if (targetTransform == null)
            {
                targetTransform = transform;
            }
        }

        protected override void OnTransitionStart()
        {

        }

        protected override void OnTransitionUpdate(in float time)
        {
            if (useLocalSpace)
            {
                targetTransform.localPosition = Vector3.LerpUnclamped(start, end, Curve.Evaluate(time));
            } else
            {
                targetTransform.position = Vector3.LerpUnclamped(start, end, time);
            }
        }

        protected override void OnTransitionEnd()
        {

        }
    }
}
