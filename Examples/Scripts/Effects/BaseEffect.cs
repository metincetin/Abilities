using UnityEngine;

namespace Abilities.Examples.Scripts.Effects
{
    [CreateAssetMenu(menuName = "Examples/Effects/Base")]
    public class BaseEffect : Effect
    {
        [SerializeField]
        private DurationType _durationType;
        public override DurationType DurationType => _durationType;

        [SerializeField]
        private float _duration;
        public override float Duration => _duration;

        [SerializeField] private float _period;
        public override float Period => _period;

        [SerializeField]
        private int _maxStack;
        public override int MaxStack => _maxStack;


        [SerializeField]
        private bool _unique;
        public override bool Unique => _unique;

        [SerializeField]
        private bool _once;
        public override bool Once => _once;
        protected override void OnExecuted()
        {
            
        }
    }
}
