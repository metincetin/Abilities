using System;
using System.Collections;
using System.Collections.Generic;
using Abilities.Attributes;
using UnityEngine;

namespace Abilities.Examples.Scripts.Executions
{
	public class HealthBar : MonoBehaviour
	{
		[SerializeField]
		private AbilityComponent _displayTarget;

		[SerializeField]
		private Transform _fill;

		[SerializeField]
		private Attribute _maxHealthAttributeTemplate;

		[SerializeField]
		private Attribute _healthAttributeTemplate;
		private FloatAttribute _maxHealthAttribute;
		private AbilityComponent.AttributeChangeListenerHandle _healthChangeHandle;

		private void Start()
		{
			_maxHealthAttribute = _displayTarget.AttributeSet.GetAttributeFromTemplate<FloatAttribute>(_maxHealthAttributeTemplate);
		}

		private void OnHealthChanged(AbilityComponent.AttributeChangePayload payload)
		{
			_fill.transform.localScale = new Vector3(payload.ReadNewValue<float>() / _maxHealthAttribute.Value, 1, 1);
		}

		private void OnEnable()
		{
			_healthChangeHandle = _displayTarget.RegisterAttributeChangeEvent(_healthAttributeTemplate, OnHealthChanged);
		}
		private void OnDisable()
		{
			_displayTarget.UnregisterAttributeChangeEvent(_healthChangeHandle);
		}

	}
}
