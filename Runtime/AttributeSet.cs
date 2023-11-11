using UnityEngine;
using System.Collections;

namespace Abilities
{
    [CreateAssetMenu(menuName = "Abilities/AttributeSet")]
    public class AttributeSet : ScriptableObject, IEnumerable
    {
        [SerializeField]
        private Attribute[] _attributes;


        // TODO Use something better for this maybe?
        public void Initialize()
        {
            for (int i = 0; i < _attributes.Length; i++)
            {
                Attribute a = _attributes[i];
                var n = a.CreateInstance(this);
                n.name = a.name;
                _attributes[i] = n;
            }

            for (int i = 0; i < _attributes.Length; i++)
            {
                Attribute a = _attributes[i];
                a.OnReady();
            }
        }

        public Attribute GetAttributeFromTemplate(Attribute template)
        {
            foreach (var a in _attributes)
            {
                if (a.Template == template)
                {
                    return a;
                }
            }
            return null;
        }

        public T GetAttributeFromTemplate<T>(Attribute template) where T : Attribute
        {
            var attr = GetAttributeFromTemplate(template);
            if (!attr) return null;
            return (T)attr;
        }

        public T GetAttribute<T>() where T : Attribute
        {
            foreach (var a in _attributes)
            {
                if (a is T)
                {
                    return a as T;
                }
            }
            return null;
        }
        public Attribute GetAttributeByName(string name)
        {
            foreach (var a in _attributes)
            {
                if (a.Name == name)
                {
                    return a;
                }
            }
            return null;
        }

        public T GetAttributeByName<T>(string name) where T : Attribute
        {
            foreach (var a in _attributes)
            {
                if (a is T attribute && attribute.Name == name)
                {
                    return attribute;
                }
            }
            return null;
        }

        public IEnumerator GetEnumerator()
        {
            return _attributes.GetEnumerator();
        }
    }

}
