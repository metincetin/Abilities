using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UIElements;

namespace Abilities
{
	public abstract class Attribute : ScriptableObject
	{
		[SerializeField] private string _name;
		public string Name => _name;

		/// <summary>
		/// The AttributeSet that this Attribute is assigned to.
		/// </summary> 
		protected AttributeSet AttributeSet { get; private set; }

		protected Attribute _template;
		public Attribute Template => _template;

		public Attribute CreateInstance(AttributeSet set)
		{
			var inst = Instantiate(this);
			inst._template = this;
			inst.AttributeSet = set;
			OnCreated();
			return inst;
		}

		protected virtual void OnCreated()
		{
		}

		public event Action<object, object> GenericValueChanged;

		public abstract string ValueString { get; }

		protected void InvokeGenericValueChangedEvent(object newValue, object previousValue)
		{
			GenericValueChanged?.Invoke(newValue, previousValue);
		}

		/// <summary>
		/// Called when AttributeSet created all the attributes. Can be used to bind events between attributes
		/// </summary>
		public virtual void OnReady()
		{
		}
	}

	public abstract class Attribute<T> : Attribute
	{
		[SerializeField] protected T _value;

		public virtual T Value
		{
			get => _value;
			set
			{
				var oldValue = _value;

				_value = PostProcessValue(value);

				InvokeValueChangedEvent(_value, oldValue);
			}
		}

		public override string ValueString => Value.ToString();

		protected void InvokeValueChangedEvent(T n, T old)
		{
			OnValueUpdated();
			ValueChanged?.Invoke(n, old);
			InvokeGenericValueChangedEvent(n, old);
		}

		/// <summary>
		/// Process the value before it is applied to the attribute. Can be used for clamping
		/// </summary>
		/// <param name="value">Value attempted to be set</param>
		/// <returns>Final processed value</returns>
		protected virtual T PostProcessValue(T value) => value;

		protected virtual void OnValueUpdated()
		{
		}


		public event Action<T, T> ValueChanged;
	}
}
