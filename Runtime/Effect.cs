using System;
using System.Collections.Generic;
using UnityEngine;

namespace Abilities
{
    /// <summary>
    /// Effects that applied to a AbilityComponent for a given duration. Can be used for durational status effects, or instant effects like damaging, healing.
    /// </summary>
    public abstract class Effect : ScriptableObject
    {
        /// <summary>
        /// Duration type of effect.
        /// Instant: Applied once and removed
        /// Durational: Applied every period seconds for given duration
        /// Infinite: Stays on unless explicitly removed via AbilityComponent.RemoveEffect
        /// </summary>
        public abstract DurationType DurationType { get; }

        /// <summary>
        /// Only one instance of this effect can be applied on a player. (Except for stacking)
        /// </summary>
        public abstract bool Unique { get; }

        /// <summary>
        /// How many times can this effect be stacked
        /// </summary>
        public abstract int MaxStack { get; }

        public int Stack { get; private set; }


        [SerializeField, Tooltip("Determines how adding new stack will behave.\nResetTime: Restarts the timer. Stack will need Duration seconds to pass to be removed\nResetPeriod: Period will be reset.\nIndividual: Makes the effect's lifetime stack number based. When effect duration is reached, it will remove a stack instead of removing whole effect")]
        private StackingFlags _stackingFlags;
        public StackingFlags StackingFlags => _stackingFlags;

        /// <summary>
        /// How long this effect will stay on if DurationType is Durational
        /// </summary>
        [Tooltip("How long this effect will stay on if DurationType is Durational")]
        public abstract float Duration { get; }

        /// <summary>
        /// Step that this effect will be executed
        /// </summary>
        public abstract float Period { get; }

        /// <summary>
        /// If true, this effect will be executed once.
        /// </summary>
        public abstract bool Once { get; }

        /// <summary>
        /// AbilityComponent that this effect is applied to
        /// </summary>
        [Tooltip("AbilityComponent that this effect is applied to")]
        public AbilityComponent Applied { get; private set; }

        [SerializeField, Tooltip("Tags to be applied once registered")]
        private TagContainer _tags;
        public TagContainer Tags => _tags;

        /// <summary>
        /// AbilityComponent that this effect is applied by
        /// </summary>
        [Tooltip("AbilityComponent that this effect is applied by")]
        public AbilityComponent Applier { get; private set; }

        /// <summary>
        /// Visual that is added to the target
        /// </summary>
        [SerializeField, Tooltip("Visual that is added to the target")]
        private GameObject _targetVisual;

        /// <summary>
        /// Offset the visual effect will be created with
        /// </summary>
        [SerializeField, Tooltip("Offset the visual effect will be created with")]
        private Vector3 _targetVisualOffset;

        [SerializeField, Tooltip("Handles how visual destruction will be handled.\nNone: No destruction will happen. You'll have to manually control its lifetime after it's added.\nDestroy: Visual will be destroyed after effect is removed.\nMessage: Message will be send when effect is removed.")]
        private VisualDestructionHandling _visualDestructionHandling = VisualDestructionHandling.Destroy;

        [SerializeField, Tooltip("Message to be sent to visual instance when effect is removed. Used with VisualDestructionHandling.Message")]
        private string _destructionMessage;

        [SerializeField, Tooltip("Tags that will prevent activation of the effect if applied has any")]
        private TagContainer _activationPreventationTags;
        public TagContainer ActivationPreventationTags => _activationPreventationTags;

        [SerializeField, Tooltip("Tags that will pause this effect while active. Effect will preserve its duration.")]
        private TagContainer _executionPausingTags;
        
        [SerializeField, Tooltip("Tags that will automatically remove this effect once added")]
        private TagContainer _removalTags;
        public TagContainer RemovalTags => _removalTags;



        private Effect _template;
        public Effect Template => _template;

        public Effect Instantiate(AbilityComponent target, AbilityComponent applier)
        {
            var inst = Instantiate(this);
            inst.Applied = target;
            inst.Applier = applier;
            inst._template = this;
            return inst;
        }

        internal void OnAdded_Internal()
        {
            Stack = 1;
            OnStackAdded(Stack);
            CreateVisualEffect();
            Applied.MergeTags(Tags);
            OnAdded();
        }

        private void CreateVisualEffect()
        {
            if (_targetVisual)
            {
                var inst = Instantiate(_targetVisual, Applied.transform);
                inst.transform.localPosition = _targetVisualOffset;
                _targetVisualInstance = inst;
            }
        }

        internal void OnRemoved_Internal()
        {
            OnStackRemoved(Stack);
            OnRemoved();
            if (_targetVisualInstance)
            {
                switch (_visualDestructionHandling)
                {
                    case VisualDestructionHandling.None:
                        break;
                    case VisualDestructionHandling.Destroy:
                        Destroy(_targetVisualInstance);
                        break;
                    case VisualDestructionHandling.Message:
                        _targetVisualInstance.SendMessage(_destructionMessage);
                        break;

                }
            }
        }

        protected virtual void OnRemoved(){}
        protected virtual void OnAdded(){}

        private float _time;
        public float Time => _time;

        private float _periodTime;
        private int _periodIndex;
        private GameObject _targetVisualInstance;

        private bool _executed;

        public void AddStack()
        {
            if (Stack < MaxStack)
            {
                Stack++;
                if (StackingFlags.HasFlag(StackingFlags.ResetTime))
                {
                    _time = 0;
                }
                if (StackingFlags.HasFlag(StackingFlags.ResetPeriod))
                {
                    _periodTime = 0;
                    _periodIndex = 0;
                }
                OnStackAdded(Stack);
            }
        }

        public void RemoveStack()
        {
            if (Stack > 0)
            {
                Stack--;
                OnStackRemoved(Stack);
            }
        }

        /// <summary>
        /// Called when new stack added
        /// </summary>
        protected virtual void OnStackAdded(int stack)
        {
        }

        /// <summary>
        /// Called when stack removed
        /// </summary>
        protected virtual void OnStackRemoved(int stack)
        {
        }

        // /// <summary>
        // /// Initial delay for the first period to start
        // /// </summary>
        // [Tooltip("Initial delay for the first period to start")]
        // public abstract float PeriodDelay { get; }

        /// <summary>
        /// Executes the executions
        /// </summary>
        [Tooltip("Executes the executions")]
        public void Execute()
        {
            foreach (var tag in _executionPausingTags)
            {
                if (Applied.HasTag(tag)) return;
            }
            
            if (Once && _executed) return;
            OnExecuted();
            _executed = true;
        }

        protected abstract void OnExecuted();


        public void UpdateTime(float delta)
        {
            _time += delta;
            if (Period > 0)
            {
                _periodTime += delta;
                if (_periodTime > Period)
                {
                    _periodIndex++;
                    _periodTime = _periodTime - Period;
                    // does it make sense to use this here?
                    Execute();
                }
            }
        }

        public void ResetTime()
        {
            _time = 0;
        }
    }

    public enum DurationType
    {
        Instant,
        Durational,
        Infinite
    }
    public enum VisualDestructionHandling
    {
        None,
        Destroy,
        Message
    }

    [System.Flags]
    public enum StackingFlags
    {
        None = 0,
        ResetTime = 1,
        ResetPeriod = 4,
        Individual = 8
    }
}
