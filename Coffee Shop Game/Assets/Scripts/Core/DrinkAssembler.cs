using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using pEventBus;

namespace Funksoft.Barista
{
    public class DrinkAssembler : MonoBehaviour
    {
        [SerializeField]
        private bool _debugLogsEnabled = false;

        [SerializeField]
        private DatabaseSO _databaseSO;

        [Header("Variables"), SerializeField, Range(0f, 1f)]
        private float minFillAmount = 0.9f;

        public DrinkRecipeData AssembleDrink(DrinkMixture drinkMixture)
        {
            //Dont assemble drink if its not filled to the minimum required amount of liquid.
            if (drinkMixture.GetTotalLiquid < minFillAmount * drinkMixture.MaxTotalLiquid)
            {
                if (_debugLogsEnabled)
                    TestUI.Log("Drink not filled enough to be a completed drink.");
                return null;
            }

            ScaleMixtureToFull(drinkMixture);

            var result = GetMatchingRecipe(drinkMixture);
            
            if (_debugLogsEnabled)
                TestUI.Log("Recipe Assembled: " + result?.Name);
            
            return result;
        }

        //Scales the values of each ingredient in the mixture proportionally to what they would be if the cup was full, but with the same proportion.
        //lowerFillLimit is a floor of how big a portion of the mixture max must be filled to allow this.
        private void ScaleMixtureToFull(DrinkMixture drinkMixture)
        {
            //Calculate how much larger the maximum liquid amount is, proportionally to the amount of liquid already in the cup.
            float inverseFilledProportion = drinkMixture.MaxTotalLiquid / drinkMixture.GetTotalLiquid;

            //Scale value amount of each ingredient up by the proportional amount required to reach max fill.
            foreach(MainIngredientData k in drinkMixture.MainIngredients.Keys.ToList())
            {
                drinkMixture.MainIngredients[k] *= inverseFilledProportion;
            }
        }
        
        //Matches contents of drink to corresponding recipe, return first alphabetical recipe that matches. Returns null if no matches are made.
        private DrinkRecipeData GetMatchingRecipe(DrinkMixture drinkMixture)
        {
            //Compare drinkmixture with each recipe to check if any match
            foreach(DrinkRecipeData recipe in _databaseSO.DrinkRecipes.HashSet)
            {
                //Holds the drinkmixture contents after conversion, for matching with recipe.
                List<MainIngredientData> convertedIngredients = new List<MainIngredientData>();
                
                //Convert the amounts of each ingredient in the mixture to match the slot format of the current recipe we are checking against.
                foreach(KeyValuePair<MainIngredientData, float> pair in drinkMixture.MainIngredients)
                {
                    //Convert the amount of liquid for this ingredient to the closest slot, assuming the total amount of liquid corresponds to the total amount of slots
                    int liquidAmountInSlots = Mathf.RoundToInt((pair.Value / drinkMixture.MaxTotalLiquid) * recipe.Ingredients.Count);
                    //Add a 
                    for(int i = 0; i < liquidAmountInSlots; i++)
                        convertedIngredients.Add(pair.Key);
                }
                //Sort to match order of Ingredient List in DatabaseSO, so we can compare them without ordering issues
                convertedIngredients = convertedIngredients.OrderBy(e => e.Name).ToList();

                if (Enumerable.SequenceEqual(convertedIngredients, recipe.Ingredients))
                    return recipe;
            }
            //If we reach the end with no match, there is no matching recipe.
            return null;
        }

    }
}
