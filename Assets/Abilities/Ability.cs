using System;
using UnityEngine;

namespace Abilities
{
    /// <summary>
    /// Abstract ability class. Abilities should extend from this. Ability behaviour should be done in OnActivated, and reverted on OnEnded (if necessary)
    /// </summary>
    public abstract class Ability: ScriptableObject
    {
        public AbilityComponent Owner { get; private set; }

        /// <summary>
        /// Cooldown duration for this ability to be casted again
        /// </summary>
        public abstract float Cooldown { get; }


        public bool IsTemplate => _template == null;

		private Ability _template;

        /// <summary>
        /// Represents the scriptable object that this ability instantiated from.
        /// </summary>
        public Ability Template => _template;

        
        public Ability Instantiate(AbilityComponent owner) {
            var inst = Instantiate(this);
            inst.Owner = owner;
            inst._template = this;
            return inst;
        }

        public bool CanBeActivated(AbilityComponent target)
        {
            return true;
        }


        /// <summary>
        /// Activate this ability.
        /// </summary>
        public void Activate()
        {
            OnActivated();
        }

        /// <summary>
        /// Deactivate this ability and remove from its owner.
        /// </summary>
        public void End()
        {
            Owner.RemoveAbility(this);
            OnEnded();
        }


        /// <summary>
        /// Called when this ability is activated. Implement your ability behaviour here
        /// </summary>
        protected abstract void OnActivated();

        /// <summary>
        /// Called when this ability ends. Revert your ability behaviour here
        /// </summary>
        protected abstract void OnEnded();
    }
}