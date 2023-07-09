using Abilities.Examples.FX;
using UnityEngine;

namespace Abilities.Examples.Scripts
{
    public class PlayerTester : MonoBehaviour
    {
        [SerializeField]
        private Attribute _healthAttribute;
        public Effect DamageEffect;
        public Effect HealEffect;
        public Effect PoisonEffect;

        public Ability TestAbility;

        private AbilityComponent _abilityComponent;
        private AbilityComponent.AttributeChangeListenerHandle _healthChangedEventHandle;

        private void Awake()
        {
            _abilityComponent = GetComponent<AbilityComponent>();
        }

        private void OnEnable()
        {
            _healthChangedEventHandle = _abilityComponent.RegisterAttributeChangeEvent(_healthAttribute, OnHealthValueChanged);
        }

        private void OnDisable()
        {
            _abilityComponent.UnregisterAttributeChangeEvent(_healthChangedEventHandle);
        }

        private void OnHealthValueChanged(AbilityComponent.AttributeChangePayload payload)
        {
            var value = payload.ReadNewValue<float>() - payload.ReadPreviousValue<float>();

            DamageNumberChannel.Request((int)value, transform, Vector3.up * 2);
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GetComponent<AbilityComponent>().AddEffect(DamageEffect);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                GetComponent<AbilityComponent>().AddEffect(HealEffect);
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                GetComponent<AbilityComponent>().AddEffect(PoisonEffect);
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (!GetComponent<AbilityComponent>().TryActivateAbility(TestAbility))
                {
                    Debug.Log("Could not activate ability");
                }
            }
        }
    }
}
