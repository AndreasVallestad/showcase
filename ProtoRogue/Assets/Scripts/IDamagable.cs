using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProtoRogue{
    //Interface implemented by the component handling the "taking damage logic" on an damagable object.
    public interface IDamagable
    {
        void TakeDamage(float damage);
    }
}