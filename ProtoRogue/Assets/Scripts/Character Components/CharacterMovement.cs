using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valley.FSM;

namespace ProtoRogue
{
    public class CharacterMovement : MonoBehaviour
    {
        //Cached reference to the Grid holding monobehaviour
        private LevelGrid levelGrid = null;
        //How close to the target pos is enough to have "reached" it (visual movement of objects)
        private readonly float targetReachedThreshhold = 0.05f;
        //Stored position of the current grid cell the character is in.
        [SerializeField]
        private Vector2Int currentGridPos;
        public Vector2Int CurrentGridPos
        {
            get { return currentGridPos; }
            private set { currentGridPos = value; }
        }
        //Stored position of the grid cell that we attempt to move the character to.
        //private Vector2Int targetGridPos;
        //World position of target position, chosen and stored after movement input
        private Vector2 moveTargetPos;

        void Awake()
        {
            levelGrid = FindObjectOfType<LevelGrid>();
        }

        private void Start()
        {
            //Store our current grid position based on initial transform position.
            CurrentGridPos = levelGrid.Grid.WorldToGridPos(transform.position);
            //Reset current position according to grid cell to ensure correct placement in cell.
            transform.position = levelGrid.Grid.GridToWorldPos(CurrentGridPos, true);
            //Add self gameobject as the object in the gridcell
            levelGrid.Grid.GetCell(CurrentGridPos).containedObject = this.gameObject;
            moveTargetPos = transform.position;
        }

        public bool MoveInDirection(Vector2Int moveDir)
        {

            #region Movement Switch
            //Temporary value to hold the target cell of the movement during calculation
            Vector2Int targetGridPos = CurrentGridPos;

            switch (moveDir)
            {
                //Compare moveDir to a vector v, and trigger case if v is also Vector2.up
                case Vector2Int v when v.Equals(Vector2Int.up):
                    //Get the position of the target cell in this direction.
                    targetGridPos = new Vector2Int(CurrentGridPos.x, CurrentGridPos.y + 1);
                    break;

                case Vector2Int v when v.Equals(Vector2Int.down):
                    //Get the position of the target cell in this direction.
                    targetGridPos = new Vector2Int(CurrentGridPos.x, CurrentGridPos.y - 1);
                    break;

                case Vector2Int v when v.Equals(Vector2Int.right):
                    //Get the position of the target cell in this direction.
                    targetGridPos = new Vector2Int(CurrentGridPos.x + 1, CurrentGridPos.y);
                    break;

                case Vector2Int v when v.Equals(Vector2Int.left):
                    //Get the position of the target cell in this direction.
                    targetGridPos = new Vector2Int(CurrentGridPos.x - 1, CurrentGridPos.y);
                    break;
                default:
                    Debug.LogError("No matching cases for movement direction value");
                    break;
            }
            #endregion

            //If movement has not been cancelled, return that movement was successful;
            return MoveToGridPos(targetGridPos);

            
        }

        //Move the character to a new grid position and update everything to reflect it.
        public bool MoveToGridPos(Vector2Int targetGridPos)
        {
            //Cancel movement if the target cell is not walkable.
            if (!levelGrid.Grid.GetCell(targetGridPos).IsWalkable) {return false;}
            if (levelGrid.Grid.GetCell(targetGridPos).containedObject) {return false;}

            //Set the previous cell to be empty, as it no longer contains this object after the movement.
            levelGrid.Grid.GetCell(CurrentGridPos).containedObject = null;
            //Set the new target cell to contain this object as we are moving there this movement.
            levelGrid.Grid.GetCell(targetGridPos).containedObject = this.gameObject;
            //Update our current position variable.
            CurrentGridPos = targetGridPos;
            //Set the movement target position for the visual world space movement to our new target position.
            moveTargetPos = levelGrid.Grid.GridToWorldPos(targetGridPos, true);

            return true;
        }


        private void Update()
        {
            //If target has been reached (within a minimal threshhold), consider it reached and cancel further movement
            var distanceToTarget = Vector3.Distance(transform.position, moveTargetPos);
            if (distanceToTarget <= targetReachedThreshhold)
            {
                transform.position = moveTargetPos;
                return;
            }
            //Move towards target location if not at target already
            transform.position = Vector3.MoveTowards(transform.position, moveTargetPos, (distanceToTarget / 0.1f) * Time.deltaTime);
        }


    }
}
