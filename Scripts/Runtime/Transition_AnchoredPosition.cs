using UnityEngine;

namespace Vulpes.Transitions
{
    /// <summary>
    /// Transitions the Anchored Position of a <see cref="RectTransform"/> from one position to another.
    /// </summary>
    [AddComponentMenu("Vulpes/Transitions/Transition Anchored Position")]
    public sealed class Transition_AnchoredPosition : Transition<Vector2>
    {
        [SerializeField] private RectTransform rectTransform = default;
        [SerializeField] private bool useViewportSpace = false;

        private Vector2 initialOffset;
        private Vector2 startPosition;
        private Vector2 endPosition;

        public override Vector2 Start
        {
            get => startPosition;
            set
            {
                start = value;
                startPosition = useViewportSpace ? Vector2.Scale(start, new Vector2(Screen.width, Screen.height)) + initialOffset : start;
            }
        }

        public override Vector2 End
        {
            get => endPosition;
            set
            {
                end = value;
                endPosition = useViewportSpace ? Vector2.Scale(end, new Vector2(Screen.width, Screen.height)) + initialOffset : end;
            }
        }

        public override Vector2 Current => rectTransform.anchoredPosition;

        public override void Initialize()
        {
            if (rectTransform == null)
            {
                rectTransform = GetComponent<RectTransform>();
            }
            initialOffset = rectTransform.anchoredPosition;
            startPosition = useViewportSpace ? Vector2.Scale(start, new Vector2(Screen.width, Screen.height)) + initialOffset : start;
            endPosition = useViewportSpace ? Vector2.Scale(end, new Vector2(Screen.width, Screen.height)) + initialOffset : end;
        }

        protected override void OnTransitionUpdate(in float time)
            => rectTransform.anchoredPosition = Vector2.LerpUnclamped(startPosition, endPosition, time);
    }
}