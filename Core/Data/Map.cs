// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System.Collections.Generic;
using System.Linq;

namespace SnowyPeak.Duality.Plugin.Frozen.Core.Data
{
    /// <summary>
    /// A two-way Dictionary that allows to lookup a Value given a Key or a Key given a Value.
    /// Based on http://stackoverflow.com/a/10966684
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class Map<K, V>
    {
        private Dictionary<K, V> _forward;
        private Dictionary<V, K> _reverse;

        /// <summary>
        /// Constructor
        /// </summary>
        public Map()
        {
            _forward = new Dictionary<K, V>();
            _reverse = new Dictionary<V, K>();
        }

        /// <summary>
        /// Returns the list of Keys as IEnumerable
        /// </summary>
        public IEnumerable<K> Keys
        {
            get { return _forward.Keys.AsEnumerable(); }
        }

        /// <summary>
        /// Returns the list of Values as IEnumerable
        /// </summary>
        public IEnumerable<V> Values
        {
            get { return _reverse.Keys.AsEnumerable(); }
        }
        /// <summary>
        /// Gets or Sets a Key, given a Value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public K this[V key]
        {
            get { return _reverse[key]; }
            set
            {
                _reverse[key] = value;
                _forward[value] = key;
            }
        }

        /// <summary>
        /// Gets or Sets a Value, given a Key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public V this[K key]
        {
            get { return _forward[key]; }
            set
            {
                _forward[key] = value;
                _reverse[value] = key;
            }
        }

        /// <summary>
        /// Checks if the Key is present in the Map
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(K item)
        {
            return _forward.ContainsKey(item);
        }

        /// <summary>
        /// Checks if the Value is present in the Map
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(V item)
        {
            return _reverse.ContainsKey(item);
        }

        /// <summary>
        /// Removes a Key and the associated Value from the Map
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(K key)
        {
            bool result = false;

            if (_forward.ContainsKey(key))
            {
                result = true;
                V item = _forward[key];

                result &= _forward.Remove(key);
                result &= _reverse.Remove(item);
            }

            return result;
        }

        /// <summary>
        /// Removes a Value and the associated Key from the Map
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(V key)
        {
            bool result = false;

            if (_reverse.ContainsKey(key))
            {
                result = true;
                K item = _reverse[key];

                result &= _reverse.Remove(key);
                result &= _forward.Remove(item);
            }

            return result;
        }
    }
}