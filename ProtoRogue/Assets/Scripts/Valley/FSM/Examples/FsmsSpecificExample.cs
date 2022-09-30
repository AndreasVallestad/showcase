using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valley.FSM.Examples{
    //This state is for a stateMachine of/for a specific Type, and can access and call any variable/function of that Type/script
    public class FsmsSpecificExample : FsmState<SpecificType>
    {
        private float variable;

        public FsmsSpecificExample(FiniteStateMachine<SpecificType> stateMachine) : base(stateMachine)
        {
            //Access Type/script specific stuff
            this.variable = stateMachine.owner.variable;
            stateMachine.owner.FunctionExample();
        }

    }
}

