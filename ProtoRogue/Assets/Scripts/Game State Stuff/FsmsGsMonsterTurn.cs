using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valley.FSM;
using System.Linq;

namespace ProtoRogue
{
    public class FsmsGsMonsterTurn : FsmState<TurnStateMachine>
    {
        public FsmsGsMonsterTurn(FiniteStateMachine<TurnStateMachine> stateMachine) : base(stateMachine)
        {

        }

        public override void EnterState()
        {
            base.UpdateState();
            //Launch entered event for subcribing monster instances.
            stateMachine.owner.LaunchStateEnterEvent(this.GetType());

            //Switch back to player turn after processing enemies.
            stateMachine.SwitchState(new FsmsGsPlayerTurn(stateMachine));
        }
    }
}

