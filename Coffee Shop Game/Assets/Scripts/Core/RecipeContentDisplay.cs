using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Funksoft.Barista
{
    public class RecipeContentDisplay : MonoBehaviour
    {
        [SerializeField]
        private RecipeWindow _recipeWindow;
        [SerializeField]
        private GameObject _contentPortionIngredientPrefab;
        [SerializeField]
        private float _verticalEdgeMargin = 40f; //How big the gap between the RecipeContentDisplay RectTransform edge and the correct start/end of the ingredients graphical display is.
        private float _fillableHeight = 170f; //Calculated in Awake. The UI coordinate heeight of the fillable area in the content display.

        private List<GameObject> _contentIngredientInstances = new List<GameObject>();
        private void Awake()
        {
            RectTransform rectTransform;
            TryGetComponent<RectTransform>(out rectTransform);
            _fillableHeight = rectTransform.rect.height - (_verticalEdgeMargin * 2);
        }

        public void LoadDisplayIngredients(int index, List<DrinkRecipeData> recipeList)
        {
            float totalVisualHeight = 0f;
            //Foreach Distinct ingredient entry in recipe, create a display object and set its properties correctly. .Select(x => x).Distinct()
            foreach(MainIngredientData dmi in recipeList[index].Ingredients.Select(x => x).Distinct())
            {
                //Count how many ingredients of the same type this recipe contains
                int thisIngCount = 0;
                foreach(MainIngredientData mi in recipeList[index].Ingredients)
                {
                    if (mi.Name != dmi.Name)
                        continue;
                    thisIngCount++;
                }
                //Get the proportion of the recipe these recipe slots constitute, and translate to corresponding visual display size.
                float proportion = (float)thisIngCount / (float)recipeList[index].Ingredients.Count;
                float fillValue = proportion * _fillableHeight;

                var inst = Instantiate(_contentPortionIngredientPrefab, this.transform.position, 
                                       Quaternion.identity, this.transform);
                _contentIngredientInstances.Add(inst);

                //rectTransform.offsetMax.x will be -(Proportion) to scale ingredient, add total height of previous ingredients to match. 
                RectTransform rectTransform;
                inst.TryGetComponent<RectTransform>(out rectTransform);
                rectTransform.offsetMin = new Vector2(0f, _verticalEdgeMargin + totalVisualHeight);
                rectTransform.offsetMax = new Vector2(0f, -_verticalEdgeMargin + fillValue - (_fillableHeight-totalVisualHeight));

                totalVisualHeight += fillValue;
            }
        }

        public void ClearDisplayIngredients()
        {
            foreach(GameObject ing in _contentIngredientInstances)
                Destroy(ing.gameObject);
        }

    }
}
