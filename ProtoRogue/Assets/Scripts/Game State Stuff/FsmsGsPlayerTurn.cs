using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valley.FSM;

namespace ProtoRogue
{
    public class FsmsGsPlayerTurn : FsmState<TurnStateMachine>
    {
        private PlayerInput playerInput = null;
        private CharacterMovement playerMovement = null;
        private Attack playerAttack = null;
        

        public FsmsGsPlayerTurn(FiniteStateMachine<TurnStateMachine> stateMachine) : base(stateMachine)
        {
            //Cache references got from state machine owner locally.
            this.playerInput = stateMachine.owner.PlayerInput;
            this.playerMovement = stateMachine.owner.PlayerMovement;
            this.playerAttack = stateMachine.owner.PlayerAttack;
        }

        public override void UpdateState()
        {
            base.UpdateState();
            //Get the Player Input values for player movement/attack.
            Vector2Int playerInputDir = playerInput.GetMovementInput();
            //If there is a movement input, attempt to move the player in the direction from the Input.
            if (playerInputDir != Vector2Int.zero)
            {
                //Do attack: If the player was able to attack successfully, switch to next turn state
                if (playerAttack.AttackInDirection(playerInputDir))
                    stateMachine.SwitchState(new FsmsGsMonsterTurn(stateMachine));
                    
                //Do movement: If the player was able to move successfully, switch to next turn state
                else if (playerMovement.MoveInDirection(playerInputDir))
                    stateMachine.SwitchState(new FsmsGsMonsterTurn(stateMachine));
            }
            //DEBUG METHOD (for now)
            if (Input.GetKeyDown(KeyCode.Space))
                stateMachine.SwitchState(new FsmsGsMonsterTurn(stateMachine));
            
        }
    }
}
