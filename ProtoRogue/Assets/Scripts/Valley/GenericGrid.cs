using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valley{
    public class GenericGrid<T> where T : new()
    {
        //The width of the grid in the amount of cells.
        public readonly int gridWidth = 10;
        //The height of the grid in the amount of cells.
        public readonly int gridHeight = 10;
        //Width of the individual cells in the grid.
        public readonly float cellWidth = 1;
        //Height of the individual cells in the grid.
        public readonly float cellHeight = 1;
        //The world space position of the origin of this Grid. Important to adjust positions for when the grid pos is not at the world space origin
        public readonly Vector2 originPos;

        //Array to store grid positions of whatever generic Type is passed to the Grid class.
        public T[,] GridArray{
            get;
            private set;
        }

        public GenericGrid(int gridWidth, int gridHeight, float cellWidth, float cellHeight, Vector2 originPos)
        {
            //Save initial values of the grid;
            this.gridWidth = gridWidth;
            this.gridHeight = gridHeight;
            this.cellWidth = cellWidth;
            this.cellHeight = cellHeight;
            this.originPos = originPos;

            //Initialize cell data array
            GridArray = new T[gridWidth, gridHeight];

            //Cycle through all gridcells and populate with new instances of T.
            for (int x = 0; x < GridArray.GetLength(0); x++)
            {
                for (int y = 0; y < GridArray.GetLength(1); y++)
                {
                    //Populate array with instances of the selected Generic Type
                    GridArray[x, y] = new T();

                    //Draw debug line one unit in positive x and y directions to draw a grid
                    Debug.DrawLine(GridToWorldPos(x, y, false), GridToWorldPos(x, y+1, false), Color.black, Mathf.Infinity);
                    Debug.DrawLine(GridToWorldPos(x, y, false), GridToWorldPos(x+1, y, false), Color.black, Mathf.Infinity);
                }
            }
            //Draw debug lines at the very to and the very right of the grid to finish the debug view of the grid
            Debug.DrawLine(GridToWorldPos(0, gridHeight, false), GridToWorldPos(gridWidth, gridHeight, false), Color.black, Mathf.Infinity);
            Debug.DrawLine(GridToWorldPos(gridWidth, 0, false), GridToWorldPos(gridWidth, gridHeight, false), Color.black, Mathf.Infinity);
        }

        //Get a T instance from the array of Ts in the Grid
        public T GetCell(int x, int y)
        {
            return GridArray[x, y];
        }
        //Get a T instance from the array of Ts in the Grid
        public T GetCell(Vector2 pos)
        {
            return GridArray[Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y)];
        }
        
        //Get a list of T instances from the array of Ts in the Grid
        public List<T> GetCells(params Vector2[] posArray)
        {
            //Create the list that will hold the instances to return
            List<T> resultList = new List<T>();
            //Get the T instance of each position in the parameters, and add them all to the list.
            foreach(Vector2 cellReq in posArray)
            {
                resultList.Add(GridArray[Mathf.FloorToInt(cellReq.x), Mathf.FloorToInt(cellReq.y)]);
            }
            return resultList;
        }

        //Converts grid position to unity world position.
        public Vector2 GridToWorldPos(int xPos, int yPos, bool getCenterPos)
        {
            //Multiply cell positions by their size to get the world position. Add originPos to account for the world pos of the grid itself
            Vector2 result =  new Vector2(xPos * cellWidth + originPos.x, yPos * cellHeight + originPos.y);
            //Center return value in the center of grid cell (if param is true) by shifting it in the positive x + y directions.
            if (getCenterPos)
            {
                result.x += cellWidth/2; 
                result.y += cellHeight/2;
            }
            return result;
        }
        //Converts grid position to unity world position.
        public Vector2 GridToWorldPos(Vector2 pos, bool getCenterPos)
        {
            //Multiply cell positions by their size to get the world position
            Vector2 result =  new Vector2(pos.x * cellWidth + originPos.x, pos.y * cellHeight + originPos.y);
            //Center return value in the center of grid cell (if param is true) by shifting it in the positive x + y directions.
            if (getCenterPos)
            {
                result.x += cellWidth/2; 
                result.y += cellHeight/2;
            }
            return result;
        }

        //Converts grid position to unity world position.
        public Vector2Int WorldToGridPos(float xPos, float yPos)
        {
            //Divide world positions by cell size and floor them to get cell pos. Subtract originPos to account for the world pos of the grid itself Convert to Ints.
            Vector2Int result =  new Vector2Int(Mathf.FloorToInt(xPos - originPos.x / cellWidth), Mathf.FloorToInt(yPos - originPos.y / cellHeight));
            return result;
        }
        public Vector2Int WorldToGridPos(Vector2 pos)
        {
            //Divide world positions by cell size and floor them to get cell pos. Subtract originPos to account for the world pos of the grid itself Convert to Ints.
            Vector2Int result =  new Vector2Int(Mathf.FloorToInt(pos.x - originPos.x / cellWidth), Mathf.FloorToInt(pos.y - originPos.y / cellHeight));
            return result;
        }

    }
}
