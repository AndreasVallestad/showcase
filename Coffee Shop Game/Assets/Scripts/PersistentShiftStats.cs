using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valley;

namespace Funksoft.Barista
{
    public class PersistentShiftStats : MonoBehaviourSingleton<PersistentShiftStats>
    {
        public int CurrentDayIndex{ get; set;}
        public int TotalCustomers{ get; set; }
        public int CompletedCustomers{ get; set; }
        public int FailedCustomers{ get; set;}

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this);
        }
        
        public void NewShiftStarted()
        {
            Destroy(this);
        }

        
    }
}
