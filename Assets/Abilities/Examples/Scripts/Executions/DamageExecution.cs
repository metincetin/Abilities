using UnityEngine;

namespace Abilities.Examples.Scripts.Executions
{
    [CreateAssetMenu(menuName = "Examples/Execution/DamageExecution")]
    public class DamageExecution : Execution
    {
        [SerializeField]
        private string _attributeName;

        [SerializeField]
        private float _value;

        public override void Execute(Effect effect)
        {
            var attribute = effect.Applied.AttributeSet.GetAttributeByName<HealthAttribute>(_attributeName);
            attribute.Value -= _value;
        }
    }
}
