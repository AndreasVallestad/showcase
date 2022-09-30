using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valley;

namespace Funksoft.Barista
{
    public class StartButton : MonoBehaviour
    {

        //Hardcoded = Bad, Time = Short;
        public void LoadShiftScene()
        {
            MonoBehaviourSingleton<SceneLoader>.Instance.LoadShiftScene(0);
        }
    }
}
