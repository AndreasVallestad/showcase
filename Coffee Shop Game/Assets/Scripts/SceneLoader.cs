using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valley;

namespace Funksoft.Barista
{
    public class SceneLoader : MonoBehaviourSingleton<SceneLoader>
    {

        public int StoredIndex{ get; set; }
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this);
        }


        public void LoadNextScene()
        {
            var nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

            //Load next scene if this scene is not the last one.
            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                Debug.Log("Loading Next Scene");
                SceneManager.LoadScene(nextSceneIndex);
                return;
            }
            Debug.Log("Cannot load next scene. Current scene is last in build index");
            return;
        }

        //I know, this is really bad.
        public void LoadShiftScene(int dayIndex)
        {
            StoredIndex = dayIndex;
            Debug.Log("Loading Shift Scene with DayIndex: " + StoredIndex);
            SceneManager.LoadScene(1);
        }
        public void LoadPostShiftScene()
        {
            SceneManager.LoadScene(2);
        }

        public void RestartCurrentScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
