using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Funksoft.Barista
{
    [CreateAssetMenu(menuName = "DataObject/Customer", fileName = "C_CustomerTypeName")]
    public class CustomerData : ScriptableObject
    {        
        [SerializeField]
        public float PatienceTimer; //Amount of time before the customer leaves due to having waited too long for their drink.
        
        [SerializeField, Min(1)]
        public int MaxServeTries; //Max amount of times the player can serve a drink with a mistake before the customer leaves.
        
        [SerializeField, Range(0f, 1f)]
        public float SideIngredientChance; //Chance of the customer wanting side ingredient on their drink. Independant roll for each ingredient, but same chance.

        //Possibly add list of possible drinks to filter in/out of possible selection.

        [SerializeField]
        public Sprite Sprite;
        [SerializeField]
        public Sprite HoverSprite;        
        [SerializeField]
        public Sprite ClickedSprite; 

        [SerializeField]
        public float DisplayPositionHeight = 0f; //How high (y-pos) the character will spawn on-screen, relative the CustomerCounter object that creates the character.



    }
}
