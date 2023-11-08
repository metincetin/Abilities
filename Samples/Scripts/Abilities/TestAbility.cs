using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Abilities.Examples.Abilities
{
    [CreateAssetMenu(menuName="Examples/Abilities/TestAbility")]
    public class TestAbility : Ability
    {
        public override float Cooldown => 5;

        protected override void OnActivated()
        {
            Debug.Log("Ability Activated!");
            Owner.StartCoroutine(AbilityEndDelay());
        }

        private IEnumerator AbilityEndDelay()
        {
            yield return new WaitForSeconds(1);
            End();
        }

        protected override void OnEnded()
        {
            RegisterCooldown();
            Debug.Log("Ability Ended!");
        }

        protected override void OnUpdated()
        {
        }
    }

}
