using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valley.FSM;
using TMPro;
using System.Linq;
using System;

namespace ProtoRogue
{
    //OBSOLETE, See GameLoop.cs for replacement.
    public class TurnStateMachine : MonoBehaviour
    {
        [SerializeField]
        private LevelGrid levelGrid;
        public LevelGrid LevelGrid
        {
            get { return levelGrid; }
            private set { levelGrid = value; }
        }

        //Component reference for use in player turn state
        [SerializeField]
        private PlayerInput playerInput;
        public PlayerInput PlayerInput
        {
            get { return playerInput; }
            private set { playerInput = value; }
        }
        //Component reference for use in player turn state
        [SerializeField]
        private CharacterMovement playerMovement;
        public CharacterMovement PlayerMovement
        {
            get { return playerMovement; }
            private set { playerMovement = value; }
        }
        //Component reference for use in player turn state
        [SerializeField]
        private Attack playerAttack;
        public Attack PlayerAttack
        {
            get { return playerAttack; }
            private set { playerAttack = value; }
        }
        
        //Reference to state machine instance.
        private FiniteStateMachine<TurnStateMachine> stateMachine;

        //Enum for checking for specific Turn States in the turn state managing FSM.
        public enum GameTurnState
        {
            Player,
            Monster
        }

        //Turn events//
        public event Action<GameTurnState> TurnEntered;
        public event Action<GameTurnState> TurnExited;


        private void Awake()
        {
            //Initialize stateMachine and initial state
            stateMachine = new FiniteStateMachine<TurnStateMachine>(this);
            stateMachine.SwitchState(new FsmsGsPlayerTurn(stateMachine));

            //Todo: replace
            //monsterList = FindObjectsOfType<GoblinController>().ToList();
        }

        void Update()
        {
            stateMachine.Update();
        }

        #region Event Management for Turn States
        //Launch event to let subscribers know that a turn state has started, pass the type of state as param.
        public void LaunchStateEnterEvent(Type stateType)
        {
            //var type = stateMachine.CurrentState.GetType();

            //Check the type of the state, and launch the TurnEntered event with the appropriate state type param.
            if (stateType == typeof(FsmsGsPlayerTurn))
            {
                //Launch event with gameturnstate param so subcribers can do actions based on the specific type of state entered.
                if (TurnEntered != null) TurnEntered(GameTurnState.Player);
            }
            else if (stateType == typeof(FsmsGsMonsterTurn))
            {
                //Launch event with gameturnstate param so subcribers can do actions based on the specific type of state entered.
                if (TurnEntered != null) TurnEntered(GameTurnState.Monster);
            }
            else
            {
                Debug.LogError("Invalid Type Error");
            }
        }
        //Launch event to let subscribers know that a turn state has started, pass the type of state as param.
        public void LaunchStateExitEvent(Type stateType)
        {
            //var type = stateMachine.CurrentState.GetType();

            //Check the type of the state, and launch the TurnEntered event with the appropriate state type param.
            if (stateType == typeof(FsmsGsPlayerTurn))
            {
                //Launch event with gameturnstate param so subcribers can do actions based on the specific type of state entered.
                if (TurnEntered != null) TurnExited(GameTurnState.Player);
            }
            else if (stateType == typeof(FsmsGsMonsterTurn))
            {
                //Launch event with gameturnstate param so subcribers can do actions based on the specific type of state entered.
                if (TurnEntered != null) TurnExited(GameTurnState.Monster);
            }
            else
            {
                Debug.LogError("Invalid Type Error");
            }
        }
        #endregion
    }
}
