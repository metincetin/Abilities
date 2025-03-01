using System;
using System.Collections.Generic;
using Abilities;
using UnityEngine;

namespace Abilities.Attributes
{
	public abstract class StackedAttribute<T> : Attribute<T>
	{
		public enum StackApplicationType
		{
			Additive,
			Multiplicative
		}

		[SerializeField] protected List<Stack> _stacks = new();

		public abstract override T Value { get; }

		public T GetStackValue(string stackName)
		{
			foreach (var stack in _stacks)
				if (stack.Name == stackName)
					return stack.Value;

			return default;
		}

		public void SetStackValue(string stackName, T value)
		{
			var changed = false;

			T old = default;

			foreach (var stack in _stacks)
				if (stack.Name == stackName)
				{
					old = Value;
					stack.Value = value;
					changed = true;
				}

			if (changed) InvokeValueChangedEvent(Value, old);
		}

		public void AddStackValue(string name, T value,
			StackApplicationType applicationType = StackApplicationType.Additive)
		{
			_stacks.Add(new Stack(name, value, applicationType));
		}

		public override void OnReady()
		{
		}

		[Serializable]
		public class Stack
		{
			public string Name;
			public T Value;
			public StackApplicationType StackApplicationType;

			public Stack(string name, T value, StackApplicationType applicationType)
			{
				Name = name;
				Value = value;
				StackApplicationType = applicationType;
			}
		}
	}
}
