using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProtoRogue{
    public class GridCell
    {
        //Bool used for movement check, decides if the square can be moved onto
        public bool IsWalkable{get; set;} = true;

        //Object in grid square
        public GameObject containedObject = null;

        public GridCell()
        {
            
        }

    }
}

