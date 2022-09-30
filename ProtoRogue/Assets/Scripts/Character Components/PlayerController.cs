using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace ProtoRogue
{
    public class PlayerController : MonoBehaviour, IController
    {
        private PlayerInput input = null;
        private CharacterMovement movement = null;
        private Attack attack = null;
        

        private void Awake()
        {
            //Cache references got from state machine owner locally.
            TryGetComponent<PlayerInput>(out input);
            TryGetComponent<CharacterMovement>(out movement);
            TryGetComponent<Attack>(out attack);
        }

        public Actor.TurnState DoTurn()
        {
            //Get the Player Input values for player movement/attack.
            Vector2Int inputDir = input.GetMovementInput();

            //If there is a movement input, attempt to move the player in the direction from the Input.
            if (inputDir != Vector2Int.zero)
            {
                //Do attack: If the player was able to attack successfully, finish turn
                if (attack.AttackInDirection(inputDir))
                    return Actor.TurnState.Finished;
                //Do movement: If the player was able to move successfully, finish turn
                else if (movement.MoveInDirection(inputDir))
                    return Actor.TurnState.Finished;
            }
            else if (Input.GetKeyDown(KeyCode.Space))
                return Actor.TurnState.Finished;

            //If not action is done, return enum so loop knowns to wait and continue executing this turn until an action is taken.
            return Actor.TurnState.Waiting;
        }
    }
}

