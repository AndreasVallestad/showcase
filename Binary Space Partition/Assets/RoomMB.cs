using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMB : MonoBehaviour
{
    public Room Room{ get; set;}

    void Start()
    {
        transform.position = new Vector3(Room.X, Room.Y, 0f);
        SpriteRenderer spriteRenderer;
        TryGetComponent<SpriteRenderer>(out spriteRenderer);
        spriteRenderer.size = new Vector2(Room.Width, Room.Height);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
