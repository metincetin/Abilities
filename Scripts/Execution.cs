using UnityEngine;

namespace Abilities
{
    public abstract class Execution: ScriptableObject
    {
        public abstract void Execute(Effect effect);
    }
}