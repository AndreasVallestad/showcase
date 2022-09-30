using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace ProtoRogue{
    public interface IController
    {
        Actor.TurnState DoTurn();
    }
}