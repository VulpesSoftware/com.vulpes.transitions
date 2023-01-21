using UnityEngine;

namespace Vulpes.Transitions
{
    /// <summary>
    /// Transitions the Pivot of a <see cref="RectTransform"/> from one position to another.
    /// </summary>
    [AddComponentMenu("Vulpes/Transitions/Transition Rect Transform Pivot")]
    public sealed class Transition_RectTransformPivot : Transition<Vector2>
    {
        [SerializeField] private RectTransform rectTransform = default;

        public override Vector2 Start
        {
            get => start;
            set => start = value;
        }

        public override Vector2 End
        {
            get => end;
            set => end = value;
        }

        public override Vector2 Current => rectTransform.pivot;

        public override void Initialize()
        {
            if (rectTransform == null)
            {
                rectTransform = GetComponent<RectTransform>();
            }
        }

        protected override void OnTransitionUpdate(in float time)
            => rectTransform.pivot = Vector2.LerpUnclamped(start, end, time);
    }
}