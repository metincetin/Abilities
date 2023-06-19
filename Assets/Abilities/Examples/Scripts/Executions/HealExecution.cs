using UnityEngine;

namespace Abilities.Examples.Scripts.Executions
{
    [CreateAssetMenu(menuName = "Examples/Execution/Heal")]
    public class HealExecution: Execution
    {
        public float Value;
        public override void Execute(Effect effect)
        {
            var health = effect.Applied.AttributeSet.GetAttributeByName<HealthAttribute>("Health");
            health.Value += Value;
        }
    }
}