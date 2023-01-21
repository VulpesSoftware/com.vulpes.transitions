using UnityEngine;

namespace Vulpes.Transitions
{
    /// <summary>
    /// Transitions the alpha of a <see cref="CanvasGroup"/> from one value to another, 
    /// automatically handling the interactable and blocks raycasts values.
    /// </summary>
    [AddComponentMenu("Vulpes/Transitions/Transition Canvas Group Alpha")]
    public sealed class Transition_CanvasGroupAlpha : Transition<float>
    {
        [SerializeField] private CanvasGroup canvasGroup = default;
        [SerializeField] private bool interactable = true;
        [SerializeField] private bool blocksRaycasts = true;

        public override float Current
            => canvasGroup.alpha;

        public override void Initialize()
        {
            if (canvasGroup == null)
            {
                canvasGroup = GetComponent<CanvasGroup>();
            }
            if (flags.HasFlag(TransitionFlags.ResetOnInitialize))
            {
                canvasGroup.blocksRaycasts = false;
                canvasGroup.interactable = false;
            } else
            {
                canvasGroup.blocksRaycasts = blocksRaycasts;
                canvasGroup.interactable = interactable;
            }
        }

        protected override void OnTransitionStart()
        {
            canvasGroup.blocksRaycasts = blocksRaycasts;
            canvasGroup.interactable = false;
        }

        protected override void OnTransitionUpdate(in float time)
            => canvasGroup.alpha = Mathf.LerpUnclamped(start, end, time);

        protected override void OnTransitionEnd()
        {
            canvasGroup.blocksRaycasts = Mode == TransitionMode.Forward && blocksRaycasts;
            canvasGroup.interactable = Mode == TransitionMode.Forward && interactable;
        }
    }
}