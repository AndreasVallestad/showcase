using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valley;

namespace Funksoft.Barista
{
    [CreateAssetMenu(menuName = "DataObject/Day", fileName = "D_N")]
    public class DayData : ScriptableObject
    {
        [SerializeField]
        public float ShiftTime = 180f;  //Time the shift lasts in seconds.
        
        [SerializeField]
        public float InitialCustomerDelay= 5f; 
        //The delay between customers spawning is random, between min and max values.
        [SerializeField, Min(0f)]
        public float MinTimeBetweenCustomers = 1f; //Minimum amount of time that passes before its possible for a customer to spawn, in seconds.
        [SerializeField, Min(1f)]
        public float MaxTimeBetweenCustomers =  20f; //Maximum amount of time that passes before its possible for a customer to spawn, in seconds.
        
        //Todo: Potentially add multiple timers/spawners?

        [SerializeField]
        public SerializableHashSet<CustomerData> PossibleCustomers; //List of customers that can be spawned during this shift.

        
    }
}
