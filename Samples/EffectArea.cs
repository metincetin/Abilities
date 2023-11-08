using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Abilities.Examples
{
    public class EffectArea : MonoBehaviour
    {
        [SerializeField]
        private Effect _effect;

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.TryGetComponent<AbilityComponent>(out var ability))
            {
                ability.AddEffect(_effect);
            }
        }
    }
}
