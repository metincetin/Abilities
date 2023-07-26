using Abilities.Examples.Scripts.Effects;
using UnityEngine;

namespace Abilities.Examples.Effects
{
    public class DamageEffect : BaseEffect
    {

        [SerializeField]
        private float _damage;

        [SerializeField]
        private FloatAttribute _targetAttribute;
        public override bool Once => true;
        
        protected override void OnExecuted()
        {
            var attr = Applied.AttributeSet.GetAttributeFromTemplate<FloatAttribute>(_targetAttribute);
            if (attr)
            {
                attr.Value -= _damage;
            }
        }
    }
}
