using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProtoRogue{
    public class PlayerInput : MonoBehaviour
    {

        public Vector2Int GetMovementInput()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                return Vector2Int.up;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                return Vector2Int.down;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                return Vector2Int.right;
            } 
            else if (Input.GetKeyDown(KeyCode.A))
            {
                return Vector2Int.left;
            }
            return Vector2Int.zero;
        }

    }
}
