using System;
using System.Collections.Generic;
using UnityEngine;

namespace Abilities
{
    public abstract class Effect: ScriptableObject
    {
        public abstract DurationType DurationType { get; }
        public abstract float Duration { get; }

        public AbilityComponent Applied { get; private set; }
        public AbilityComponent Applier { get; private set; }

        public Effect Instantiate(AbilityComponent target, AbilityComponent applier)
        {
            var inst = Instantiate(this);
            inst.Applied = target;
            inst.Applier = applier;
            return inst;
        }

        [SerializeField]
        private List<Execution> executions = new List<Execution>();

        private float _time;
        public float Time => _time;

        public abstract float Period { get; }
        public abstract float PeriodDelay { get; }

        public void Execute()
        {
            foreach(var execution in executions)
            {
                execution.Execute(this);
            }
        }


        public void UpdateTime(float delta)
        {
            _time += delta;
        }
    }

    public enum DurationType
    {
        Instant,
        Durational,
        Infinite
    }
}