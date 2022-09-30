using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Funksoft.Barista
{
    public class RecipeBook : MonoBehaviour, IClickable
    {
        [SerializeField]
        private string _displayName;
        [SerializeField]
        private RecipeWindow _recipeWindowPrefab;
        [SerializeField]
        private Canvas _canvas;

        [SerializeField]
        private Sprite _hoverSprite;
        [SerializeField]
        private Sprite _clickedSprite;

        public void OnActivation()
        {
            if (!_recipeWindowPrefab.gameObject.activeSelf)
            {
                _recipeWindowPrefab.gameObject.SetActive(true);
                BlockClickables.Instance.BlockEnabled = true;
            }         
        }
                
        //Provide name and sprites for this object and its clickable component states.
        public string GetDisplayName()
        {
            return _displayName;
        }
        
        public Sprite GetHoverSprite()
        {
            return _hoverSprite;
        }
        public Sprite GetClickedSprite()
        {
            return _clickedSprite;
        }
    }
}
