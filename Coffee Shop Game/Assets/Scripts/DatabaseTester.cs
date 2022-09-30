using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Funksoft.Barista
{
    public class DatabaseTester : MonoBehaviour
    {
        [SerializeField]
        private DatabaseSO _database;



        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                foreach(DrinkRecipeData dr in _database.DrinkRecipes.HashSet)
                {
                    Debug.Log(dr.Name);
                }
                foreach(MainIngredientData mi in _database.MainIngredients.HashSet)
                {
                    Debug.Log(mi.Name);
                }
                foreach(SideIngredientData si in _database.SideIngredients.HashSet)
                {
                    Debug.Log(si.Name);
                    if (_database.SideIngredients.HashSet.Contains(si))
                        Debug.Log("YES!");
                }
            }
        }
    }
}
