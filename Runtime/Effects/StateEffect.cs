using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Abilities.Effects
{
    /// <summary>
    /// Basic effect that does nothing. Can be used to assign state to the component
    /// </summary>
    [CreateAssetMenu(menuName = "Abilities/Effects/State Effect")]
    public class StateEffect : Effect
    {
        public override DurationType DurationType => DurationType.Infinite;

        public override bool Unique => true;

        public override int MaxStack => 1;

        public override float Duration => 0;

        public override float Period => 0;

        public override bool Once => true;

        protected override void OnExecuted()
        {
        }
    }
}
