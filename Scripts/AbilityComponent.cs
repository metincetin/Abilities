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


        private Dictionary<Attribute, Action<AttributeChangePayload>> _attributeChangeListeners = new Dictionary<Attribute, Action<AttributeChangePayload>>();

        private void Awake()
        {
            _attributeSet = Instantiate(_attributeSet);
            _attributeSet.Initialize();


            ListenAttributeEvents();
        }

        private void ListenAttributeEvents()
        {
            foreach (Attribute attribute in _attributeSet)
            {
                attribute.GenericValueChanged += (object n, object p) => OnAttributeGenericValueChanged(attribute.Template, n, p);
            }
        }

        private void OnAttributeGenericValueChanged(Attribute template, object newValue, object previousValue)
        {
            var payload = new AttributeChangePayload { NewValue = newValue, PreviousValue = previousValue };
            if (_attributeChangeListeners.ContainsKey(template))
            {
                _attributeChangeListeners[template]?.Invoke(payload);
            }
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

        public void RemoveAbility(Ability ability)
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
            foreach (var a in _activeAbilities)
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

        public void RemoveEffect(Effect effect) { }

        private void Update()
        {
            _stackTree.Update(Time.deltaTime);
        }

        public AttributeChangeListenerHandle RegisterAttributeChangeEvent(Attribute template, Action<AttributeChangePayload> callback)
        {
            var handle = new AttributeChangeListenerHandle();
            handle.Callback = callback;
            handle.Template = template;
            if (_attributeChangeListeners.ContainsKey(template))
            {
                _attributeChangeListeners[template] += handle.Callback;
            }
            else
            {
                _attributeChangeListeners[template] = handle.Callback;
            }
            return handle;
        }

        public void UnregisterAttributeChangeEvent(AttributeChangeListenerHandle handle)
        {
            _attributeChangeListeners[handle.Template] -= handle.Callback;
        }

        public class AttributeChangeListenerHandle
        {
            public Attribute Template;
            public Action<AttributeChangePayload> Callback;
        }
        public struct AttributeChangePayload
        {
            public object PreviousValue;
            public object NewValue;

            public T ReadPreviousValue<T>()
            {
                return (T)PreviousValue;
            }
            public T ReadNewValue<T>()
            {
                return (T)NewValue;
            }
        }
    }
}
