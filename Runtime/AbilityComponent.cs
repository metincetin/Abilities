using System;
using System.Collections.Generic;
using UnityEngine;

namespace Abilities
{
    [DefaultExecutionOrder(-100)]
    public class AbilityComponent : MonoBehaviour
    {
        private List<Ability> _activeAbilities = new List<Ability>();
        private EffectStack _effectStack = new EffectStack();
        public EffectStack EffectStack => _effectStack;

        private List<Cooldown> _cooldowns = new List<Cooldown>();

        [SerializeField]
        private AttributeSet _attributeSet;
        public AttributeSet AttributeSet => _attributeSet;

        private TagContainer _tags = new TagContainer();


        private Dictionary<Attribute, Action<AttributeChangePayload>> _attributeChangeListeners = new Dictionary<Attribute, Action<AttributeChangePayload>>();

        public event Action<Effect> EffectAdded;
        public event Action<Effect> EffectRemoved;

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

        public bool TryActivateAbility(Ability template, object payload = null)
        {
            if (GetActiveAbilityInstanceOfTemplate(template)) return false;
            if (template.CanBeActivated(this))
            {
                ActivateAbility(template, payload);
                return true;
            }
            return false;
        }

        private void OnDestroy()
        {
            for (var i = _activeAbilities.Count - 1; i >= 0; i--)
            {
                var ability = _activeAbilities[i];
                RemoveAbility(ability);
            }
        }

        private Ability GetActiveAbilityInstanceOfTemplate(Ability template)
        {
            foreach(var a in _activeAbilities)
            {
                if (a.Template == template) return a;
            }

            return null;
        }

        public void AddCooldown(Ability template, float cooldown)
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

            _cooldowns.Add(new Cooldown(template, cooldown));
        }
        
        public void AddCooldown(Ability template)
        {
            AddCooldown(template, template.Cooldown);
        }

        public void AddTag(string tag)
        {
            _tags.Add(tag);

            foreach (Effect ef in EffectStack)
            {
                if (ef.RemovalTags.Contains(tag))
                {
                    RemoveEffect(ef);
                    break;
                }
            }
        }

        public void RemoveTag(string tag) => _tags.Remove(tag);
        public bool HasTag(string tag)
        {
            return _tags.Contains(tag);
        }
        
        public float GetCooldownPercentage(Ability template)
        {
            if (!template.IsTemplate)
            {
                template = template.Template;
            }

            foreach(var c in _cooldowns)
            {
                if (c.Template != template) continue;
                return c.RemainingCooldown / c.StartDuration;
            }
            return 0;
        }
        
        public float GetCooldownRemaining(Ability template)
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

        private void ActivateAbility(Ability ability, object payload = null)
        {
            var abilityInstance = ability.Instantiate(this);
            abilityInstance.Payload = payload;
            _activeAbilities.Add(abilityInstance);
            abilityInstance.Activate();
        }

        public void RemoveAbility(Ability ability)
        {
            if (ability.Owner != this) return;
            if (_activeAbilities.Remove(ability))
            {
                Destroy(ability);
            }
        }
        public void RemoveAbilityTemplate(Ability ability)
        {
            var ab = GetAbility(ability);
            if (ab) { RemoveAbility(ability); }
        }

        public T GetAbility<T>() where T: Ability
        {
            foreach(var a in _activeAbilities){
                if (a is T t)
                {
                    return t;
                }
            }
            return default(T);
        }

        public Ability GetAbility(Ability ability)
        {
            foreach (var a in _activeAbilities)
            {
                if (ability.IsTemplate && ability == a.Template)
                {
                    return a;
                }
                if (ability.Template == a.Template)
                {
                    return a;
                }
            }
            return null;
        }

        public bool HasActiveAbilityOfType<T>() where T: Ability
        {
            foreach (var a in _activeAbilities)
            {
                if (a is T) return true;
            }

            return false;
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

        public void AddEffect(Effect effect, AbilityComponent applier = null)
        {
            if (effect.ActivationPreventationTags.ContainsAny(_tags)) return;
            var inst = effect.Instantiate(this, applier);
            if (inst.DurationType == DurationType.Instant)
            {
                inst.Execute();
            }
            else
            {
                _effectStack.Add(inst);
            }
            EffectAdded?.Invoke(effect);
        }


        public void RemoveEffect(Effect effect) {
            _effectStack.Remove(effect);
            EffectRemoved?.Invoke(effect);
        }

        private void Update()
        {
            _effectStack.Update(Time.deltaTime);
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
            public float StartDuration;

            public Cooldown(Ability template, float cooldown)
            {
                Template = template;
                RemainingCooldown = cooldown;
                StartDuration = RemainingCooldown;
            }
        }

        public void MergeTags(TagContainer tags)
        {
            _tags.Merge(tags);
        }
    }
}
