using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Vulpes.Promises;

namespace Vulpes.Transitions
{
    /// <summary>
    /// Base class for all <see cref="Transition"/>s.
    /// </summary>
    public abstract class Transition : MonoBehaviour, ITransition
    {
        /// <summary>
        /// Dummy Component used for anchoring Coroutines to a static scene object.
        /// </summary>
        public sealed class TransitionAnchor : MonoBehaviour { }

        private static TransitionAnchor transitionAnchor;

        public static TransitionAnchor GetTransitionAnchor()
        {
            if (transitionAnchor != null)
            {
                return transitionAnchor;
            }

            transitionAnchor = FindObjectOfType<TransitionAnchor>();

            if (transitionAnchor != null)
            {
                return transitionAnchor;
            }

            GameObject transitionAnchorObject = new("TransitionAnchor", typeof(TransitionAnchor));
            transitionAnchorObject.hideFlags |= HideFlags.HideInHierarchy | HideFlags.HideInInspector;
            DontDestroyOnLoad(transitionAnchorObject);
            transitionAnchor = transitionAnchorObject.GetComponent<TransitionAnchor>();
            return transitionAnchor;
        }

        [SerializeField, Min(0.0f)] protected float duration = 1.0f;
        [SerializeField, Min(0.0f)] protected float delay = 0.0f;
        [SerializeField] protected AnimationCurve forwardCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
        [SerializeField] protected AnimationCurve reverseCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
        [SerializeField] protected TransitionFlags flags = TransitionFlags.ResetOnInitialize | TransitionFlags.ResetOnPlay | TransitionFlags.DisableWhenDone;

        public UnityEvent<TransitionMode> onTransitionStarted = new();
        public UnityEvent<TransitionMode> onTransitionEnded = new();

        protected Coroutine transitionRoutine;
        protected IPromise transitionPromise;

        public float CurrentTime { get; protected set; }

        public bool Instant { get; protected set; }

        public TransitionMode Mode { get; protected set; }

        public AnimationCurve Curve => Mode == TransitionMode.Forward ? forwardCurve : reverseCurve;

        public bool IsPlaying { get; protected set; }

        public virtual float Duration
        {
            get => duration;
            set => duration = value;
        }

        public float Delay
        {
            get => delay;
            set => delay = value;
        }

        public float TotalDuration => Delay + Duration;

        public TransitionFlags Flags
        {
            get => flags;
            set => flags = value;
        }

        protected IEnumerator TransitionRoutine(Action<float> action, float duration, AnimationCurve curve, bool reversed, Action onComplete = null)
        {
            float elapsed = 0.0f;

            while (elapsed < duration)
            {
                float t = elapsed / duration;
                action(curve.Evaluate(reversed ? 1.0f - t : t));
                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }

            action(curve.Evaluate(reversed ? 0.0f : 1.0f));

            onComplete?.Invoke();
        }

        protected IEnumerator TransitionRoutine(Action<float> action, float duration, float elapsed, AnimationCurve curve, bool reversed, Action onComplete = null)
        {
            if (reversed)
            {
                elapsed = 1.0f - elapsed;
            }
            
            while (elapsed < duration)
            {
                float t = elapsed / duration;
                action(curve.Evaluate(reversed ? 1.0f - t : t));
                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }
            
            action(curve.Evaluate(reversed ? 0.0f : 1.0f));

            onComplete?.Invoke();
        }

        protected void Awake()
        {
            Initialize();
            if (IsPlaying)
            {
                return;
            }
            if (flags.HasFlag(TransitionFlags.ResetOnInitialize))
            {
                OnTransitionUpdate(0.0f);
                if (flags.HasFlag(TransitionFlags.DisableWhenDone))
                {
                    gameObject.SetActive(false);
                }
            } else
            {
                if (flags.HasFlag(TransitionFlags.DisableWhenDone))
                {
                    gameObject.SetActive(true);
                }
                OnTransitionUpdate(1.0f);
            }
        }

        public virtual void Initialize() { }

        protected virtual void OnTransitionStart() { }

        protected virtual void OnTransitionUpdate(in float time) { }

        protected virtual void OnTransitionEnd() { }

