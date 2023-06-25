using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UIElements;

namespace Abilities
{
	public abstract class Attribute : ScriptableObject
	{
		protected Attribute _template;
		public Attribute Template => _template;
		public Attribute CreateInstance()
		{
			var inst = Instantiate(this);
			inst._template = this;
			return inst;
		}
	}

	public abstract class Attribute<T> : Attribute
	{
		[SerializeField]
		protected T _value;

		public T Value
        {
			get => _value;
            set
            {
				var oldValue = _value;

				_value = PostProcessValue(value);

				OnValueUpdated();
				ValueChanged?.Invoke(_value, oldValue);
            }
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