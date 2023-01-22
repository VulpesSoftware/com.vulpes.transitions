using System;
using UnityEngine;
using Vulpes.Promises;

namespace Vulpes.Transitions
{
    public enum TransitionMode : int
    {
        Reverse,
        Forward,
    }

    [Flags]
    public enum TransitionFlags : int
    {
        /// <summary>Resets the transition to the begining at startup.</summary>
        ResetOnInitialize = 1 << 0,
        /// <summary>Resets the transition before playing.</summary>
        ResetOnPlay = 1 << 1,
        /// <summary>Disables the GameObject when transitioned out (good for performance).</summary>
        DisableWhenDone = 1 << 2,
    }

    public interface ITransition 
    {
        void Initialize();

        IPromise Play(TransitionMode mode = TransitionMode.Forward, in bool instant = false, float? delayOverride = null);
        
        void Complete();
        
        void SetTime(in float time, TransitionMode mode = TransitionMode.Forward);

        float CurrentTime { get; }

        bool Instant { get; }

        TransitionMode Mode { get; }

        AnimationCurve Curve { get; }

        bool IsPlaying { get; }

        float Duration { get; set; }

        float Delay { get; set; }

        float TotalDuration { get; }

        TransitionFlags Flags { get; set; }
    }
}
