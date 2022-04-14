using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lesson1
{
    internal class ThreadSafeList<T> : IEnumerable<T>, ICollection<T>
    {
        private readonly List<T> _list;
        private readonly object _lock = new object();

        public int Count
        {
            get
            {
                lock(_lock)
                {                    
                    return _list.Count;
                }                
            }
        }

        public bool IsReadOnly => false;

        public ThreadSafeList() : this(null) { }

        public ThreadSafeList(IEnumerable<T> items)
        {
            _list = items is null ? new List<T>() : new List<T>(items);            
        }

        public void Add(T item)
        {
            lock(_lock)
            {                
                _list.Add(item);
            }            
        }

        public void Clear()
        {
            lock (_lock)
            {              
                _list.Clear();
            }            
        }

        public bool Contains(T item)
        {
            lock (_lock)
            {                
                return _list.Contains(item);
            }            
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (_lock)
            {                
                _list.CopyTo(array, arrayIndex);
            }            
        }

        public bool Remove(T item)
        {
            lock (_lock)
            {                
                return _list.Remove(item);
            }            
        }

        private IEnumerable<T> Enumerate()
        {
            lock (_lock)
            {                
                foreach (T item in _list)
                    yield return item;
            }            
        }

        public IEnumerator<T> GetEnumerator()
            => Enumerate().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => Enumerate().GetEnumerator();
    }
}
