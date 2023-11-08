using System.Collections;
using System.Collections.Generic;
using Abilities.Attributes;
using Abilities.Examples.Scripts.Effects;
using UnityEngine;

namespace Abilities.Examples.Effects
{
    public class SlowEffect : BaseEffect
    {
        [SerializeField]
        private float _slowMultiplierPerStack;

        [SerializeField]
        private FloatAttribute _targetAttribute;


        protected override void OnStackAdded(int stack)
        {
            var movementSpeedAttribute = Applied.AttributeSet.GetAttributeFromTemplate<FloatAttribute>(_targetAttribute);
            if (movementSpeedAttribute)
            {
                movementSpeedAttribute.Value -= _slowMultiplierPerStack;
            }
        }

        protected override void OnStackRemoved(int stack)
        {
            var movementSpeedAttribute = Applied.AttributeSet.GetAttributeFromTemplate<FloatAttribute>(_targetAttribute);
            if (movementSpeedAttribute)
            {
                movementSpeedAttribute.Value +=  _slowMultiplierPerStack;
            }
        }
    }
}
