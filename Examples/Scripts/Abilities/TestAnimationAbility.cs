using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Abilities.Examples.Abilities
{
    [CreateAssetMenu(menuName = "Examples/Abilities/TestAnimationAbility")]
    public class TestAnimationAbility : Ability
    {
        [SerializeField]
        private string _animationStateName;

        public override float Cooldown => 4;

        protected override void OnActivated()
        {
            StartCoroutine(PlayAnimation());
        }

        private IEnumerator PlayAnimation()
        {
            yield return new PlayAndWaitAnimation(Owner.GetComponentInChildren<Animator>(), 1f, new AnimationPlayOptions{ StateName = _animationStateName});
            Debug.Log("Attaaack!");

            RegisterCooldown();
            End();
        }

        protected override void OnEnded()
        {
        }

        protected override void OnUpdated()
        {
        }
    }
}
