using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Funksoft.Barista
{
    [CreateAssetMenu(menuName = "DataObject/DrinkRecipe", fileName = "R_RecipeName")]
    public class DrinkRecipeData : ScriptableObject
    {
        [SerializeField]
        public string Name;
        [SerializeField]
        public Sprite DrinkSprite;
        [SerializeField]
        public Sprite FillSprite;
        [SerializeField]
        public string DescText;
        [SerializeField]
        public List<MainIngredientData> Ingredients;
    }
}
