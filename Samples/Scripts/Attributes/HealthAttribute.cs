using Abilities.Attributes;
using UnityEngine;
namespace Abilities.Examples
{
    [CreateAssetMenu(menuName = "Examples/Attributes/HealthAttribute")]
    public class HealthAttribute: FloatAttribute
    {
        [SerializeField]
        private FloatAttribute _clampTo;

		protected override float PostProcessValue(float value)
		{
			if (_clampTo)
			return Mathf.Clamp(value, 0, _clampTo.Value);
			return base.PostProcessValue(value);
		}
	}
}