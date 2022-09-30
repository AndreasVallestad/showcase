using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Funksoft.Barista
{
    [CreateAssetMenu(menuName = "DataObject/SideIngredient", fileName = "SI_IngredientName")]
    public class SideIngredientData : ScriptableObject
    {
        [SerializeField]
        public string Name;
    }
}
