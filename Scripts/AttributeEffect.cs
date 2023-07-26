using System;
using UnityEngine;

namespace Abilities
{
	/// <summary>
	/// Base effect that alters target's attributes once added.
	/// </summary>
	[CreateAssetMenu(menuName = "Abilities/Effects/Attribute Effect")]
	public class AttributeEffect : Effect
	{
		public override DurationType DurationType { get; }
		public override bool Unique { get; }
		public override int MaxStack { get; }
		public override float Duration { get; }
		public override float Period { get; }
		public override bool Once { get; }

		[SerializeField, Tooltip("Should revert the effect when removed?")]
		private bool _revert;

		protected override void OnAdded()
		{
		}

		protected override void OnRemoved()
		{
			if (_revert)
			{
				Revert();
			}
		}

		private void Revert()
		{
		}


		protected override void OnExecuted()
		{
		}
	}

	[Serializable]
	public class AttributeChangerPair
	{
		public Attribute Attribute;
		[SerializeReference]
		public AttributeChangerValue Value;
	}

	[System.Serializable]
	public abstract class AttributeChangerValue
	{
		public abstract float Value { get; }
	}

	public enum AttributeChangeType
	{
		Offset,
		Multiplier,
	}
}