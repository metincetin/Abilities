using UnityEngine;
using System.Collections;

namespace Abilities
{
    [CreateAssetMenu(menuName = "Ability/AttributeSet")]
	public class AttributeSet : ScriptableObject
	{
		[SerializeField]
		private Attribute[] _attributes;


        // TODO Use something better for this maybe?
        public void Initialize()
        {
            for (int i = 0; i < _attributes.Length; i++)
            {
                Attribute a = _attributes[i];
                var n = Instantiate(a);
                n.name = a.name;
                _attributes[i] = n;
            }
        }

        public Attribute GetAttributeFromTemplate(Attribute template)
        {
	        foreach (var a in _attributes)
	        {
		        if (a.GetType() == template.GetType())
		        {
			        return a;
		        }
	        }
	        return null;
        }
        
        public T GetAttributeFromTemplate<T>(Attribute template) where T: Attribute
        {
	        var attr = GetAttributeFromTemplate(template);
	        if (!attr) return null;
	        return (T)attr;
        }
        
		public T GetAttribute<T>() where T : Attribute
        {
			foreach(var a in _attributes)
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
			foreach(var a in _attributes)
            {
				if (a.name == name)
                {
					return a;
                }
            }
			return null;
        }

		public T GetAttributeByName<T>(string name) where T: Attribute
        {
			foreach(var a in _attributes)
            {
				if (a is T attribute && attribute.name == name)
                {
                    return attribute;
                }
            }
            return null;
        }
	}

}