using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valley.Pathfinding;
using System.Linq;

namespace ProtoRogue
{
    [RequireComponent(typeof(CharacterMovement))]
    public class PathMovement : MonoBehaviour
    {
        private LevelGrid levelGrid = null;
        private CharacterMovement characterMovement;

        //The cached path that is currently being followed (not yet in use)
        public List<Vector2Int> CurrentPath
        {
            get;
            private set;
        }

        void Awake()
        {
            //Todo replace find call and find better solution for pathfinding grid (use Levelgrid or other alternatives)
            levelGrid = FindObjectOfType<LevelGrid>();
            characterMovement = GetComponent<CharacterMovement>();
        }

        private void Update()
        {
        }


        public List<Vector2Int> GetGridPath(Vector2Int startPos, Vector2Int goalPos)
        {
            //Create a grid to do pathfinding calculations on.
            var pathGrid = new Valley.GenericGrid<BreadthFirstSearch.PathNode>(levelGrid.Grid.gridWidth, levelGrid.Grid.gridHeight,
                levelGrid.Grid.cellWidth, levelGrid.Grid.cellHeight, levelGrid.Grid.originPos);

            //Check each gridcell in LevelGrid and copy the walkable info to the pathGrid.
            for (var i = 0; i < levelGrid.Grid.gridWidth; i++)
            {
                for (var j = 0; j < levelGrid.Grid.gridHeight; j++)
                {
                    //Set the pathGrid walkability of this cell to match if the corresponding levelGrid cell is walkable or not.
                    pathGrid.GetCell(i, j).isWalkable = levelGrid.Grid.GetCell(i, j).IsWalkable;
                }
            }
            //Create a BFS instance to handle the pathfinding on the pathGrid.
            BreadthFirstSearch bfs = new BreadthFirstSearch(pathGrid);
            //Pathfind from start to target, get the path(list of cells to move in order).
            var path = bfs.GetPath(startPos, goalPos, false);
            if (path == null)
            {
                Debug.Log(gameObject.name + " failed to find path to target.");
                return null;
            }
            //Draw the path on a tilemap in levelGrid
            //levelGrid.DrawPath(path, levelGrid.pathTilemap, levelGrid.pathTile, levelGrid.finalPathTile);
            return path;
        }

        //Will move character along path. Returns true when character has reached their destination.
        public bool MoveAlongPath(List<Vector2Int> path)
        {
            //Cancel movement if there is no path to target.
            if (path == null)
                return false;
                
            //Get the index of how far along we are in the path.
            int currentPathIndex = path.IndexOf(characterMovement.CurrentGridPos);

            //Move to the next pos in the path (if we are not on the path, move to the first pos).
            if (currentPathIndex + 1 < path.Count && 
                characterMovement.MoveToGridPos(path[currentPathIndex + 1]))
            {
                //Update current path index to account the possible movement just made
                currentPathIndex = path.IndexOf(characterMovement.CurrentGridPos);
                //If the last position (aka. target position) of the path is this one, the path movement has been completed.
                if (path[currentPathIndex] == path.Last())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
