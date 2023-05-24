using UnityEngine;
using Vulpes.Promises;

namespace Vulpes.Transitions
{
    /// <summary>
    /// Triggers a series of <see cref="Transition"/>s all at once.
    /// </summary>
    [AddComponentMenu("Vulpes/Transitions/Transition Group"), DefaultExecutionOrder(100)]
    public sealed class TransitionGroup : Transition
    {
        [SerializeField] private Transition[] transitions = new Transition[0];

        public override float Duration
        {
            get
            {
                duration = 0.0f;
                if (transitions == null || transitions.Length == 0)
                {
                    return duration;
                }
                for (int i = 0; i < transitions.Length; i++)
                {
                    if (transitions[i] == null)
                    {
                        continue;
                    }
                    duration = Mathf.Max(duration, transitions[i].TotalDuration);
                }
                return duration;
            }
        }

        public override void Initialize() 
        { 
            if (flags.HasFlag(TransitionFlags.ResetOnInitialize))
            {
                for (int i = 0; i < transitions.Length; i++)
                {
                    transitions[i].Flags |= TransitionFlags.ResetOnInitialize;
                }
            }
        }

        private void StartTransition()
        {
            if (Mode == TransitionMode.Forward)
            {
                gameObject.SetActive(true);
            }

            OnTransitionStart();

            onTransitionStarted?.Invoke(Mode);

            int i;

            if (Instant)
            {
                for (i = 0; i < transitions.Length; i++)
                {
                    transitions[i].Play(Mode, Instant);
                }
                OnTransitionUpdate(Mode == TransitionMode.Forward ? 1.0f : 0.0f);
                EndTransition();
                return;
            }

            for (i = 0; i < transitions.Length; i++)
            {
                if (Mode == TransitionMode.Reverse)
                {
                    transitions[i].Play(Mode, Instant, Instant ? 0.0f : duration - transitions[i].TotalDuration);
                } else
                {
                    transitions[i].Play(Mode, Instant);
                }
            }

            transitionRoutine = GetTransitionAnchor().StartCoroutine(
                TransitionRoutine(
                    (t) => OnTransitionUpdate(t),
                    duration,
                    Curve,
                    Mode == TransitionMode.Reverse,
                    () =>
                    {
                        EndTransition();
                    }
                ));
        }

        private void EndTransition()
        {
            IsPlaying = false;
            OnTransitionEnd();
            onTransitionEnded?.Invoke(Mode);
            transitionPromise.Resolve();
            if (Mode == TransitionMode.Reverse)
            {
                gameObject.SetActive(false);
            }
        }

        public override IPromise Play(TransitionMode mode = TransitionMode.Forward, in bool instant = false, float? delayOverride = null)
        {
            if (IsPlaying)
            {
                Complete();
            }

            duration = 0.0f;
            for (int i = 0; i < transitions.Length; i++)
            {
                duration = Mathf.Max(duration, transitions[i].TotalDuration);
            }

            transitionPromise = Promise.Create();
            Instant = instant;
            Mode = mode;

            IsPlaying = true;

            if (delayOverride.GetValueOrDefault() > 0.0f || delay > 0.0f)
            {
                GetTransitionAnchor().StartCoroutine(DelayedTransitionRoutine(delayOverride ?? delay, StartTransition));
            } else
            {
                StartTransition();
            }

            return transitionPromise;
        }

        public override void Complete()
        {
            if (IsPlaying)
            {
                if (transitionRoutine != null)
                {
                    GetTransitionAnchor().StopCoroutine(transitionRoutine);
                }
                OnTransitionUpdate(Mode == TransitionMode.Forward ? 1.0f : 0.0f);
                for (int i = 0; i < transitions.Length; i++)
                {
                    transitions[i].Complete();
                }
                EndTransition();
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
            => duration = TotalDuration;
#endif
    }
}