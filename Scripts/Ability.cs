﻿using System;
using UnityEngine;

namespace Abilities
{
    /// <summary>
    /// Abstract ability class. Abilities should extend from this. Ability behaviour should be done in OnActivated and OnUpdated, and reverted on OnEnded (if necessary)
    /// </summary>
    public abstract class Ability : ScriptableObject
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




        public Ability Instantiate(AbilityComponent owner)
        {
            var inst = Instantiate(this);
            inst.Owner = owner;
            inst._template = this;
            return inst;
        }

        /// <summary>
        /// Returns true if ability is valid to be activated.
        /// </summary>
        public virtual bool CanBeActivated(AbilityComponent owner)
        {
            return owner.GetCooldown(this) <= 0;
        }


        /// <summary>
        /// Activate this ability.
        /// </summary>
        public void Activate()
        {
            OnActivated();
        }

        /// <summary>
        /// Deactivate this ability and remove from its owner adding cooldown.
        /// </summary>
        public void End()
        {
            Owner.RemoveAbility(this);
            if (Cooldown > 0)
            {
                Owner.AddCooldown(Template);
            }
            OnEnded();
        }

        public void Update()
        {
            OnUpdated();
        }


        /// <summary>
        /// Called when this ability is activated. Implement your ability behaviour here
        /// </summary>
        protected abstract void OnActivated();

        /// <summary>
        /// Called every frame after this ability is activated. Implement your ability's update behaviour here
        /// </summary>
        protected abstract void OnUpdated();

        /// <summary>
        /// Called when this ability ends. Revert your ability behaviour here
        /// </summary>
        protected abstract void OnEnded();
    }
}
