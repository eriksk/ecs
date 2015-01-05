using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecs.Core
{
    /// <summary>
    /// List that can add and remove objects when iterating with a foreach
    /// </summary>
    internal class DynamicList<T> : IEnumerator, IEnumerable
    {
        private readonly List<T> _items, _newItems, _removedItems; 
        private int _current;

        public DynamicList()
        {
            _items = new List<T>();
            _newItems = new List<T>();
            _removedItems = new List<T>();

            Reset();
        }

        public void Clear()
        {
            _items.Clear();
            _newItems.Clear();
            _removedItems.Clear();
        }

        public T this[int index]
        {
            get
            {
                Syncronize();
                return _items[index];
            }
        }

        public List<T> Items
        {
            get { return _items.Concat(_newItems).Concat(_removedItems).ToList(); }
        }

        public int Count 
        {
            get { return _items.Count + _newItems.Count + _removedItems.Count; }
        }

        public bool MoveNext()
        {
            _current++;
            return _current < _items.Count;
        }

        public void Reset()
        {
            _current = -1;
            Syncronize();
        }

        public void Add(T item)
        {
            _newItems.Add(item);
        }

        public void Remove(T item)
        {
            _removedItems.Add(item);
        }

        private void Syncronize()
        {
            while (_newItems.Count > 0)
            {
                _items.Add(_newItems[0]);
                _newItems.RemoveAt(0);
            }

            while (_removedItems.Count > 0)
            {
                _items.Remove(_removedItems[0]);
                _removedItems.RemoveAt(0);
            }
        }

        public object Current 
        {
            get { return _items[_current]; }
        }

        public IEnumerator GetEnumerator()
        {
            Reset();
            return this;
        }
    }
}
