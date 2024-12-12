using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Abilities
{
	[System.Serializable]
	public class TagContainer: IList<string>
	{
		[SerializeField]
		private List<string> _tags = new ();
        
		public IEnumerator<string> GetEnumerator()
		{
			return _tags.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Add(string item)
		{
			_tags.Add(item);
		}

		public void Clear()
		{
			_tags.Clear();
		}

		public bool Contains(string item)
		{
			return _tags.Contains(item);
		}

		public void CopyTo(string[] array, int arrayIndex)
		{
			_tags.CopyTo(array, arrayIndex);
		}

		public bool Remove(string item)
		{
			return _tags.Remove(item);
		}

		public int Count => _tags.Count;

		public bool IsReadOnly { get; } = false;
        
		public int IndexOf(string item)
		{
			return _tags.IndexOf(item);
		}

		public void Insert(int index, string item)
		{
			_tags.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			_tags.RemoveAt(index);
		}

		public string this[int index]
		{
			get => _tags[index];
			set => _tags[index] = value;
		}

		public void Merge(TagContainer tags)
		{
			foreach (var t in tags)
			{
				_tags.Add(t);
			}
		}

		public bool ContainsAny(TagContainer tags)
		{
			if (tags == null) return false;
			foreach (var t in tags)
			{
				if (_tags.Contains(t)) return true;
			}

			return false;
		}
	}
}
