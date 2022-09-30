using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProtoRogue{
    //Interface implemented by the component handling the "being attacked logic" on an attackable object.
    public interface IAttackable
    {
        //attackingInst = the Attack component instance doing the attacking of the attackable object.
        void OnAttacked(Attack attackingInst);
    }
}
