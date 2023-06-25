using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Abilities
{
    public class AbilityComponent : MonoBehaviour
    {
        private List<Ability> _activeAbilities = new List<Ability>();
        private StackTree _stackTree = new StackTree();
        public StackTree StackTree => _stackTree;

        [SerializeField]
        private AttributeSet _attributeSet;
        public AttributeSet AttributeSet => _attributeSet;

        private void Awake()
        {
            _attributeSet = Instantiate(_attributeSet);
            _attributeSet.Initialize();
        }

        public bool TryActivateAbility(Ability ability)
        {
            if (ability.CanBeActivated(this))
            {
                ActivateAbility(ability);
                return true;
            }
            return false;
        }

        private void ActivateAbility(Ability ability)
        {
            var abilityInstance = ability.Instantiate(this);
            abilityInstance.Activate();
        }

        public  void RemoveAbility(Ability ability)
        {
            if (ability.Owner != this) return;
            if (_activeAbilities.Remove(ability))
                Destroy(ability);
        }
        
        public Ability GetAbility(Ability ability)
        {
            foreach (var a in _activeAbilities)
            {
                if (ability.Template == a.Template)
                {
                    return a;
                }
            }
            return null;
        }

        public bool HasActiveAbility(Ability ability)
        {
            foreach(var a in _activeAbilities)
            {
				if (ability.Template == a.Template)
                {
                    return true;
                }
            }
            return false;
        }

        public void AddEffect(Effect effect)
        {
            var inst = effect.Instantiate(this, null);
            if (inst.DurationType == DurationType.Instant)
            {
                inst.Execute();
            }
            else
            {
                _stackTree.Add(inst);
            }
        }
        public void RemoveEffect(Effect effect){}

        private void Update()
        {
            _stackTree.Update(Time.deltaTime);
        }
    }
}