using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valley.FSM;

namespace ProtoRogue{
    public class FsmsGsEmpty : FsmState<TurnStateMachine>
    {

        public FsmsGsEmpty(FiniteStateMachine<TurnStateMachine> stateMachine) : base(stateMachine)
        {
        }

        public override void UpdateState()
        {
            //If backspace switch back to player turn (DEBUG)
            //if (Input.GetKeyDown(KeyCode.Backspace)) //Comment out line to instantly switch back to player turn state instead.
            {
                stateMachine.SwitchState(new FsmsGsPlayerTurn(stateMachine));
            }
        }

    }
}

