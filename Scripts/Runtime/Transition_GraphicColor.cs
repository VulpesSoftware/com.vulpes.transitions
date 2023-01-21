using UnityEngine;
using UnityEngine.UI;

namespace Vulpes.Transitions
{
    /// <summary>
    /// Transitions the <see cref="Color"/> of a <see cref="Graphic"/> from one <see cref="Color"/> to another using a <see cref="Gradient"/>.
    /// </summary>
    [AddComponentMenu("Vulpes/Transitions/Transition Graphic Color")]
    public sealed class Transition_GraphicColor : Transition
    {
        [SerializeField] private Graphic targetGraphic = default;
        [SerializeField] private Gradient color = new();

        public Color Current => targetGraphic.color;

        public override void Initialize()
        {
            if (targetGraphic == null)
            {
                targetGraphic = GetComponent<Graphic>();
            }
        }

        protected override void OnTransitionUpdate(in float time)
            => targetGraphic.color = color.Evaluate(time);
    }
}