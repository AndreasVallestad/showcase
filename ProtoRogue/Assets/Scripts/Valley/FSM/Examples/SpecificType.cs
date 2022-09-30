using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valley.FSM.Examples{
    //Script only used as an example for how to access info from types and monobehaviour references in the statemachine
    public class SpecificType : MonoBehaviour
    {
        private FiniteStateMachine<SpecificType> stateMachine;
        public float variable = 0f;

        private void Start()
        {
            //Initialize the state machine
            stateMachine = new FiniteStateMachine<SpecificType>(this);
            //Switch to the initial state
            stateMachine.SwitchState(new FsmsSpecificExample(stateMachine));
        }

        private void Update()
        {
            stateMachine?.Update();
        }

        public void FunctionExample()
        {
            Debug.Log("Type/script specific function can be accessed in state!");
        }
    }
}
