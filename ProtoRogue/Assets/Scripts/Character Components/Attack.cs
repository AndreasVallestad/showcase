using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProtoRogue{
    public class Attack : MonoBehaviour
    {
        //Todo: replace damage variable with damage value from weapon or other source for attack
        public float damage = default;
        //Todo: replace attackpattern with pattern assigned by weapon or other source of attack
        private AttackPattern attackPattern = new AttackPattern(new Vector2Int(-1,1), new Vector2Int(0,1), new Vector2Int(1,1));

        //Cached reference to the Grid holding monobehaviour
        private LevelGrid levelGrid = null;

        private void Start()
        {
            levelGrid = FindObjectOfType<LevelGrid>();
        }

        //Attempt to attack in a direction. Attempt an attack against any objects 
        public bool AttackInDirection(Vector2Int attackDir)
        {
            //Get current position for use when attacking relative positions.
            Vector2Int currentGridPos = levelGrid.Grid.WorldToGridPos(transform.position);
            //Bool to hold if a target was attacked. False if there was no valid target to attack.
            bool attackedTarget = false;

            switch(attackDir)
            {
                //Compare moveDir to a vector v, and trigger case if v is also Vector2.up
                case Vector2Int v when v.Equals(Vector2Int.up):
                    //Attack any attackable targets in each cell that the attack pattern contains (relative to the players pos and given attack direction).
                    foreach(Vector2Int targetCell in attackPattern.GetRelativePatternAt(currentGridPos, AttackPattern.Dir.Up))
                    {
                        //Check if there is an object to attempt an attack against in the cell.
                        var targetInst = levelGrid.Grid.GetCell(targetCell)?.containedObject;
                        if (targetInst)
                        {
                            //Attack the target and return wether or not attack was successful.
                            attackedTarget = AttackObject(targetInst);
                        }
                    }
                    break;

                case Vector2Int v when v.Equals(Vector2Int.down):

                    //Attack any attackable targets in each cell that the attack pattern contains (relative to the players pos and attack direction).
                    foreach(Vector2Int targetCell in attackPattern.GetRelativePatternAt(currentGridPos, AttackPattern.Dir.Down))
                    {
                        //Check if there is an object to attempt an attack against in the cell.
                        var targetInst = levelGrid.Grid.GetCell(targetCell)?.containedObject;
                        if (targetInst)
                        {
                            //Attack the target and return wether or not attack was successful.
                            attackedTarget = AttackObject(targetInst);
                        }
                    }
                    break;

                case Vector2Int v when v.Equals(Vector2Int.right):
                    //Attack any attackable targets in each cell that the attack pattern contains (relative to the players pos and attack direction).
                    foreach(Vector2Int targetCell in attackPattern.GetRelativePatternAt(currentGridPos, AttackPattern.Dir.Right))
                    {
                        //Check if there is an object to attempt an attack against in the cell.
                        var targetInst = levelGrid.Grid.GetCell(targetCell)?.containedObject;
                        if (targetInst)
                        {
                            //Attack the target and return wether or not attack was successful.
                            attackedTarget = AttackObject(targetInst);
                        }
                    }
                    break;

                case Vector2Int v when v.Equals(Vector2Int.left):
                    //Attack any attackable targets in each cell that the attack pattern contains (relative to the players pos and attack direction).
                    foreach(Vector2Int targetCell in attackPattern.GetRelativePatternAt(currentGridPos, AttackPattern.Dir.Left))
                    {
                        //Check if there is an object to attempt an attack against in the cell.
                        var targetInst = levelGrid.Grid.GetCell(targetCell)?.containedObject;
                        if (targetInst)
                        {
                            //Attack the target and return wether or not attack was successful.
                            attackedTarget = AttackObject(targetInst);
                        }
                    }
                    break;
                default:
                    Debug.LogError("No matching cases for attack direction value");
                    break;
                
            }
            //Return if a target was sucessfully attacked (A target was found, and it was possible to attempt an attack).
            return attackedTarget;
        }

        private void AttackPosition()
        {

        }

        //Attack the target object, return bool of if object is attackable.
        private bool AttackObject(GameObject targetObject)
        {
            //if the object is attackable (has script that implements IAttackable), call the scripts OnAttack
            IAttackable attackableInst;
            if (targetObject.TryGetComponent<IAttackable>(out attackableInst))
            {
                //Pass self (this script) as the attacker to the attackable component
                attackableInst.OnAttacked(this);
                //return true, since the object was attacked
                return true;
            }
            //return false if object was not attackable
            return false;
        }
    }
}
