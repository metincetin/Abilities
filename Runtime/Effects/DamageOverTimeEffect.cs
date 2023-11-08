using Abilities.Examples;
using Abilities.Examples.Scripts.Effects;
using UnityEngine;

namespace Abilities.Effects
{
    public class DamageOverTimeEffect : BaseEffect
    {
        public float Damage;
        
        protected override void OnExecuted()
        {
            Applied.AttributeSet.GetAttribute<HealthAttribute>().Value -= Damage;
        }
    }
}
