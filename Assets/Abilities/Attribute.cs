using UnityEngine;
using System.Collections;
using System;

namespace Abilities
{
	public abstract class Attribute : ScriptableObject
	{

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

				_value = value;

				OnValueUpdated();
				ValueChanged?.Invoke(_value, oldValue);
                
            }
        }

		protected virtual void OnValueUpdated()
		{
		}


		public event Action<T, T> ValueChanged;
	}
}