using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valley.BSP;

public class Room : MonoBehaviour
{
    public int X {get; private set;}
    public int Y {get; private set;}
    public int Width {get; private set;}
    public int Height {get; private set;}
    private int _minSize;
    private int _borderTrim;
    public BSP.Node Partition{get; set;}


    public void CreateRoom(BSP.Node partition, int minSize, int bordertrim = 1)
    {
        this.Partition = partition;
        this._minSize = minSize; 
        this._borderTrim = bordertrim; //How far from the edge of the partition its possible for the closest edge of the room to be

        DefineRoomBounds();

        transform.position = new Vector3(X, Y, 0f);
        SpriteRenderer spriteRenderer;
        TryGetComponent<SpriteRenderer>(out spriteRenderer);
        spriteRenderer.size = new Vector2(Width, Height);
        spriteRenderer.sortingOrder += 1;

        
    }

    private void Update()
    {

    }

    private void DefineRoomBounds()
    {
        //Set the dimensions of the room within the partition //HERE MINSIZE IF PARTITION SMALLER
        Width = Random.Range(_minSize, Partition.Width - _borderTrim*2 - 1); //-1 instead of -2 due to exclusive max value in Random.Range
        Height = Random.Range(_minSize, Partition.Height - _borderTrim*2 - 1); //-1 instead of -2 due to exclusive max value in Random.Range

        //Set the location of the room, such that its guaranteed to be contained iwthin the partition.
        X = Random.Range(Partition.X + _borderTrim, Partition.X + Partition.Width - _borderTrim - Width);
        Y = Random.Range(Partition.Y + _borderTrim, Partition.Y + Partition.Height - _borderTrim - Height);

    }


    


}
