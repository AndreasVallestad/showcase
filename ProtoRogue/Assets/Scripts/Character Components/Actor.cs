using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace ProtoRogue{
    [RequireComponent(typeof(IController))]
    public class Actor : MonoBehaviour
    {
        //Get the controller for this character, and run its behaviour for this turn
        public TurnState ProcessTurn()
        {
            IController controller;
            TryGetComponent<IController>(out controller);
            return controller.DoTurn();
        }

        public enum TurnState
        {
            Finished,
            Waiting
        }
    }
}