using System;
using System.Collections.Generic;

namespace Abilities
{
    public class StackTree
    {
        private List<Effect> _effects = new List<Effect>();
        private List<Effect> _removalQueue = new List<Effect>();

        public void Add(Effect effect)
        {
            _effects.Add(effect);
        }

        public void Update(float delta)
        {
            foreach(var e in _effects)
            {
                e.UpdateTime(delta);
                switch (e.DurationType)
                {
                    case DurationType.Instant:
                        break;
                    case DurationType.Durational:
                    case DurationType.Infinite:
                        if (e.Period > 0)
                        {
                            if (e.Time % e.Period < 0.001)
                            {
                                e.Execute();
                            }
                        }
                        else
                        {
                            e.Execute();
                        }
                        break;
                }

                if (e.DurationType == DurationType.Durational)
                {
                    if (e.Time >= e.Duration)
                    {
                        _removalQueue.Add(e);
                    }
                }
            }

            foreach (var r in _removalQueue)
            {
                _effects.Remove(r);
                UnityEngine.Object.Destroy(r);
            }
            _removalQueue.Clear();
        }
    }
}

