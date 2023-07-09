using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Abilities
{
    public class StackTree : IEnumerable
    {
        private List<Effect> _effects = new List<Effect>();
        private List<Effect> _removalQueue = new List<Effect>();

        public int ActiveEffectCount => _effects.Count;

        public Effect Fetch(Effect effect)
        {
            return Fetch<Effect>(effect);
        }
        public T Fetch<T>() where T : Effect
        {
            foreach (var e in _effects)
            {
                if (e is T casted)
                {
                    return casted;
                }
            }
            return null;
        }
        public T Fetch<T>(Effect template) where T : Effect
        {
            foreach (var e in _effects)
            {
                if (e.Template == template && e is T casted)
                {
                    return casted;
                }
            }
            return null;
        }

        public void Add(Effect effect)
        {
            var existingEffect = Fetch(effect.Template);
            if (existingEffect)
            {
                var hasAvailableStack = existingEffect.Stack + 1 <= existingEffect.MaxStack;
                if (!hasAvailableStack) return;
                if (existingEffect.Unique && !hasAvailableStack) return;
                existingEffect.AddStack();
            }
            else
            {
                _effects.Add(effect);
                effect.OnAdded_Internal();
            }
        }

        public void Update(float delta)
        {
            foreach (var e in _effects)
            {
                e.UpdateTime(delta);
                switch (e.DurationType)
                {
                    case DurationType.Instant:
                        break;
                    case DurationType.Durational:
                    case DurationType.Infinite:
                        if (e.Period == 0)
                        {
                            e.Execute();
                        }
                        break;
                }

                if (e.DurationType == DurationType.Durational)
                {
                    if (e.Time >= e.Duration)
                    {
                        if (e.StackingFlags.HasFlag(StackingFlags.Individual))
                        {
                            if (e.Stack > 0)
                            {
                                e.RemoveStack();
                                e.ResetTime();
                            }
                            else
                            {
                                _removalQueue.Add(e);
                            }
                        }
                        else
                        {
                            _removalQueue.Add(e);
                        }
                    }
                }
            }

            foreach (var r in _removalQueue)
            {
                Remove(r);
                UnityEngine.Object.Destroy(r);
            }
            _removalQueue.Clear();
        }
        public void Remove(Effect effect)
        {
            if (_effects.Remove(effect))
            {
                effect.OnRemoved_Internal();
            }
        }

        public IEnumerator GetEnumerator()
        {
            return _effects.GetEnumerator();
        }
    }
}

