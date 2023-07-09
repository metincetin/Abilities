using UnityEngine;

namespace Abilities.Examples.Scripts
{
    public class PlayerTester : MonoBehaviour
    {
        public Effect DamageEffect;
        public Effect HealEffect;

        public Ability TestAbility;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GetComponent<AbilityComponent>().AddEffect(DamageEffect);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                GetComponent<AbilityComponent>().AddEffect(HealEffect);
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
