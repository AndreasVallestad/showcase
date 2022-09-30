using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valley;
using System.Linq;

namespace Funksoft.Barista
{
    [CreateAssetMenu(menuName = "DataObject/Database", fileName = "new database SO")]
    public class DatabaseSO : ScriptableObject
    {
        [SerializeField]
        public SerializableHashSet<DrinkRecipeData> DrinkRecipes;

        [SerializeField]
        public SerializableHashSet<MainIngredientData> MainIngredients;

        [SerializeField]
        public SerializableHashSet<SideIngredientData> SideIngredients;

        [SerializeField]
        public SerializableHashSet<CustomerData> CustomerTypes;

        [SerializeField]
        public SerializableHashSet<DayData> Days;
    
        private void OnEnable()
        {
            Debug.Log("DatabaseSO Enabled");

            //Sort the recipelist alphabetically by name, makes for easier matching of ingredients in recipes.
            foreach(DrinkRecipeData r in DrinkRecipes.HashSet)
                r.Ingredients = r.Ingredients.OrderBy(e => e.Name).ToList();
        }
    }
}
