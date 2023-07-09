using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Abilities.Examples.FX
{
    public class DamageNumber : MonoBehaviour
    {

        [SerializeField]
        private TMP_Text _text;

        private static Camera _cameraReference;
        public Transform TransformReference { get; set; }

        private float _verticalOffset;

        [SerializeField]
        private Color _positiveColor;

        [SerializeField]
        private Color _negativeColor;

        [SerializeField]
        private float _lifeTime = 2;

        [SerializeField]
        private float _movementSpeed = 25;

        private float _time;

        private const float FADE_TIME = 0.2f;

        private int _value;
        public int Value
        {
            get => _value;
            set
            {
                _value = value;
                _text.text = value.ToString();
                _text.color = value >= 0 ? _positiveColor : _negativeColor;
            }
        }

        public Vector3 Offset { get; set; }

        private void Awake()
        {
            if (_cameraReference == null)
            {
                _cameraReference = Camera.main;
            }
        }

        private void Start()
        {
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            var uiPos = _cameraReference.WorldToScreenPoint(TransformReference.position + Offset);
            transform.position = uiPos + Vector3.up * _verticalOffset;
        }

        private void Update()
        {
            UpdatePosition();
            _verticalOffset += Time.deltaTime * _movementSpeed;

            _time += Time.deltaTime;

            if (_time >= _lifeTime - FADE_TIME)
            {
                var ftStart = _lifeTime - FADE_TIME;

                float t = Mathf.InverseLerp(ftStart, _lifeTime, _time);

                _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, 1 - t);
            }

            if (_time >= _lifeTime)
            {
                Destroy(gameObject);
            }
        }
    }
}
