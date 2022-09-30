using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoRogue;

namespace Valley.FSM{
    //Finite state machine can be of any type that is, or inherits from, MonoBehaviours
    public class FiniteStateMachine<T> where T : MonoBehaviour
    {
        //Holds the current state of the state machine.
        private FsmState<T> currentState;
        //References the owner of the State Machine of Type T (Type/script that holds and uses State Machine)
        public T owner;

        public FsmState<T> CurrentState{
            get{return currentState;}
            private set{currentState = value;}
        }

        //State machines initialized with this constructor does nothing until SwitchState is used to set the first state.
        public FiniteStateMachine(T owner)
        {
            this.owner = owner;
        }
        //Initialize state machine with an initial state.
        public FiniteStateMachine(T owner, FsmState<T> initialState)
        {
            this.owner = owner;
            currentState = initialState;
            currentState.EnterState();
        }

        public void Update()
        {
            currentState?.UpdateState();
        }

        //Change states and run the appropriate state transition methods.
        public void SwitchState(FsmState<T> newState)
        {
            //Uncomment line below to print every state change in the console.
            //Debug.Log("<size=14><b>" + owner.ToString() + "</b> FSM switched to: <b>" + newState + "</b> from: <b>" + currentState + "</b></size>");
            currentState?.ExitState();
            currentState = newState;
            currentState.EnterState();

        }
    }
}

