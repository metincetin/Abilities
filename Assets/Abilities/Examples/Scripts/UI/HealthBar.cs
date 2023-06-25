using System;
using System.Collections;
using System.Collections.Generic;
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
		private FloatAttribute _maxHealthAttribute;


		private void Awake()
		{
			_maxHealthAttribute = _maxHealthAttribute = _maxHealthAttribute = _displayTarget.AttributeSet.GetAttributeFromTemplate<FloatAttribute>(_maxHealthAttribute);
		}
		private void OnEnable()
		{
			_displayTarget.AttributeSet.GetAttribute<HealthAttribute>().ValueChanged += OnHealthChanged;
		}
		private void OnDisable()
		{
			_displayTarget.AttributeSet.GetAttribute<HealthAttribute>().ValueChanged -= OnHealthChanged;
		}

		private void OnHealthChanged(float val, float prev)
		{
			_fill.transform.localScale = new Vector3(val / _maxHealthAttribute.Value, 1, 1);
		}
	}
}
