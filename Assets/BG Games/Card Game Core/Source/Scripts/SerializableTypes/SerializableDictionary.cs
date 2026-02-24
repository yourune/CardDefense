using System.Collections.Generic;
using UnityEngine;

namespace BG_Games.Card_Game_Core.SerializableTypes
{
    [System.Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {

        [SerializeField] private List<TKey> _keys = new List<TKey>();
        [SerializeField] private List<TValue> _values = new List<TValue>();
    
        public void OnBeforeSerialize()
        {
            _keys.Clear();
            _values.Clear();
            foreach (KeyValuePair<TKey, TValue> pair in this) 
            {
                _keys.Add(pair.Key);
                _values.Add(pair.Value);
            }
        }
    
        public void OnAfterDeserialize()
        {
            this.Clear();

            if (_keys.Count != _values.Count) 
            {
                Debug.LogError("Tried to deserialize a SerializableDictionary, but the amount of keys ("
                               + _keys.Count + ") does not match the number of values (" + _values.Count 
                               + ") which indicates that something went wrong");
            }

            for (int i = 0; i < _keys.Count; i++) 
            {
                this.Add(_keys[i], _values[i]);
            }
        }

    }
}
