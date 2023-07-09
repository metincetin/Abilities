using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Abilities.Examples.FX
{
    public class DamageNumberChannelListener : MonoBehaviour
    {
        [SerializeField]
        private DamageNumber _damageNumberPrefab;

        private void OnEnable()
        {
            DamageNumberChannel.DamageNumberRequested += OnDamageNumberRequested;
        }

        private void OnDisable()
        {
            DamageNumberChannel.DamageNumberRequested -= OnDamageNumberRequested;
        }

        private void OnDamageNumberRequested(int value, Transform target, Vector3 offset)
        {
            var inst = Instantiate(_damageNumberPrefab, transform);
            inst.TransformReference = target;
            inst.Offset = offset;
            inst.Value = value;
        }
    }
}
