namespace Abilities.Attributes
{
	public class StackedFloatAttribute : StackedAttribute<float>
	{
		public override float Value
		{
			get
			{
				float val = 0;

				foreach (var stack in _stacks)
					if (stack.StackApplicationType == StackApplicationType.Additive)
						val += stack.Value;
					else
						val *= stack.Value;

				return val;
			}
		}
	}
}
