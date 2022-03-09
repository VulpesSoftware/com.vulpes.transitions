﻿using UnityEngine;

namespace Vulpes.Transitions
{
    /// <summary>
    /// Transitions nothing but the delay and duration values are still used in <see cref="TransitionGroup"/>s.
    /// </summary>
    [AddComponentMenu("Vulpes/Transitions/Transition Null")]
    public sealed class Transition_Null : Transition
    {
        public override void Initialize() { }

        protected override void OnTransitionStart() { }

        protected override void OnTransitionUpdate(in float time) { }

        protected override void OnTransitionEnd() { }
    }
}
