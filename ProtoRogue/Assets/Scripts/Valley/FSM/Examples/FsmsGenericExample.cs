using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valley.FSM.Examples{
    //Generic state can be used in stateMachines for/of any Type, but only access general MonoBehaviour vars/functions.
    public class FsmsGenericExample<T> : FsmState<T> where T : MonoBehaviour
    {
        private Transform ownerTransform; 

        public FsmsGenericExample(FiniteStateMachine<T> stateMachine) : base(stateMachine)
        {
            //Can only access General MonoBehaviour stuffs
            ownerTransform = stateMachine.owner.transform;
            stateMachine.owner.CompareTag("FunctionHereOnlyForDemoPurposes");
        }

    }
}

