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

        private List<Cooldown> _cooldowns = new List<Cooldown>();

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
            if (newValue == previousValue) return;
            if (_attributeChangeListeners.ContainsKey(template))
            {
                _attributeChangeListeners[template]?.Invoke(payload);
            }
        }

        public bool TryActivateAbility(Ability template)
        {
            if (GetActiveAbilityInstanceOfTemplate(template)) return false;
            if (template.CanBeActivated(this))
            {
                ActivateAbility(template);
                return true;
            }
            return false;
        }

        private Ability GetActiveAbilityInstanceOfTemplate(Ability template)
        {
            foreach(var a in _activeAbilities)
            {
                if (a.Template == template) return a;
            }

            return null;
        }

        public void AddCooldown(Ability template)
        {
            if (!template.IsTemplate)
            {
                template = template.Template;
            }

            foreach(var c in _cooldowns)
            {
                if (c.Template == template)
                {
                    c.RemainingCooldown += template.Cooldown;
                    return;
                }
            }

            _cooldowns.Add(new Cooldown(template));
        }

        public float GetCooldown(Ability template)
        {
            if (!template.IsTemplate)
            {
                template = template.Template;
            }

            foreach(var c in _cooldowns)
            {
                if (c.Template != template) continue;
                return c.RemainingCooldown;
            }
            return 0;
        }

        private void ActivateAbility(Ability ability)
        {
            var abilityInstance = ability.Instantiate(this);
            abilityInstance.Activate();
            _activeAbilities.Add(abilityInstance);
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
            foreach (var ability in _activeAbilities)
            {
                ability.Update();
            }
            UpdateCooldowns();
        }

        private void UpdateCooldowns()
        {
            for (int i = _cooldowns.Count - 1; i >= 0; i--)
            {
                Cooldown cooldown = _cooldowns[i];
                cooldown.RemainingCooldown -= Time.deltaTime;
                if (cooldown.RemainingCooldown <= 0)
                {
                    _cooldowns.RemoveAt(i);
                }
            }
        }

        public AttributeChangeListenerHandle RegisterAttributeChangeEvent(Attribute template, Action<AttributeChangePayload> callback)
        {
            Debug.Assert(template, "Template should not be null");
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

        private class Cooldown
        {
            public Ability Template;
            public float RemainingCooldown;

            public Cooldown(Ability template)
            {
                Template = template;
                RemainingCooldown = template.Cooldown;
            }
        }
    }
}
