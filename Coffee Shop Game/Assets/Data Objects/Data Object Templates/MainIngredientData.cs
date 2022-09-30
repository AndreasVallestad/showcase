using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Funksoft.Barista
{
    [CreateAssetMenu(menuName = "DataObject/MainIngredient", fileName = "MI_IngredientName")]
    public class MainIngredientData : ScriptableObject
    {
        [SerializeField]
        public string Name;
        [SerializeField]
        public Color InCupColor;
    }
}
