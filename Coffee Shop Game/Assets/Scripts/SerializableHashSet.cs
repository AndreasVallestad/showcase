using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Valley
{
    //I know this is ugly and nonsensical. Unity's own insanity around serialization makes it have to be this way.
    [System.Serializable]
    public class SerializableHashSet<T> : ISerializationCallbackReceiver
    {
        //The actual hashset to access for data contents. Had to be wrapped in this class due to internal problems with unity's deserialization.
        [HideInInspector]
        public HashSet<T> HashSet = new HashSet<T>();

        //List that can be serialized and displayed in Unity Inspector. Contents will be converted and transferred to HashMap when deserialized.
        [SerializeField]
        private List<T> _items = new List<T>();

        public void OnBeforeSerialize()
        {
            //Copy data from Hashset to Unity-Serializable List.
            _items = new List<T>(HashSet);
        }

        public void OnAfterDeserialize()
        {
            //Reaquire data from list after serialization
            HashSet = new HashSet<T>(_items);
        }
    }
}
