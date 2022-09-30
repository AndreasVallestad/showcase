using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valley.FSM{
    //State is passed generic Type, pass the Type of the State Machine it belongs to, or make it Generic to work with multiple differently typed stateMachines
    public abstract class FsmState<T> where T : MonoBehaviour
    {
        //Reference to the state machine this state belongs to.
        protected FiniteStateMachine<T> stateMachine;

        public FsmState(FiniteStateMachine<T> stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        //Called once whenever the state is entered.
        public virtual void EnterState()
        {}
        //Called every frame this state is active in the state machine.
        public virtual void UpdateState()
        {}
        //Called when this state is switched out or otherwise exited.
        public virtual void ExitState()
        {}
    }
}
