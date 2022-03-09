using UnityEngine;

namespace Vulpes.Transitions
{
    /// <summary>
    /// Transitions the euler angles of a <see cref="Transform"/> from one orientation to another.
    /// </summary>
    [AddComponentMenu("Vulpes/Transitions/Transition Euler Angles")]
    public sealed class Transition_EulerAngles : Transition<Vector3>
    {
        [SerializeField] private Transform targetTransform = default;
        [SerializeField] private bool useLocalSpace = false;

        public override Vector3 Current
        {
            get
            {
                return useLocalSpace ? transform.localEulerAngles : targetTransform.eulerAngles;
            }
        }

        public override void Initialize()
        {
            if (targetTransform == null)
            {
                targetTransform = transform;
            }
        }

        protected override void OnTransitionStart() { }

        protected override void OnTransitionUpdate(in float time)
        {
            if (useLocalSpace)
            {
                targetTransform.localEulerAngles = Vector3.LerpUnclamped(start, end, Curve.Evaluate(time));
            } else
            {
                targetTransform.eulerAngles = Vector3.LerpUnclamped(start, end, time);
            }
        }

        protected override void OnTransitionEnd() { }
    }
}
