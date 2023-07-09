using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Abilities.Examples.FX
{
    public static class DamageNumberChannel
    {

        public static event Action<int, Transform, Vector3> DamageNumberRequested;

        public static void Request(int value, Transform target, Vector3 offset)
        {
            DamageNumberRequested?.Invoke(value, target, offset);
        }

    }
}
