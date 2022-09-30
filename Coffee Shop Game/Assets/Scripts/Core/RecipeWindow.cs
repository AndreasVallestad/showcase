using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

namespace Funksoft.Barista
{
    public class RecipeWindow : MonoBehaviour
    {
        [SerializeField]
        private DatabaseSO _database;
        [SerializeField]
        private int _pageIndex = 0;

        [SerializeField]
        private int _totalPages = 1;

        [SerializeField]
        private List<DrinkRecipeData> _leftRecipes;

        [SerializeField]
        private List<DrinkRecipeData> _rightRecipes;

        [SerializeField, Header("\nContent References\n")]
        private Image _leftDrinkImage;
        [SerializeField]
        private Image _rightDrinkImage;

        [SerializeField]
        private Image _leftFillImage;
        [SerializeField]
        private Image _rightFillImage;

        [SerializeField]
        private TextMeshProUGUI _leftTitleText;
        [SerializeField]
        private TextMeshProUGUI _rightTitleText;

        [SerializeField]
        private TextMeshProUGUI _leftDescText;
        [SerializeField]
        private TextMeshProUGUI _rightDescText;

        [SerializeField]
        private RecipeContentDisplay _leftRecipeContentDisplay;
        [SerializeField]
        private RecipeContentDisplay _rightRecipeContentDisplay;

        private void Start()
        {
            //Sort recipes into left and right recipebook lists, alternating.
            var index = 0;
            foreach(DrinkRecipeData dr in _database.DrinkRecipes.HashSet)
            {
                if (index % 2 == 0)
                    _leftRecipes.Add(dr);
                else
                    _rightRecipes.Add(dr);
                index++;
            }
            //Get the total amount of pages the recipes add up to (assuming 2 recipes per page, and the final recipe of odd numbered count gets its own page, still.)
            _totalPages = Mathf.CeilToInt((index+1)/2);
            LoadPageData(_pageIndex);
        }

        public void NextPage()
        {
            //Wrap around to first index if we are brought beyond the page count
            if (_pageIndex >= _totalPages-1)
                _pageIndex = 0;
            else
                _pageIndex++;
            
            _leftRecipeContentDisplay.ClearDisplayIngredients();
            _rightRecipeContentDisplay.ClearDisplayIngredients();
            LoadPageData(_pageIndex);
        }
        public void PrevPage()
        {
            //Wrap around to last index, if we are brought below the first index
            if (_pageIndex == 0)
                _pageIndex = _totalPages-1;
            else
                _pageIndex--;

            _leftRecipeContentDisplay.ClearDisplayIngredients();
            _rightRecipeContentDisplay.ClearDisplayIngredients();
            LoadPageData(_pageIndex); 
        }

        //Populate UI elements with the data from the recipes
        private void LoadPageData(int index)
        {
            _leftTitleText.text = _leftRecipes[index].Name;
            _leftDescText.text = _leftRecipes[index].DescText;
            _leftDrinkImage.sprite = _leftRecipes[index].DrinkSprite;
            _leftFillImage.sprite = _leftRecipes[index].FillSprite;

            //Load the correct visual content display ingredients for the recipe, on the left content display window.
            _leftRecipeContentDisplay.LoadDisplayIngredients(index, _leftRecipes);
            
            //If we are on the last page, and there are not enough rightside recipes to fill the right side of the last page, 
            //...meaning there is an odd number of recipes, dont attempt to load the nonexistant recipe. Show blank info instead.
            if (_pageIndex == _totalPages-1 && _leftRecipes.Count > _rightRecipes.Count)
            {
                _rightTitleText.text = "";
                _rightDescText.text = "";
                _rightDrinkImage.sprite = null;
                _rightFillImage.sprite = null;
                return;
            }
            _rightTitleText.text = _rightRecipes[index].Name;
            _rightDescText.text = _rightRecipes[index].DescText;
            _rightDrinkImage.sprite = _rightRecipes[index].DrinkSprite;
            _rightFillImage.sprite = _rightRecipes[index].FillSprite;

            //Load the correct visual content display ingredients for the recipe, on the right content display window.
            _rightRecipeContentDisplay.LoadDisplayIngredients(index, _rightRecipes);
        }

        public void Open()
        {
            enabled = true;
        }

        public void Close()
        {
            gameObject.SetActive(false);
            BlockClickables.Instance.BlockEnabled = false;
        }
    }
}
