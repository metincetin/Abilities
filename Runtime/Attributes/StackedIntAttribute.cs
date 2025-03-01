namespace Abilities.Attributes
{
	public class StackedIntAttribute : StackedAttribute<int>
	{
		public override int Value
		{
			get
			{
				var val = 0;

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
