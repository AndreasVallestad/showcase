using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Valley.Pathfinding
{   
    //Class for doing pathfinding with the Breadth First Search algorithm on square grids.
    public class BreadthFirstSearch
    {
        //Grid that holds the nodes for pathfinding
        private GenericGrid<PathNode> PathGrid{get;}

        public BreadthFirstSearch(GenericGrid<PathNode> pathGrid)
        {
            this.PathGrid = pathGrid;
        }

        //Path from the start gridpos to end gridpos, path: a list of pathnode positions in order, from start to end.
        public List<Vector2Int> GetPath(Vector2Int startPos, Vector2Int targetPos, bool allowDiagonals)
        {
            //Create a list to keep track of which nodes have already been visited.
            List<Vector2Int> visitedList = new List<Vector2Int>();
            //Create new queue to hold all nodes to be evaluated.
            Queue<Vector2Int> queue = new Queue<Vector2Int>();
            //Add initial starting position to queue for processing.
            queue.Enqueue(startPos);
            
            //If the queue is not empty, aka there are still nodes to evaluate
            while(queue.Count > 0)
            {
                //Remove the next node in the queue.
                var currentNode = queue.Dequeue();

                //If the current node is the target, return a list of all node positions from the start (excluding the starting node) to the target.
                if (currentNode.Equals(targetPos))
                {
                    var node = targetPos;
                    List<Vector2Int> finalPath = new List<Vector2Int>();
                    //Recursively add each nodes parent to the list until we get to the starting node
                    while(node != startPos)
                    {
                        finalPath.Add(node);
                        //The parent of this node is the next to be evaluated.
                        node = PathGrid.GetCell(node).parentNode;
                    }
                    //Reverse path list to get it ordered from start to goal, instead of goal to start.
                    finalPath.Reverse();
                    //Return the final list of nodes from start to target (excluding the startNode). This list is the "Path".
                    return finalPath;
                }

                //Add each new neighbor of the current node to the queue, and set the current node as its parent node.
                foreach(Vector2Int neighborNode in GetNodeNeighbors(currentNode, allowDiagonals))
                {
                    //Add neighbor to queue if its walkable, and not in the queue already.
                    if (!visitedList.Contains(neighborNode) && PathGrid.GetCell(neighborNode).isWalkable)
                    {
                        //Add the node to the queue for future processing.
                        queue.Enqueue(neighborNode);
                        //Add the node to the visitedList to prevent checking it again.
                        visitedList.Add(neighborNode);
                        //Set the current node as the neighbors parent.
                        PathGrid.GetCell(neighborNode).parentNode = currentNode;
                    }
                }
            }
            Debug.Log("No valid path found!");
            //Idk when this would trigger
            return null;
        }

        //Get the positions of all the neighbors of the node in nodePos.
        private List<Vector2Int> GetNodeNeighbors(Vector2Int nodePos, bool allowDiagonals)
        {
            //Create list to hold all neighbor positions
            List<Vector2Int> neighborList = new List<Vector2Int>();
            #region //Add orthogonal position vectors for each neighbor.
            var v = new Vector2Int(nodePos.x, nodePos.y+1);
            if (v.x >= 0 && v.x < PathGrid.gridWidth && v.y >= 0 && v.y < PathGrid.gridHeight)
            {neighborList.Add(v);}

            v = new Vector2Int(nodePos.x+1, nodePos.y);
            if (v.x >= 0 && v.x < PathGrid.gridWidth && v.y >= 0 && v.y < PathGrid.gridHeight)
            {neighborList.Add(v);}

            v = new Vector2Int(nodePos.x, nodePos.y-1);
            if (v.x >= 0 && v.x < PathGrid.gridWidth && v.y >= 0 && v.y < PathGrid.gridHeight)
            {neighborList.Add(v);}

            v = new Vector2Int(nodePos.x-1, nodePos.y);
            if (v.x >= 0 && v.x < PathGrid.gridWidth && v.y >= 0 && v.y < PathGrid.gridHeight)
            {neighborList.Add(v);}
            #endregion  
            //Return the above orthogonal neighbors if diags are not allowed.
            if (!allowDiagonals)
            {
                //Shuffle list to prevent the path from doing all its moves on one axis, then the other.
                return neighborList.Shuffle().ToList();
            }

            #region //Add diagonal position vectors for each neighbor.
            v = new Vector2Int(nodePos.x+1, nodePos.y+1);
            if (v.x >= 0 && v.x < PathGrid.gridWidth && v.y >= 0 && v.y < PathGrid.gridHeight)
            {neighborList.Add(v);}

            v = new Vector2Int(nodePos.x+1, nodePos.y-1);
            if (v.x >= 0 && v.x < PathGrid.gridWidth && v.y >= 0 && v.y < PathGrid.gridHeight)
            {neighborList.Add(v);}

            v = new Vector2Int(nodePos.x-1, nodePos.y-1);
            if (v.x >= 0 && v.x < PathGrid.gridWidth && v.y >= 0 && v.y < PathGrid.gridHeight)
            {neighborList.Add(v);}

            v = new Vector2Int(nodePos.x-1, nodePos.y+1);
            if (v.x >= 0 && v.x < PathGrid.gridWidth && v.y >= 0 && v.y < PathGrid.gridHeight)
            {neighborList.Add(v);}
            #endregion
            //Return list of the neighbor nodes positions.
            return neighborList;
        }

        public class PathNode
        {
            //Position/Index of parent node (previous node in path).
            public Vector2Int parentNode = default;
            public bool isWalkable = false;
        }
    }
}

