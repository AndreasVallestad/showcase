using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ProtoRogue
{
    public class Game : MonoBehaviour
    {
        private bool paused = false;
        private GameLoop gameLoop = new GameLoop();

        private void Start()
        {
            //Add all actors to the actor turn loop
            gameLoop.actors = FindObjectsOfType<Actor>().ToList();
            
            foreach(Actor a in gameLoop.actors)
            {
                Debug.Log(a);
            }
        }

        private void Update()
        {
            if (!paused)
                gameLoop.ProcessActors();
        }
    }
}

