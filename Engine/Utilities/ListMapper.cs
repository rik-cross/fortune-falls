using System.Collections.Generic;
using System;

namespace Engine
{
    public class ListMapper<T>
    {
        private List<T> _entityList;
        private Dictionary<T, int> _entityMapper;

        public ListMapper()
        {
            _entityList = new List<T>();
            _entityMapper = new Dictionary<T, int>();
        }

        // Add the entity to the list and mapper
        public void Add(object o)
        {
            if (o == null)
                return;

            _entityList.Add((T)o);
            _entityMapper[(T)o] = _entityList.Count - 1;
        }

        public bool Contains(object o)
        {
            if (_entityMapper.ContainsKey((T)o))
                return true;
            else
                return false;
        }

        public T Get(object o)
        {
            // Or use Contains()
            if (_entityMapper.TryGetValue((T)o, out int index))
                return _entityList[index];
            else
                return default;
        }

        // Return the list
        public List<T> GetAll()
        {
            return _entityList;
        }

        public bool Remove(object o)
        {
            // To keep the index values accurate in the mapper
            // and for fast removal of an entity from the list,
            // overwrite the current entity with the last entity
            // in the list and update the mapper.
            if (_entityMapper.ContainsKey((T)o))
            {
                // Get the index of the current entity
                int index = _entityMapper[(T)o];

                // Get the last entity at the end of the list
                object lastEntity = _entityList[^1];

                // Replace the current entity with the last entity
                _entityList[index] = (T)lastEntity;

                // Update the mapper with the new index value
                _entityMapper[(T)lastEntity] = index;

                // Remove the last entity from the list
                _entityList.RemoveAt(_entityList.Count - 1);

                // Remove the current entity from the mapper
                _entityMapper.Remove((T)o);

                // Testing
                Console.WriteLine($"Delete {o}:");
                Console.WriteLine(string.Join(", ", _entityList));
                foreach (KeyValuePair<T, int> kv in _entityMapper)
                    Console.WriteLine($"Key:{kv.Key} Value:{kv.Value}");

                return true;
            }
            else
                return false;
        }

    }
}
