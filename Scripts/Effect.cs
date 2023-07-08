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
        /// How long this effect will stay on if DurationType is Durational
        /// </summary>
        [Tooltip("How long this effect will stay on if DurationType is Durational")]
        public abstract float Duration { get; }

        /// <summary>
        /// AbilityComponent that this effect is applied to
        /// </summary>
        [Tooltip("AbilityComponent that this effect is applied to")]
        public AbilityComponent Applied { get; private set; }

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

        [SerializeField, Tooltip("Handles how visual destruction will be handled.\nNone: No destruction will happen. You'll have to manually control its lifetime after it's added.\nDestroy: Visual will be destroyed after effect is removed.\nMessage: Message will be send when effect is removed.")]
        private VisualDestructionHandling _visualDestructionHandling = VisualDestructionHandling.Destroy;

        [SerializeField, Tooltip("Message to be sent to visual instance when effect is removed")]
        private string _destructionMessage;


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
            CreateVisualEffect();
        }

        private void CreateVisualEffect()
        {
            if (_targetVisual)
            {
                var inst = Instantiate(_targetVisual, Applied.transform);
                inst.transform.localPosition = Vector3.zero;
                _targetVisualInstance = inst;
            }
        }

        internal void OnRemoved_Internal()
        {
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

        /// <summary>
        /// Behaviour to be executed when this effect is applied
        /// </summary>
        [SerializeField, Tooltip("Behaviour to be executed when this effect is applied")]
        private List<Execution> executions = new List<Execution>();

        private float _time;
        public float Time => _time;

        private float _periodTime;
        private int _periodIndex;
        private GameObject _targetVisualInstance;

        /// <summary>
        /// Step that executions will be executed.
        /// </summary>
        [Tooltip("Step that executions will be executed.")]
        public abstract float Period { get; }

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
            foreach (var execution in executions)
            {
                execution.Execute(this);
            }
        }


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
}
