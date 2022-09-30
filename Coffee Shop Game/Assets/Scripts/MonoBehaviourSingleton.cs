using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valley
{
    [DisallowMultipleComponent]
    public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                //If we dont have an instance reference of the singleton type stored, get or make one.
                if (_instance == null)
                {   
                    //Check if there is an instance of T in the scene and use it as our single instance.
                    var existingInstances = FindObjectsOfType<T>();
                    if (existingInstances.Length > 0)
                        _instance = existingInstances[0];
                    //There should be no more than a single instance of a singleton. If there is, we have a problem.
                    if (existingInstances.Length > 1)
                        Debug.LogError("More than one existing instance of Singeton of type " + typeof(T).ToString() + " in Scene.");

                    //If there wasn't an instance of T in the scene, Create a new one and use that as our single instance
                    if (_instance == null)
                    {
                        var obj = new GameObject();
                        //Todo (maybe): give obj a name
                        _instance = obj.AddComponent<T>();
                    }
                }
                
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            //Exist only if no other instance exists. Otherwise destroy self.
            if (_instance == null)
                _instance = this as T;
            else
            {
                Destroy(this.gameObject);
                return;
            }
            
                
        }


    }
}
