using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valley;
using UnityEngine.Tilemaps;
using System.Linq;

namespace ProtoRogue{
    public class LevelGrid : MonoBehaviour
    {
        [SerializeField]
        private int gridWidth = 10;
        [SerializeField]
        private int gridHeight = 10;
        [SerializeField]
        private float cellSize = 1;
        [SerializeField]
        private Tilemap tilemap = null;
        [SerializeField]
        private TileBase walkableTile = null;
        [SerializeField]
        private TileBase nonWalkableTile = null;

        //Vars for debug visual representation of paths.
        [SerializeField]
        public Tilemap pathTilemap = null;
        [SerializeField]
        public TileBase pathTile = null;
        [SerializeField]
        public TileBase finalPathTile = null;

        //Reference to the actual grid object.
        public GenericGrid<GridCell> Grid
        {
            get;
            private set;
        }

        void Awake()
        {
            //Create a Generic Grid to hold the actual grid data
            Grid = new GenericGrid<GridCell>(gridWidth, gridHeight, cellSize, cellSize, transform.position);
            //Set the properties of the tilemap and the grid component the tilemap corresponds to, to the values from our own custom GenericGrid.
            tilemap.layoutGrid.cellSize = new Vector2(this.cellSize, this.cellSize);
            tilemap.transform.position = new Vector3Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y), 0);

            pathTilemap.layoutGrid.cellSize = new Vector2(this.cellSize, this.cellSize);
            pathTilemap.transform.position = new Vector3Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y), 0);

            PopulateTilemap();
        }

        void Update()
        {
            //Mouseclick to gridcell test
            if (Input.GetMouseButtonDown(0))
            {
                //Convert mouse position from camera screen pixel position to World position
                var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //Convert mouse position from world pos to Grid pos
                Vector2 targetGridPos = Grid.WorldToGridPos(mousePos.x, mousePos.y);
                //Set variable of the grid cell entry at the converted mouse position
                var targetCell = Grid.GetCell(targetGridPos);
                targetCell.IsWalkable = !targetCell.IsWalkable;

                PopulateTilemap();
            }
        }

        //Set the tilemap tiles to correspond to the data in the equivilent gridcell (for all of them).
        private void PopulateTilemap()
        {
            //Cycle through Grid and match the tilemap tiles to the gridcells.
            for(int x = 0; x < Grid.GridArray.GetLength(0); x++)
            {
                for(int y = 0; y < Grid.GridArray.GetLength(1); y++)
                {
                    var currentCell = Grid.GetCell(x, y);
                    //Set walkable tile if gridcell data shows the cell is walkable.
                    if (currentCell.IsWalkable)
                    {
                        tilemap.SetTile(new Vector3Int(x, y, 0), walkableTile);
                    }
                    else
                    {
                        tilemap.SetTile(new Vector3Int(x, y, 0), nonWalkableTile);
                    }
                }
            }
        }

        public void DrawPath(List<Vector2Int> path, Tilemap tilemap, TileBase tile, TileBase goalTile)
        {
            //For every pos in the path, set a tile in the corresponding pos on the tilemap
            foreach(Vector2Int nodePos in path)
            {
                if (nodePos.Equals(path.Last()))
                {
                    pathTilemap.SetTile(new Vector3Int(nodePos.x, nodePos.y, 0), goalTile);
                    return;
                }
                    pathTilemap.SetTile(new Vector3Int(nodePos.x, nodePos.y, 0), tile);
            }
        }
    }
}

