using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valley.FSM;
using System;

namespace ProtoRogue
{
    public class GoblinController : MonoBehaviour, IController
    {
        //private FsmState<GoblinController> initialState;
        //private FiniteStateMachine<GoblinController> stateMachine;

        #region Store cached component references
        private CharacterMovement characterMovement = null;
        private PathMovement pathMovement = null;
        private Health health = null;
        private Attack attack = null;
        #endregion

        private LevelGrid levelGrid = null;
        //private TurnStateMachine turnStateMachine = null; //Used in old Pre-Actor fsm gamestate system
        private CharacterMovement playerMovement = null;


        void Awake()
        {
            //Cache component references
            TryGetComponent<CharacterMovement>(out characterMovement);
            TryGetComponent<PathMovement>(out pathMovement);
            TryGetComponent<Health>(out health);
            TryGetComponent<Attack>(out attack);

            //Cache component references of other objects.
            levelGrid = FindObjectOfType<LevelGrid>();
            //turnStateMachine = FindObjectOfType<TurnStateMachine>();
            playerMovement = GameObject.FindWithTag("Player").GetComponent<CharacterMovement>();

            //Create and initialize state machine.
            //stateMachine = new FiniteStateMachine<GoblinController>(this);
            //stateMachine.SwitchState(new FsmsGoblinChase(stateMachine));
        }
        #region old Pre-Actor gamestate stuff
        /*
        private void OnEnable()
        {
            turnStateMachine.TurnEntered += CheckTurnEnter;
        }
        private void OnDisable()
        {
            turnStateMachine.TurnEntered -= CheckTurnEnter;
        }

        private void CheckTurnEnter(TurnStateMachine.GameTurnState stateTypeEnum)
        {
            //If the state entered is the monster state, do our turn.
            if (stateTypeEnum == TurnStateMachine.GameTurnState.Monster)
            {
                DoTurn();
            }
        }
        */
        #endregion

        //Todo: Refactor and extract behaviour into state machine
        public Actor.TurnState DoTurn()
        {
            #region Do attack if possible.
            //Get vector to player
            var toTargetVector = playerMovement.CurrentGridPos - characterMovement.CurrentGridPos;
            //If player is within one space, attack it.
            if (toTargetVector.magnitude == 1)
            {
                //Convert Vector to Vector2Int direction.
                var toTargetDir = new Vector2Int((int)Math.Sign(toTargetVector.x), (int)Math.Sign(toTargetVector.y));
                //Attack in that direction.
                attack.AttackInDirection(toTargetDir);
                return Actor.TurnState.Finished;
            }
            #endregion
            else
            {
                pathMovement.MoveAlongPath(pathMovement.GetGridPath(characterMovement.CurrentGridPos, playerMovement.CurrentGridPos));
                return Actor.TurnState.Finished;
            }
        }
    }
}