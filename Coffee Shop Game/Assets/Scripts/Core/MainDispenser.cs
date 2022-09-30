using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pEventBus;

namespace Funksoft.Barista
{
    public class MainDispenser : MonoBehaviour, IClickable
    {
        [SerializeField]
        public MainIngredientData Ingredient;

        [SerializeField]
        public float FillAmountPerSec = 50f;

        [SerializeField]
        private Sprite _hoverSprite;

        [SerializeField]
        private Sprite _clickedSprite;


        public struct Used : IEvent
        {
            public MainIngredientData ingredient;
            public float amount;
        }

        public void OnActivation()
        {
            var useEvent = new Used{ingredient = this.Ingredient, amount = this.FillAmountPerSec * Time.deltaTime};
            EventBus<Used>.Raise(useEvent);
        }
        //Provide name and sprites for this object's clickable component states.
        public string GetDisplayName()
        {
            return Ingredient.Name;
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
