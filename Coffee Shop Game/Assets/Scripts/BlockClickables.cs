using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valley;

namespace Funksoft.Barista
{
    public class BlockClickables : MonoBehaviourSingleton<BlockClickables>
    {
        [SerializeField]
        public bool BlockEnabled = false;
        
        protected override void Awake()
        {
            base.Awake();
        }
    }
}
