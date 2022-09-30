using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ProtoRogue{
    public class GameLoop
    {
        public List<Actor> actors {get; set;} = new List<Actor>();
        public Actor currentActor {get; private set;}

        public void ProcessActors()
        {
            //Get the index of the current actor so we can continue the loop where we left off. TODO: refactor section so removing actors from list wont break it.
            int index = actors.IndexOf(currentActor);
            //If there is no current actor we want to continue the loop from, start at the beginning.
            if (index == -1) //List.IndexOf() returns -1 if argument is not found in list.
                index = 0;
            
            for(int i = index; i < actors.Count; i++)
            {
                //Save current actor so we can continue turns from the same place if we have to wait for something during the loop.
                currentActor = actors[i];

                //If there is no actor instance at the current index, remove the null entry from the list and reprocess loop index to account for list shift.
                if (currentActor == null)
                {
                    actors.RemoveAt(i);
                    i--;
                    continue;
                }
                    
                //If the actors whos turn it is needs to wait for something (f ex: player input), yield control by exiting loop
                if (currentActor.ProcessTurn() == Actor.TurnState.Waiting)
                    return;
            }
            //Reset if the turn finished as there is no currently waiting actor to continue the loop from.
            currentActor = null;
        }
    }
}