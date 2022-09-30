using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pEventBus;

namespace Funksoft.Barista
{
    public class SideDispenser : MonoBehaviour, IClickable
    {
        [SerializeField]
        public SideIngredientData Ingredient;

        [SerializeField]
        private Sprite _hoverSprite;

        [SerializeField]
        private Sprite _clickedSprite;

        public struct Used : IEvent
        {
            public SideIngredientData ingredient;
        }

        //Activated by ClickableObject component when clicked. Completes IClickable interface
        public void OnActivation()
        {
            EventBus<Used>.Raise(new Used{ingredient = this.Ingredient});
        }

        //Provide name and sprites for this object and its clickable component states.
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
