using UnityEngine;

namespace Abilities.Examples.Scripts.Effects
{
    public class DamageEffect : Effect
    {
        public override DurationType DurationType { get; }
        public override float Duration { get; }
        public override float Period { get; }

        public override int MaxStack { get; }

        public override bool Unique { get; }
    }
}
