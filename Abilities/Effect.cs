using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Abilities
{
	/// <summary>
	/// Effects that applied to a AbilityComponent for a given duration. Can be used for durational status effects, or instant effects like damaging, healing.
	/// </summary>
	public abstract class Effect : ScriptableObject
	{
		/// <summary>
		/// Duration type of effect. 
		/// Instant: Applied once and removed
		/// Durational: Applied every period seconds for given duration
		/// Infinite: Stays on unless explicitly removed via AbilityComponent.RemoveEffect
		/// </summary>
		public abstract DurationType DurationType { get; }

		/// <summary>
		/// How long this effect will stay on if DurationType is Durational
		/// </summary>
		[Tooltip("How long this effect will stay on if DurationType is Durational")]
		public abstract float Duration { get; }

		/// <summary>
		/// AbilityComponent that this effect is applied to
		/// </summary>
		[Tooltip("AbilityComponent that this effect is applied to")]
		public AbilityComponent Applied { get; private set; }

		/// <summary>
		/// AbilityComponent that this effect is applied by
		/// </summary>
		[Tooltip("AbilityComponent that this effect is applied by")]
		public AbilityComponent Applier { get; private set; }

		private Effect _template;
		public Effect Template => _template;

		public Effect Instantiate(AbilityComponent target, AbilityComponent applier)
		{
			var inst = Instantiate(this);
			inst.Applied = target;
			inst.Applier = applier;
			inst._template = this;
			return inst;
		}

		/// <summary>
		/// Behaviour to be executed when this effect is applied
		/// </summary>
		[SerializeField, Tooltip("Behaviour to be executed when this effect is applied")]
		private List<Execution> executions = new List<Execution>();

		private float _time;
		public float Time => _time;

		/// <summary>
		/// Step that executions will be executed.
		/// </summary>
		[Tooltip("Step that executions will be executed.")]
		public abstract float Period { get; }

		/// <summary>
		/// Initial delay for the first period to start
		/// </summary>
		[Tooltip("Initial delay for the first period to start")]
		public abstract float PeriodDelay { get; }

		/// <summary>
		/// Executes the executions
		/// </summary>
		[Tooltip("Executes the executions")]
		public void Execute()
		{
			foreach (var execution in executions)
			{
				execution.Execute(this);
			}
		}


		public void UpdateTime(float delta)
		{
			_time += delta;
		}
	}

	public enum DurationType
	{
		Instant,
		Durational,
		Infinite
	}
}