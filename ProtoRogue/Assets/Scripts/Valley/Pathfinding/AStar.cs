using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valley;

namespace Valley.Pathfinding
{
    //On hiatus, TODO: Finish A* pathfinding
    public class AStar
    {
        //Grid to hold the gridnodes.
        private GenericGrid<AStarNode> Grid{get; set;}

        //List of nodes to evaluate.
        private List<AStarNode> openList;
        //List of nodes that have been evaluated or disqualified.
        private List<AStarNode> closedList;
        private AStarNode currentNode;
        
        //If pathfinding diagonally will be allowed by the algorithm.
        private bool allowDiagonal = true;

        public AStar(GenericGrid<AStarNode> grid, bool allowDiagonal)
        {
            this.Grid = grid;
            this.allowDiagonal = allowDiagonal;
        }

        //return a list of all the given nodes neighbors.
        public List<AStarNode> GetNodeNeighbors(int xPos, int yPos)
        {
            //Get position vectors for each neighbor
            var uNeighbor = new Vector2Int(xPos, yPos+1);
            var rNeighbor = new Vector2Int(xPos+1, yPos);
            var dNeighbor = new Vector2Int(xPos, yPos-1);
            var lNeighbor = new Vector2Int(xPos-1, yPos);
            //If path can also go diagonally, get the diagonal neighbors too.
            if (allowDiagonal)
            {
                var urNeighbor = new Vector2Int(xPos+1, yPos+1);
                var drNeighbor = new Vector2Int(xPos+1, yPos-1);
                var dlNeighbor = new Vector2Int(xPos-1, yPos-1);
                var ulNeighbor = new Vector2Int(xPos-1, yPos+1);
                //return list of all the nodes in the above positions, including diagonals.
                return Grid.GetCells(uNeighbor, rNeighbor, dNeighbor, lNeighbor, urNeighbor, drNeighbor, dlNeighbor, ulNeighbor);   
            }
            //return list of all the nodes in the above positions, excluding diagonals
            return Grid.GetCells(uNeighbor, rNeighbor, dNeighbor, lNeighbor);    
        }

        public void FindPath(int x, int y)
        {

        }
    }

    public class AStarNode
    {
        public int GCost{get; set;}
        public int HCost{get; set;}
        public int FCost{get{return GCost + HCost;}}

        public AStarNode PathParent{get; set;}

        public AStarNode(){}

    }
}

