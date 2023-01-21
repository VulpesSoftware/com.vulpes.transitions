using UnityEngine;

namespace Vulpes.Transitions
{
    /// <summary>
    /// Transitions the size delta of a <see cref="RectTransform"/> from one value to another.
    /// </summary>
    [AddComponentMenu("Vulpes/Transitions/Transition Size Delta")]
    public sealed class Transition_SizeDelta : Transition<Vector2>
    {
        [SerializeField] private RectTransform rectTransform = default;

        public override Vector2 Current => rectTransform.sizeDelta;

        public override void Initialize()
        {
            if (rectTransform == null)
            {
                rectTransform = GetComponent<RectTransform>();
            }
        }

        protected override void OnTransitionUpdate(in float time) 
            => rectTransform.sizeDelta = Vector2.LerpUnclamped(start, end, time);
    }
}