        /// <summary>
        /// Plays the <see cref="Transition"/> in the specified direction and returns a <see cref="Promise"/> 
        /// that will resolve once the <see cref="Transition"/> is complete.
        /// </summary>
        public virtual IPromise Play(TransitionMode mode = TransitionMode.Forward, in bool instant = false, float? delayOverride = null)
        {
            if (IsPlaying)
            {
                if (Mode == mode)
                {
                    if (transitionPromise == null)
                    {
                        return Promise.Resolved();
                    } else
                    {
                        return transitionPromise;
                    }
                }
                float t = CurrentTime;
                Complete();
                CurrentTime = t;
            }

            IsPlaying = true;

            transitionPromise = Promise.Create();
            Instant = instant;
            Mode = mode;

            if ((delayOverride.GetValueOrDefault() > 0.0f || delay > 0.0f) && !instant)
            {
                if (flags.HasFlag(TransitionFlags.ResetOnPlay))
                {
                    OnTransitionUpdate(Mode == TransitionMode.Forward ? 0.0f : 1.0f);
                }
                transitionRoutine = GetTransitionAnchor().StartCoroutine(DelayedTransitionRoutine(delayOverride ?? delay, StartTransition));
            } else
            {
                StartTransition();
            }

            return transitionPromise;
        }

        protected IEnumerator DelayedTransitionRoutine(float delay, Action onComplete)
        {
            if (delay > 0.0f)
            {
                yield return new WaitForSecondsRealtime(delay);
            }
            onComplete?.Invoke();
        }

        private void StartTransition()
        {
            if (flags.HasFlag(TransitionFlags.DisableWhenDone) && Mode == TransitionMode.Forward)
            {
                gameObject.SetActive(true);
            }

            if (flags.HasFlag(TransitionFlags.ResetOnPlay))
            {
                CurrentTime = Mode == TransitionMode.Forward ? 0.0f : 1.0f;
            }

            OnTransitionStart();

            onTransitionStarted?.Invoke(Mode);

            if (Instant)
            {
                OnTransitionUpdate(Mode == TransitionMode.Forward ? 1.0f : 0.0f);
                EndTransition();
                return;
            }

            transitionRoutine = GetTransitionAnchor().StartCoroutine(
                TransitionRoutine(
                    (t) =>
                    {
                        CurrentTime = t;
                        OnTransitionUpdate(t);
                    },
                    duration,
                    CurrentTime, 
                    Curve,
                    Mode == TransitionMode.Reverse,
                    EndTransition
                ));
        }

        private void EndTransition()
        {
            IsPlaying = false;
            CurrentTime = Mode == TransitionMode.Forward ? 1.0f : 0.0f;
            OnTransitionEnd();
            onTransitionEnded?.Invoke(Mode);
            transitionPromise.Resolve();

            if (flags.HasFlag(TransitionFlags.DisableWhenDone) && Mode == TransitionMode.Reverse)
            {
                gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Completes the active <see cref="Transition"/> immediately and resolves the pending <see cref="Promise"/>.
        /// </summary>
        public virtual void Complete()
        {
            if (IsPlaying)
            {
                if (transitionRoutine != null)
                {
                    GetTransitionAnchor().StopCoroutine(transitionRoutine);
                }
                OnTransitionUpdate(Mode == TransitionMode.Forward ? 1.0f : 0.0f);
                EndTransition();
            }
        }

        /// <summary>
        /// Sets the current time of the <see cref="Transition"/>.
        /// </summary>
        public void SetTime(in float time, TransitionMode mode = TransitionMode.Forward)
        {
            CurrentTime = mode == TransitionMode.Reverse ? 1.0f - time : time;
            OnTransitionUpdate(time);
        }
    }

    /// <summary>
    /// Base class for all <see cref="Transition"/>s that require a value.
    /// </summary>
    public abstract class Transition<T> : Transition where T : IEquatable<T>
    {
        [SerializeField] protected T start = default;
        [SerializeField] protected T end = default;

        public virtual T Start
        {
            get => start;
            set => start = value;
        }

        public virtual T End
        {
            get => end;
            set => end = value;
        }

        public abstract T Current { get; }
    }
}