using System.Collections;
using System.Collections.Generic;
using Abilities.Attributes;
using UnityEngine;


namespace Abilities.Examples
{
    public class CharacterMovement : MonoBehaviour
    {
        private AbilityComponent _abilityComponent;
        private CharacterController _controller;

        [SerializeField]
        private Attribute _movementSpeedAttribute;
        private AbilityComponent.AttributeChangeListenerHandle _movementSpeedChangeEventHandle;
        private float _movementSpeed;

        private void Awake()
        {
            _abilityComponent = GetComponent<AbilityComponent>();
            _controller = GetComponent<CharacterController>();
        }

        private void OnEnable()
        {
            _movementSpeedChangeEventHandle = _abilityComponent.RegisterAttributeChangeEvent(_movementSpeedAttribute, OnMovementSpeedChanged);
        }


        private void OnDisable()
        {
            _abilityComponent.UnregisterAttributeChangeEvent(_movementSpeedChangeEventHandle);
        }

        private void Start()
        {
            _movementSpeed = _abilityComponent.AttributeSet.GetAttributeFromTemplate<FloatAttribute>(_movementSpeedAttribute).Value;
        }



        private void Update()
        {
            var movementInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            var velocity = movementInput * _movementSpeed;

            _controller.SimpleMove(velocity);
        }

        private void OnMovementSpeedChanged(AbilityComponent.AttributeChangePayload payload)
        {
            _movementSpeed = payload.ReadNewValue<float>();
        }
    }
}
