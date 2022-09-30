using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valley.FSM{
    public class FsmsWait<T> : FsmState<T> where T : MonoBehaviour
    {
        private float secondsToWait; //Amount of seconds this state will "wait" switching to the next state.
        private FsmState<T> nextState; //The state to switch to when the wait is over.

        private float targetTime; //The target timestamp of when the wait time is over.

        //Wait in this state for a set amount of time (in seconds), before releasing control to the new determined state.
        public FsmsWait(FiniteStateMachine<T> stateMachine, float secondsToWait, FsmState<T> nextState) : base(stateMachine)
        {
            this.secondsToWait = secondsToWait;
            this.nextState = nextState;
        }

        public override void EnterState()
        {
            //Set target time of when the wait is over.
            targetTime = Time.time + secondsToWait;
        }
        public override void UpdateState()
        {
            //When the set time has passed, switch to the next state.
            if (Time.time >= targetTime)
            {
                stateMachine.SwitchState(nextState);
            }
        }
    }
}

