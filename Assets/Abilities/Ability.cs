using System;
using UnityEngine;

namespace Abilities
{
    public abstract class Ability: ScriptableObject
    {
        public AbilityComponent Owner { get; private set; }

        public abstract float Cooldown { get; }

        
        public Ability Instantiate(AbilityComponent owner) {
            var inst = Instantiate(this);
            inst.Owner = owner;
            return inst;
        }

        public bool CanBeActivated(AbilityComponent target)
        {
            return true;
        }


        public void Activate()
        {
            OnActivated();
        }
        public void End()
        {
            Owner.RemoveAbility(this);
            OnEnded();
        }


        protected abstract void OnActivated();
        protected abstract void OnEnded();
    }
}